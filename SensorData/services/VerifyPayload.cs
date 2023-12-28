using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SensorData.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.services
{
    public class VerifyPayload
    {
        private readonly ILogger logger;

        public VerifyPayload(ILogger logger)
        {
            this.logger = logger;
        }

        public SuccessStatus successStatus(string requestBody)
        {
            SuccessStatus successStatus = new SuccessStatus();

            IncomingPayload payloadData = JsonConvert.DeserializeObject<IncomingPayload>(requestBody);

            if (payloadData != null )
            {
                successStatus.Success = true;
                successStatus.Payload = payloadData;
            }
            else
            {
                successStatus.Success = false;
                successStatus.Message = "Issuse with payload vallidation";
                successStatus.Stage = "Payload vallidation";
                logger.LogError("Issuse with payload vallidation");
            }

            return successStatus;
        }
    }
}
