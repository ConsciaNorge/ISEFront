using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISEFront.api.ViewModels
{
    public class ServiceProviderViewModel
    {
        [JsonProperty("assertionConsumerServiceUrl")]
        public string AssertionConsumerServiceUrl { get; set; }
        [JsonProperty("wantAuthnRequestSigned")]
        public bool WantAuthnRequestSigned { get; set; }
        [JsonProperty("signSAMLResponse")]
        public bool SignSAMLResponse { get; set; }
        [JsonProperty("signAssertion")]
        public bool SignAssertion { get; set; }
        [JsonProperty("encryptAssertion")]
        public bool EncryptAssertion { get; set; }
        [JsonProperty("assertionLifeTime")]
        public TimeSpan AssertionLifeTime { get; set; }


        [JsonProperty("disableInboundLogout")]
        public bool DisableInboundLogout { get; set; }
        [JsonProperty("disableOutboundLogout")]
        public bool DisableOutboundLogout { get; set; }
        [JsonProperty("disableInResponseToCheck")]
        public bool DisableInResponseToCheck { get; set; }
        [JsonProperty("disablePendingLogoutCheck")]
        public bool DisablePendingLogoutCheck { get; set; }
        [JsonProperty("signLogoutRequest")]
        public bool SignLogoutRequest { get; set; }
        [JsonProperty("signLogoutResponse")]
        public bool SignLogoutResponse { get; set; }
        [JsonProperty("wantLogoutRequestSigned")]
        public bool WantLogoutRequestSigned { get; set; }
        [JsonProperty("wantLogoutResponseSigned")]
        public bool WantLogoutResponseSigned { get; set; }
        [JsonProperty("useEmbeddedCertificate")]
        public bool UseEmbeddedCertificate { get; set; }
        [JsonProperty("issuerFormat")]
        public string IssuerFormat { get; set; }
        [JsonProperty("nameIDFormat")]
        public string NameIDFormat { get; set; }
        [JsonProperty("digestMethod")]
        public string DigestMethod { get; set; }
        [JsonProperty("signatureMethod")]
        public string SignatureMethod { get; set; }
        [JsonProperty("wantDigestMethod")]
        public string WantDigestMethod { get; set; }
        [JsonProperty("wantSignatureMethod")]
        public string WantSignatureMethod { get; set; }
        [JsonProperty("keyEncryptionMethod")]
        public string KeyEncryptionMethod { get; set; }
        [JsonProperty("dataEncryptionMethod")]
        public string DataEncryptionMethod { get; set; }
        [JsonProperty("disableDestinationCheck")]
        public bool DisableDestinationCheck { get; set; }
        [JsonProperty("logoutRequestLifeTime")]
        public TimeSpan LogoutRequestLifeTime { get; set; }
        [JsonProperty("singleLogoutServiceBinding")]
        public string SingleLogoutServiceBinding { get; set; }
        [JsonProperty("singleLogoutServiceResponseUrl")]
        public string SingleLogoutServiceResponseUrl { get; set; }
        [JsonProperty("partnerCertificateFile")]
        public string PartnerCertificateFile { get; set; }
        [JsonProperty("partnerCertificateSerialNumber")]
        public string PartnerCertificateSerialNumber { get; set; }
        [JsonProperty("partnerCertificateThumbprint")]
        public string PartnerCertificateThumbprint { get; set; }
        [JsonProperty("partnerCertificateSubject")]
        public string PartnerCertificateSubject { get; set; }
        [JsonProperty("secondaryPartnerCertificateFile")]
        public string SecondaryPartnerCertificateFile { get; set; }
        [JsonProperty("secondaryPartnerCertificateSerialNumber")]
        public string SecondaryPartnerCertificateSerialNumber { get; set; }
        [JsonProperty("clockSkew")]
        public TimeSpan ClockSkew { get; set; }
        [JsonProperty("secondaryPartnerCertificateThumbprint")]
        public string SecondaryPartnerCertificateThumbprint { get; set; }
        [JsonProperty("tertiaryPartnerCertificateFile")]
        public string TertiaryPartnerCertificateFile { get; set; }
        [JsonProperty("tertiaryPartnerCertificateSerialNumber")]
        public string TertiaryPartnerCertificateSerialNumber { get; set; }
        [JsonProperty("tertiaryPartnerCertificateThumbprint")]
        public string TertiaryPartnerCertificateThumbprint { get; set; }
        [JsonProperty("tertiaryPartnerCertificateSubject")]
        public string TertiaryPartnerCertificateSubject { get; set; }
        [JsonProperty("singleLogoutServiceUrl")]
        public string SingleLogoutServiceUrl { get; set; }
        [JsonProperty("secondaryPartnerCertificateSubject")]
        public string SecondaryPartnerCertificateSubject { get; set; }
        [JsonProperty("authnContext")]
        public string AuthnContext { get; set; }


        [JsonProperty("tertiaryLocalCertificateThumbprint")]
        public string TertiaryLocalCertificateThumbprint { get; set; }
        [JsonProperty("tertiaryLocalCertificateSerialNumber")]
        public string TertiaryLocalCertificateSerialNumber { get; set; }
        [JsonProperty("tertiaryLocalCertificatePasswordKey")]
        public string TertiaryLocalCertificatePasswordKey { get; set; }
        [JsonProperty("tertiaryLocalCertificatePassword")]
        public string TertiaryLocalCertificatePassword { get; set; }
        [JsonProperty("tertiaryLocalCertificateFile")]
        public string TertiaryLocalCertificateFile { get; set; }
        [JsonProperty("secondaryLocalCertificateSubject")]
        public string SecondaryLocalCertificateSubject { get; set; }
        [JsonProperty("secondaryLocalCertificateThumbprint")]
        public string SecondaryLocalCertificateThumbprint { get; set; }
        [JsonProperty("secondaryLocalCertificateSerialNumber")]
        public string SecondaryLocalCertificateSerialNumber { get; set; }
        [JsonProperty("secondaryLocalCertificatePasswordKey")]
        public string SecondaryLocalCertificatePasswordKey { get; set; }
        [JsonProperty("secondaryLocalCertificatePassword")]
        public string SecondaryLocalCertificatePassword { get; set; }
        [JsonProperty("secondaryLocalCertificateFile")]
        public string SecondaryLocalCertificateFile { get; set; }
        [JsonProperty("localCertificateSubject")]
        public string LocalCertificateSubject { get; set; }
        [JsonProperty("localCertificateThumbprint")]
        public string LocalCertificateThumbprint { get; set; }
        [JsonProperty("localCertificateSerialNumber")]
        public string LocalCertificateSerialNumber { get; set; }
        [JsonProperty("localCertificatePasswordKey")]
        public string LocalCertificatePasswordKey { get; set; }
        [JsonProperty("localCertificatePassword")]
        public string LocalCertificatePassword { get; set; }
        [JsonProperty("localCertificateFile")]
        public string LocalCertificateFile { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("tertiaryLocalCertificateSubject")]
        public string TertiaryLocalCertificateSubject { get; set; }
    }
}