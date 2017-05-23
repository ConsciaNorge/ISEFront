using Newtonsoft.Json;
using System;

namespace ISEFront.api.ViewModels
{
    public class BankIDSettingsViewModel
    {
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
        [JsonProperty("clientSecret")]
        public string ClientSecret { get; set; }
        [JsonProperty("oidcBaseUrl")]
        public Uri OIDCBaseUrl { get; set; }
        [JsonProperty("redirectUrl")]
        public Uri RedirectUrl { get; set; }
        [JsonProperty("manifestUrl")]
        public Uri ManifestUrl { get; set; }
        [JsonProperty("authenticationType")]
        public string AuthenticationType { get; set; }
        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}