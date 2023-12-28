using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SensorData.model;

namespace SensorData.services
{
    public class DatabaseService
    {
        private readonly ILogger logger;
        private readonly DbSettings dbSettings;
        private CosmosClient client;

        public DatabaseService(ILogger logger, IConfiguration configuration)
        {
            this.logger = logger;
            dbSettings = new DbSettings();
            dbSettings.EndPointUri = configuration["DB_Settings:EndPointUri"];
            dbSettings.Key = configuration["DB_Settings:Key"];
            client = new(dbSettings.EndPointUri, dbSettings.Key);
        }

        public async Task<SuccessStatus> AddValsToDbAsync(string databaseName, string containerName, IncomingPayload payload)
        {
            SuccessStatus successStatus = new SuccessStatus();
            try
            {
                Microsoft.Azure.Cosmos.Database database = client.GetDatabase(databaseName);
                Microsoft.Azure.Cosmos.Container container = database.GetContainer(containerName);

                payload.SensorItems.ForEach(async item =>
                {
                    var dbItem = new SensorDataDbItem();
                    dbItem.Id = Guid.NewGuid();
                    dbItem.PartitionKey = payload.Site;
                    dbItem.Site = payload.Site;
                    dbItem.SensorId = item.SensorId;
                    dbItem.SensorType = item.SensorType;
                    dbItem.Value = item.SensorValue;

                    ItemResponse<SensorDataDbItem> response = await container.UpsertItemAsync<SensorDataDbItem>(
                        item: dbItem,
                        partitionKey: new PartitionKey(dbItem.Site)
                    );
                    logger.LogInformation(response.ToString());

                });

                successStatus.Success = true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                successStatus.Success = false;
                successStatus.Message = ex.ToString();
                successStatus.Stage = "Add values to DB";
            }

            return successStatus;
        }
    }
}
