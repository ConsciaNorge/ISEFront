using ISEFront.api.ViewModels;
using ISEFront.Utility.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Http;

namespace ISEFront.api
{
    public class IdentityProviderController : ApiController
    {
        public IdentityProviderController()
        {
            try
            {
                ComponentSpace.SAML2.SAMLController.Initialize();
            }
            catch (ComponentSpace.SAML2.Exceptions.SAMLConfigurationException e)
            {
                Trace.WriteLine("Failed to load saml.config - " + e.Message);
            }
        }

        public HttpResponseMessage Get()
        {
            var x = ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration;

            // TODO : Map path better?
            var certFilename = HttpContext.Current.Server.MapPath("~/" + x.LocalCertificateFile);
            var certificateCollection = new X509Certificate2Collection();
            certificateCollection.Import(certFilename, x.LocalCertificatePassword, X509KeyStorageFlags.DefaultKeySet);

            var certificates = new List<CertificateViewModel>();
            foreach(var cert in certificateCollection)
            {
                certificates.Add(new CertificateViewModel
                {
                    Version = cert.Version,
                    SerialNumber = cert.SerialNumber,
                    SignatureAlgorithmId = cert.SignatureAlgorithm.FriendlyName,
                    SubjectName = cert.SubjectName.Name,
                    Issuer = cert.Issuer,
                    IssuerName = cert.IssuerName.Name,
                    NotBefore = cert.NotBefore,
                    NotAfter = cert.NotAfter,
                    PublicKeyAlgorithm = cert.PublicKey.Key.SignatureAlgorithm,
                    SubjectPublicKey = CertificateTools.ExportPublicKeyToPEMFormat(cert.PublicKey.Key as RSA)
                });
            }

            var result = new IdentityProviderViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Certificates = certificates
            };

            var jsonText = JsonConvert.SerializeObject(result);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            return response;
        }

        [HttpPost]
        [Route("api/identityprovider/generatecertificate")]
        public HttpResponseMessage PostGenerateCertificate(CertificateGenerationDetailsViewModel details)
        {
            var certificateData = SelfSignedCertificate.GenerateSelfSigned(
                    details.SubjectName,
                    details.IssuerName,
                    details.NotBefore,
                    details.NotAfter,
                    details.PrivateKeyPassword,
                    details.HashAlgorithm
                );

            var certificatePathString = HttpContext.Current.Server.MapPath("~/app_data/Certificates");
            var certificateFileName = Guid.NewGuid().ToString() + ".pfx";
            var certificateFilePath = Path.Combine(certificatePathString, certificateFileName);

            SelfSignedCertificate.Save(certificateFilePath, certificateData);

            ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration.LocalCertificatePassword = details.PrivateKeyPassword;
            // TODO : Consider a cleaner method of combining paths here.
            ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration.LocalCertificateFile = Path.Combine("app_data", Path.Combine("Certificates", certificateFileName));

            var result = CertificateToViewModel(ComponentSpace.SAML2.SAMLController.CertificateManager.GetLocalIdentityProviderSignatureCertificates("default", null).FirstOrDefault());

            ComponentSpace.SAML2.Configuration.SAMLConfigurationFile.Save(ComponentSpace.SAML2.SAMLController.Configuration);

            var jsonText = JsonConvert.SerializeObject(result);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            return response;
        }

        [HttpPut]
        public HttpResponseMessage PutIdentityProvider(IdentityProviderConfigurationViewModel idpConfig)
        {
            var certificateData = SelfSignedCertificate.GenerateSelfSigned(
                    idpConfig.IdpCertificateParameters.SubjectName,
                    idpConfig.IdpCertificateParameters.IssuerName,
                    idpConfig.IdpCertificateParameters.NotBefore,
                    idpConfig.IdpCertificateParameters.NotAfter,
                    idpConfig.IdpCertificateParameters.PrivateKeyPassword,
                    idpConfig.IdpCertificateParameters.HashAlgorithm
                );

            var certificatePathString = HttpContext.Current.Server.MapPath("~/app_data/Certificates");
            var certificateFileName = Guid.NewGuid().ToString() + ".pfx";
            var certificateFilePath = Path.Combine(certificatePathString, certificateFileName);

            SelfSignedCertificate.Save(certificateFilePath, certificateData);

            var newConfiguration = new ComponentSpace.SAML2.Configuration.SAMLConfiguration();

            var localIdp = new ComponentSpace.SAML2.Configuration.LocalIdentityProviderConfiguration();
            localIdp.Name = idpConfig.IdpName;
            localIdp.Description = idpConfig.IdpDescription;
            localIdp.LocalCertificatePassword = idpConfig.IdpCertificateParameters.PrivateKeyPassword;
            // TODO : Consider a cleaner method of combining paths here.
            localIdp.LocalCertificateFile = Path.Combine("app_data", Path.Combine("Certificates", certificateFileName));
            newConfiguration.LocalIdentityProviderConfiguration = localIdp;

            ComponentSpace.SAML2.SAMLController.Configurations.Add("default", newConfiguration);
            ComponentSpace.SAML2.Configuration.SAMLConfigurationFile.Save(ComponentSpace.SAML2.SAMLController.Configuration);

            var result = "ok";

            var jsonText = JsonConvert.SerializeObject(result);

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(jsonText, Encoding.UTF8, "application/json");

            return response;
        }

        public CertificateViewModel CertificateToViewModel(X509Certificate2 cert)
        {
            return cert == null ? null : new CertificateViewModel
            {
                Version = cert.Version,
                SerialNumber = cert.SerialNumber,
                SignatureAlgorithmId = cert.SignatureAlgorithm.FriendlyName,
                SubjectName = cert.SubjectName.Name,
                Issuer = cert.Issuer,
                IssuerName = cert.IssuerName.Name,
                NotBefore = cert.NotBefore,
                NotAfter = cert.NotAfter,
                PublicKeyAlgorithm = cert.PublicKey.Key.SignatureAlgorithm,
                SubjectPublicKey = CertificateTools.ExportPublicKeyToPEMFormat(cert.PublicKey.Key as RSA)
            };
        }
    }
}