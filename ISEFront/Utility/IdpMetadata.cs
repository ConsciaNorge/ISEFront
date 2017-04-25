using ComponentSpace.SAML2;
using ComponentSpace.SAML2.Metadata;
using ComponentSpace.SAML2.Utility;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace ISEFront.Utility
{
    public class IdpMetadata
    {
        // Creates a KeyInfo from the supplied X.509 certificate
        private static KeyInfo CreateKeyInfo(X509Certificate2 x509Certificate)
        {
            KeyInfoX509Data keyInfoX509Data = new KeyInfoX509Data();
            keyInfoX509Data.AddCertificate(x509Certificate);

            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(keyInfoX509Data);

            return keyInfo;
        }

        // Creates a KeyDescriptor from the supplied X.509 certificate
        private static KeyDescriptor CreateKeyDescriptor(X509Certificate2 x509Certificate)
        {
            KeyDescriptor keyDescriptor = new KeyDescriptor();
            KeyInfo keyInfo = CreateKeyInfo(x509Certificate);
            keyDescriptor.KeyInfo = keyInfo.GetXml();

            // Set the encryption method by specifying the entire XML.
            //XmlDocument xmlDocument = new XmlDocument();
            //xmlDocument.PreserveWhitespace = true;
            //xmlDocument.LoadXml("<md:EncryptionMethod xmlns:md=\"urn:oasis:names:tc:SAML:2.0:metadata\" Algorithm=\"http://www.w3.org/2001/04/xmlenc#aes256-cbc\"/>");
            //keyDescriptor.EncryptionMethods.Add(xmlDocument.DocumentElement);

            // Set the encryption method by specifying just the algorithm.
            //keyDescriptor.AddEncryptionMethod("http://www.w3.org/2001/04/xmlenc#aes256-cbc");

            return keyDescriptor;
        }

        // Creates an IdP SSO descriptor
        private static IDPSSODescriptor CreateIDPSSODescriptor(
                X509Certificate2 idpCertificate,
                Uri artifactResolutionServiceUrl,
                Uri singleSignOnServiceUrl,
                Uri singleLogoutServiceUrl
            )
        {
            IDPSSODescriptor idpSSODescriptor = new IDPSSODescriptor();
            idpSSODescriptor.WantAuthnRequestsSigned = true;
            idpSSODescriptor.ProtocolSupportEnumeration = SAML.NamespaceURIs.Protocol;

            //X509Certificate2 x509Certificate = new X509Certificate2(idpCertificateFilename);
            idpSSODescriptor.KeyDescriptors.Add(CreateKeyDescriptor(idpCertificate));

            IndexedEndpointType artifactResolutionService = new IndexedEndpointType(1, true);
            artifactResolutionService.Binding = SAMLIdentifiers.BindingURIs.SOAP;
            artifactResolutionService.Location = artifactResolutionServiceUrl.ToString();

            idpSSODescriptor.ArtifactResolutionServices.Add(artifactResolutionService);

            idpSSODescriptor.NameIDFormats.Add(SAMLIdentifiers.NameIdentifierFormats.Transient);

            EndpointType singleSignOnService = new EndpointType(SAMLIdentifiers.BindingURIs.HTTPRedirect, singleSignOnServiceUrl.ToString(), null);
            idpSSODescriptor.SingleSignOnServices.Add(singleSignOnService);

            EndpointType singleLogoutService = new EndpointType(SAMLIdentifiers.BindingURIs.HTTPRedirect, singleLogoutServiceUrl.ToString(), null);
            idpSSODescriptor.SingleLogoutServices.Add(singleLogoutService);

            return idpSSODescriptor;
        }

        // Creates an IdP entity descriptor
        public static EntityDescriptor CreateIDPEntityDescriptor(
            X509Certificate2 idpCertificate,
            Uri entityUrl,
            Uri artifactResolutionServiceUrl,
            Uri singleSignonUrl,
            Uri singleLogoutUrl)
        {
            EntityDescriptor entityDescriptor = new EntityDescriptor();
            entityDescriptor.EntityID = new EntityIDType(entityUrl.ToString());
            entityDescriptor.IDPSSODescriptors.Add(
                CreateIDPSSODescriptor(
                    idpCertificate, 
                    artifactResolutionServiceUrl, 
                    singleSignonUrl, 
                    singleLogoutUrl
                )
            );

            Organization organization = new Organization();
            organization.OrganizationNames.Add(new OrganizationName("IdP", "en"));
            organization.OrganizationDisplayNames.Add(new OrganizationDisplayName("IdP", "en"));
            organization.OrganizationURLs.Add(new OrganizationURL(entityUrl.ToString(), "en"));
            entityDescriptor.Organization = organization;

            ContactPerson contactPerson = new ContactPerson();
            contactPerson.ContactTypeValue = "technical";
            contactPerson.GivenName = "Joe";
            contactPerson.Surname = "User";
            contactPerson.EmailAddresses.Add("joe.user@idp.com");
            entityDescriptor.ContactPeople.Add(contactPerson);

            return entityDescriptor;
        }

        static public byte[] GenerateIDPMetadata(
                string baseUrl,
                string artifactResolutionServicePath = "/ArtifactResolutionService",
                string singleSignonServicePath = "/saml/ssoservice",
                string singleLogoutServicePath = "/saml/sloservice"
            )
        {
            var idpCertificate = SAMLController.CertificateManager.GetLocalIdentityProviderSignatureCertificates("default", null).FirstOrDefault();
            if (idpCertificate == null)
                return null;

            var artifactResolutionServiceUrl = new UriBuilder(baseUrl);
            artifactResolutionServiceUrl.Path = artifactResolutionServicePath;

            var singleSignonServiceUrl = new UriBuilder(baseUrl);
            singleSignonServiceUrl.Path = singleSignonServicePath;

            var singleLogoutServiceUrl = new UriBuilder(baseUrl);
            singleLogoutServiceUrl.Path = singleLogoutServicePath;

            var entityUrl = new UriBuilder(baseUrl);

            var descriptor = CreateIDPEntityDescriptor(
                    idpCertificate,
                    entityUrl.Uri,
                    artifactResolutionServiceUrl.Uri,
                    singleSignonServiceUrl.Uri,
                    singleLogoutServiceUrl.Uri
                );

            var xmlElement = descriptor.ToXml();

            SAMLMetadataSignature.Generate(xmlElement, idpCertificate.PrivateKey, idpCertificate);

            var xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.Encoding = new UTF8Encoding(false);

            var stringWriter = new MemoryStream();

            var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
            xmlElement.OwnerDocument.WriteContentTo(xmlWriter);
            xmlWriter.Flush();

            stringWriter.Flush();
            stringWriter.Position = 0;

            return stringWriter.GetBuffer();
        }
    }
}