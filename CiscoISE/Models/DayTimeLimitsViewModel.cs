using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class DayTimeLimitsViewModel
    {
        [JsonProperty("startTime")]
        public TimeSpan StartTime { get; set; }
        [JsonProperty("endTime")]
        public TimeSpan EndTime { get; set; }
        [JsonProperty("days")]
        public List<String> Days { get; set; }
    }
}
