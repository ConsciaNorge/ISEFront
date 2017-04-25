using ComponentSpace.SAML2.Exceptions;
using ComponentSpace.SAML2.Metadata;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;

namespace ISEFront.api.ViewModels
{
    public class ServiceProvidersController : ApiController
    {
        public ServiceProvidersController()
        {
            ComponentSpace.SAML2.SAMLController.Initialize();
        }

        // GET api/<controller>
        public HttpResponseMessage Get()
        {
            var result = ComponentSpace.SAML2.SAMLController.Configuration.PartnerServiceProviderConfigurations
                .Select(x => new ServiceProviderBriefItem
                {
                    Id = x.Value.Name.GetHashCode(),
                    Name = x.Value.Name,
                    Description = x.Value.Description,
                    WantAuthnRequestSigned = x.Value.WantAuthnRequestSigned,
                    SignSAMLResponse = x.Value.SignSAMLResponse,
                    SignAssertion = x.Value.SignAssertion,
                    EncryptAssertion = x.Value.EncryptAssertion
                })
                .ToList();

            var jsonText = JsonConvert.SerializeObject(result);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            return response;
        }

        [HttpGet]
        [Route("api/serviceprovider/{id:int}")]
        public HttpResponseMessage Get(int id)
        {
            var result = ComponentSpace.SAML2.SAMLController.Configuration.PartnerServiceProviderConfigurations
                .Where(x => x.Value.Name.GetHashCode() == id)
                .Select(x => new ServiceProviderViewModel
                {
                    AssertionConsumerServiceUrl = x.Value.AssertionConsumerServiceUrl,
                    WantAuthnRequestSigned = x.Value.WantAuthnRequestSigned,
                    SignSAMLResponse = x.Value.SignSAMLResponse,
                    SignAssertion = x.Value.SignAssertion,
                    EncryptAssertion = x.Value.EncryptAssertion,
                    AssertionLifeTime = x.Value.AssertionLifeTime,


                    DisableInboundLogout = x.Value.DisableInboundLogout,
                    DisableOutboundLogout = x.Value.DisableOutboundLogout,
                    DisableInResponseToCheck = x.Value.DisableInResponseToCheck,
                    DisablePendingLogoutCheck = x.Value.DisablePendingLogoutCheck,
                    SignLogoutRequest = x.Value.SignLogoutRequest,
                    SignLogoutResponse = x.Value.SignLogoutResponse,
                    WantLogoutRequestSigned = x.Value.WantLogoutRequestSigned,
                    WantLogoutResponseSigned = x.Value.WantLogoutResponseSigned,
                    UseEmbeddedCertificate = x.Value.UseEmbeddedCertificate,
                    IssuerFormat = x.Value.IssuerFormat,
                    NameIDFormat = x.Value.NameIDFormat,
                    DigestMethod = x.Value.DigestMethod,
                    SignatureMethod = x.Value.SignatureMethod,
                    WantDigestMethod = x.Value.WantDigestMethod,
                    WantSignatureMethod = x.Value.WantSignatureMethod,
                    KeyEncryptionMethod = x.Value.KeyEncryptionMethod,
                    DataEncryptionMethod = x.Value.DataEncryptionMethod,
                    DisableDestinationCheck = x.Value.DisableDestinationCheck,
                    LogoutRequestLifeTime = x.Value.LogoutRequestLifeTime,
                    SingleLogoutServiceBinding = x.Value.SingleLogoutServiceBinding,
                    SingleLogoutServiceResponseUrl = x.Value.SingleLogoutServiceResponseUrl,
                    PartnerCertificateFile = x.Value.PartnerCertificateFile,
                    PartnerCertificateSerialNumber = x.Value.PartnerCertificateSerialNumber,
                    PartnerCertificateThumbprint = x.Value.PartnerCertificateThumbprint,
                    PartnerCertificateSubject = x.Value.PartnerCertificateSubject,
                    SecondaryPartnerCertificateFile = x.Value.SecondaryPartnerCertificateFile,
                    SecondaryPartnerCertificateSerialNumber = x.Value.SecondaryPartnerCertificateSerialNumber,
                    ClockSkew = x.Value.ClockSkew,
                    SecondaryPartnerCertificateThumbprint = x.Value.SecondaryPartnerCertificateThumbprint,
                    TertiaryPartnerCertificateFile = x.Value.TertiaryPartnerCertificateFile,
                    TertiaryPartnerCertificateSerialNumber = x.Value.TertiaryPartnerCertificateSerialNumber,
                    TertiaryPartnerCertificateThumbprint = x.Value.TertiaryPartnerCertificateThumbprint,
                    TertiaryPartnerCertificateSubject = x.Value.TertiaryPartnerCertificateSubject,
                    SingleLogoutServiceUrl = x.Value.SingleLogoutServiceUrl,
                    SecondaryPartnerCertificateSubject = x.Value.SecondaryPartnerCertificateSubject,
                    AuthnContext = x.Value.AuthnContext,


                    TertiaryLocalCertificateThumbprint = x.Value.TertiaryLocalCertificateThumbprint,
                    TertiaryLocalCertificateSerialNumber = x.Value.TertiaryLocalCertificateSerialNumber,
                    TertiaryLocalCertificatePasswordKey = x.Value.TertiaryLocalCertificatePasswordKey,
                    TertiaryLocalCertificatePassword = x.Value.TertiaryLocalCertificatePassword,
                    TertiaryLocalCertificateFile = x.Value.TertiaryLocalCertificateFile,
                    SecondaryLocalCertificateSubject = x.Value.SecondaryLocalCertificateSubject,
                    SecondaryLocalCertificateThumbprint = x.Value.SecondaryLocalCertificateThumbprint,
                    SecondaryLocalCertificateSerialNumber = x.Value.SecondaryLocalCertificateSerialNumber,
                    SecondaryLocalCertificatePasswordKey = x.Value.SecondaryLocalCertificatePasswordKey,
                    SecondaryLocalCertificatePassword = x.Value.SecondaryLocalCertificatePassword,
                    SecondaryLocalCertificateFile = x.Value.SecondaryLocalCertificateFile,
                    LocalCertificateSubject = x.Value.LocalCertificateSubject,
                    LocalCertificateThumbprint = x.Value.LocalCertificateThumbprint,
                    LocalCertificateSerialNumber = x.Value.LocalCertificateSerialNumber,
                    LocalCertificatePasswordKey = x.Value.LocalCertificatePasswordKey,
                    LocalCertificatePassword = x.Value.LocalCertificatePassword,
                    LocalCertificateFile = x.Value.LocalCertificateFile,
                    Description = x.Value.Description,
                    Name = x.Value.Name,
                    TertiaryLocalCertificateSubject = x.Value.TertiaryLocalCertificateSubject
                })
                .FirstOrDefault();

            var jsonText = JsonConvert.SerializeObject(result);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            return response;
        }

