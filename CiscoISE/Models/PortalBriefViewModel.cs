using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiscoISE.Models
{
    public class PortalBriefViewModel
    {
        [JsonProperty("id")]
        public Guid? Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description", NullValueHandling=NullValueHandling.Ignore)]
        public string Description { get; set; }
        [JsonProperty("link", NullValueHandling = NullValueHandling.Ignore)]
        public LinkViewModel Link { get; set; }
    }
}
