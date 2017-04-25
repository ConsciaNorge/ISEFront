using Newtonsoft.Json;
using System.Collections.Generic;

namespace ISEFront.api.ViewModels
{
    public class IdentityProviderViewModel
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("certificates")]
        public List<CertificateViewModel> Certificates { get; set; }
    }
}