        [Route("api/serviceprovider/spmetadataupload")]
        [HttpPost]
        [ValidateMimeMultipartContentFilter]
        public async Task<string> UploadSingleFile()
        {
            var result = "ok";
            var streamProvider = new MultipartMemoryStreamProvider();
            await Request.Content.ReadAsMultipartAsync<MultipartMemoryStreamProvider>(streamProvider).ContinueWith(async (tsk) =>
            {

                foreach (HttpContent ctnt in streamProvider.Contents)
                {
                    // You would get hold of the inner memory stream here
                    Stream stream = await ctnt.ReadAsStreamAsync();

                    // do something witht his stream now
                    System.Diagnostics.Debug.WriteLine("piece transferred - " + ctnt.Headers.ContentDisposition.FileName);

                    if(ctnt.Headers.ContentType.MediaType != "text/xml")
                    {
                        System.Diagnostics.Debug.WriteLine("Not an XML file");
                        result = "fail";
                        continue;
                    }

                    stream.Position = 0;

                    var xmlDocument = new XmlDocument();
                    xmlDocument.PreserveWhitespace = true;
                    xmlDocument.Load(stream);

                    try
                    {
                        if (SAMLMetadataSignature.Verify(xmlDocument.DocumentElement))
                        {
                            System.Diagnostics.Debug.WriteLine("Valid metadata signature");
                        }
                    }
                    catch(SAMLSignatureException e)
                    {
                        System.Diagnostics.Debug.WriteLine(e.Message);
                    }

                    if (EntityDescriptor.IsValid(xmlDocument.DocumentElement))
                    {
                        var entityDescriptor = new EntityDescriptor(xmlDocument.DocumentElement);
                        IList<X509Certificate2> x509Certificates = new List<X509Certificate2>();

                        var certificatePath = new DirectoryInfo(System.Web.Hosting.HostingEnvironment.MapPath("~/Certificates"));

                        ComponentSpace.SAML2.Configuration.MetadataImporter.Import(entityDescriptor, ComponentSpace.SAML2.SAMLController.Configuration, x509Certificates);

                        var partner = ComponentSpace.SAML2.SAMLController.Configuration.GetPartnerServiceProvider(entityDescriptor.EntityID.URI);
                        foreach(var certificate in x509Certificates)
                        {
                            var fileName = CreateCertificateFileName(certificate);
                            ComponentSpace.SAML2.Configuration.MetadataImporter.SaveCertificate(certificate, Path.Combine(certificatePath.FullName, fileName));
                            partner.PartnerCertificateFile = Path.Combine("Certificates", fileName);
                        }

                        ComponentSpace.SAML2.Configuration.SAMLConfigurationFile.Save(ComponentSpace.SAML2.SAMLController.Configuration);

                        var wtf = ComponentSpace.SAML2.SAMLController.CertificateManager.GetPartnerServiceProviderSignatureCertificates("default", partner.Name);

                        result = "valid entity descriptor";
                    }
                }
            });

            return "ok";
        }

        private static string CreateCertificateFileName(X509Certificate2 x509Certificate)
        {
            string subjectName = Regex.Replace(x509Certificate.Subject, "CN=", "", RegexOptions.IgnoreCase);

            return string.Format("{0}.cer", Regex.Replace(subjectName, @"[^A-Za-z0-9_=\.]+", "_"));
        }
    }
}