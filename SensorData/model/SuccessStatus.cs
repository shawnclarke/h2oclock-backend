using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.model
{
    public class SuccessStatus
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public string? Stage { get; set; }
        public IncomingPayload? Payload { get; set; }
    }
}
