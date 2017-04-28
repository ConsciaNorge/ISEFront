using Newtonsoft.Json;
using System;

namespace libciscoise
{
    public class GuestAccessInfoViewModel
    {
        [JsonProperty("validDays")]
        public int ValidDays { get; set; }
        [JsonProperty("fromDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset FromDate { get; set; }
        [JsonProperty("toDate", NullValueHandling = NullValueHandling.Ignore)]
        public DateTimeOffset ToDate { get; set; }
        [JsonProperty("location", NullValueHandling = NullValueHandling.Ignore)]
        public string Location { get; set; }
        [JsonProperty("ssid", NullValueHandling = NullValueHandling.Ignore)]
        public string SSID { get; set; }
        [JsonProperty("groupTag", NullValueHandling = NullValueHandling.Ignore)]
        public string GroupTag { get; set; }
    }
}