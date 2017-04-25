using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.api.ViewModels
{
    public class ServiceProviderBriefItem
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("wantAuthnRequestSigned")]
        public bool WantAuthnRequestSigned { get; set; }
        [JsonProperty("signSAMLResponse")]
        public bool SignSAMLResponse { get; set; }
        [JsonProperty("signAssertion")]
        public bool SignAssertion { get; set; }
        [JsonProperty("encryptAssertion")]
        public bool EncryptAssertion { get; set; }
    }
}