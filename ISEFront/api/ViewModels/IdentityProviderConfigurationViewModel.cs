using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.api.ViewModels
{
    public class IdentityProviderConfigurationViewModel
    {
        [JsonProperty("idpName")]
        public string IdpName { get; set; }
        [JsonProperty("idpDescription")]
        public string IdpDescription { get; set; }
        [JsonProperty("idpCertificateParameters")]
        public CertificateGenerationDetailsViewModel IdpCertificateParameters { get; set; }
    }
}