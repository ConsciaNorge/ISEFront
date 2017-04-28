using Newtonsoft.Json;
using System;

namespace libciscoise
{
    public class GuestUserBriefViewModel
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public LinkViewModel Link { get; set; }
    }
}