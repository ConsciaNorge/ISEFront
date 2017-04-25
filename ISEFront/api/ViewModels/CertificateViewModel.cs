using Newtonsoft.Json;
using System;

namespace ISEFront.api.ViewModels
{
    public class CertificateViewModel
    {
        [JsonProperty("version")]
        public int Version { get; set; }
        [JsonProperty("serialNumber")]
        public string SerialNumber { get; set; }
        [JsonProperty("signatureAlgorithmId")]
        public string SignatureAlgorithmId { get; set; }
        [JsonProperty("subjectName")]
        public string SubjectName { get; set; }
        [JsonProperty("issuer")]
        public string Issuer { get; set; }
        [JsonProperty("issuerName")]
        public string IssuerName { get; set; }
        [JsonProperty("notBefore")]
        public DateTimeOffset NotBefore { get; set; }
        [JsonProperty("notAfter")]
        public DateTimeOffset NotAfter { get; set; }
        [JsonProperty("publicKeyAlgorithm")]
        public string PublicKeyAlgorithm { get; set; }
        [JsonProperty("subjectPublicKey")]
        public string SubjectPublicKey { get; set; }
    }
}