using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class GuestTypeAccessTimeViewModel
    {
        [JsonProperty("fromFirstLogin")]
        public bool FromFirstLogin { get; set; }
        [JsonProperty("maxAccountDuration")]
        public int MaxAccountDuration { get; set; }
        [JsonProperty("durationTimeUnit")]
        public string DurationTimeUnit { get; set; }
        [JsonProperty("defaultDuration")]
        public int DefaultDuration { get; set; }
        [JsonProperty("allowAccessOnSpecificDayTimes")]
        public bool AllowAccessOnSpecificDayTimes { get; set; }
        [JsonProperty("dayTimeLimits", NullValueHandling = NullValueHandling.Ignore)]
        public List<DayTimeLimitsViewModel> DayTimeLimits { get; set; }
    }
}
