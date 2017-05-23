using ISEFront.Utility.Json.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace ISEFront.api.ViewModels
{
    public class ISEServerSettingsViewModel
    {
        [JsonConverter(typeof(IPAddressConverter))]
        [JsonProperty("serverIP")]
        public IPAddress ServerIP { get; set; }
        [JsonProperty("apiUsername")]
        public string ApiUsername { get; set; }
        [JsonProperty("apiPassword")]
        public string ApiPassword { get; set; }
        [JsonProperty("sponsorPortalUsername")]
        public string SponsorPortalUsername { get; set; }
        [JsonProperty("sponsorPortalPassword")]
        public string SponsorPortalPassword { get; set; }
    }
}