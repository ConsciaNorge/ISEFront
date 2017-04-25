using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.api.ViewModels
{
    public class CertificateGenerationDetailsViewModel
    {
        [JsonProperty("subjectName")]
        public string SubjectName { get; set; }
        [JsonProperty("issuerName")]
        public string IssuerName { get; set; }
        [JsonProperty("notBefore")]
        public DateTimeOffset NotBefore { get; set; }
        [JsonProperty("notAfter")]
        public DateTimeOffset NotAfter { get; set; }
        [JsonProperty("privateKeyPassword")]
        public string PrivateKeyPassword { get; set; }
        [JsonProperty("keyStrength")]
        public int KeyStrength { get; set; }
        [JsonProperty("hashAlgorithm")]
        public string HashAlgorithm { get; set; }
    }
}