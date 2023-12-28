using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorData.model
{
    public class SensorDataDbItem
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("partitionKey")]
        public string PartitionKey { get; set; }
        public string Site { get; set; }
        public string SensorId { get; set; }
        public string SensorType { get; set; }
        public string Value { get; set; }
    }
}
