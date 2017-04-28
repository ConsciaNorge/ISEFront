using Newtonsoft.Json;
using System;

namespace CiscoISE.Models
{
    public class GuestUserBriefViewModel
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public Guid? Id { get; set; }
        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }
        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public LinkViewModel Link { get; set; }
    }
}