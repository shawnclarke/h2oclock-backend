using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.model
{
    public class IncomingPayload
    {
        public string? DeviceId { get; set; }
        public string? Site { get; set; }
        public List<SensorItem>? SensorItems { get; set; }
    }
}
