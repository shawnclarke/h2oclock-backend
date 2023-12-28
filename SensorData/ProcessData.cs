using System.ComponentModel;
using System.Net;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SensorData.model;
using Microsoft.Extensions.Configuration;
using SensorData.services;

namespace SensorData
{
    public class ProcessData
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ProcessData(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.CreateLogger<ProcessData>();
            _configuration = configuration;
        }

        [Function("ProcessData")]
        public async Task<HttpResponseData> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "sensorvals")] HttpRequestData req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            HttpResponseData response;

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            _logger.LogInformation("Got request body");

            VerifyPayload verifyPayload = new VerifyPayload(_logger);

            SuccessStatus payloadData = verifyPayload.successStatus(requestBody);

            _logger.LogInformation("Verified payload");

            if (payloadData.Success)
            {
                DatabaseService database = new DatabaseService(_logger, _configuration);
                SuccessStatus addData = await database.AddValsToDbAsync("h2oclock", "sensorVals1", payloadData.Payload);
                if (addData.Success)
                {
                    _logger.LogInformation("Successfully added data");
                    response = req.CreateResponse(HttpStatusCode.OK);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                    response.WriteString("Data successfully added to DB");
                }
                else
                {
                    response = req.CreateResponse(HttpStatusCode.InternalServerError);
                    response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                    response.WriteString(addData.Message);
                }
            }
            else
            {
                response = req.CreateResponse(HttpStatusCode.InternalServerError);
                response.Headers.Add("Content-Type", "text/plain; charset=utf-8");
                response.WriteString(payloadData.Message);
            }

            return response;
        }
    }
}
