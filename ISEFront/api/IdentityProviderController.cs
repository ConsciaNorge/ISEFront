using ISEFront.api.ViewModels;
using ISEFront.Utility.Security;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

            }
        }

        public HttpResponseMessage Get()
        {
            var x = ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration;

            var certFilename = HttpContext.Current.Server.MapPath("~\\" + x.LocalCertificateFile);
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
                    SubjectPublicKey = ExportPublicKeyToPEMFormat(cert.PublicKey.Key as RSA)
                });
            }

            var result = new IdentityProviderViewModel
            {
                Name = x.Name,
                Description = x.Description,
                Certificates = certificates
                //Certificate = cert == null ? null : new CertificateViewModel
                //{
                //    Version = cert.Version,
                //    SerialNumber = cert.SerialNumber,
                //    SignatureAlgorithmId = cert.SignatureAlgorithm.FriendlyName,
                //    SubjectName = cert.SubjectName.Name,
                //    Issuer = cert.Issuer,
                //    IssuerName = cert.IssuerName.Name,
                //    NotBefore = cert.NotBefore,
                //    NotAfter = cert.NotAfter,
                //    PublicKeyAlgorithm = cert.PublicKey.Key.SignatureAlgorithm,
                //    SubjectPublicKey = ExportPublicKeyToPEMFormat(cert.PublicKey.Key as RSA)
                //}
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

            var certificatePathString = HttpContext.Current.Server.MapPath("~/Certificates");
            var certificateFileName = Guid.NewGuid().ToString() + ".pfx";
            var certificateFilePath = Path.Combine(certificatePathString, certificateFileName);

            SelfSignedCertificate.Save(certificateFilePath, certificateData);

            ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration.LocalCertificatePassword = details.PrivateKeyPassword;
            ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration.LocalCertificateFile = Path.Combine("Certificates", certificateFileName);

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

            var certificatePathString = HttpContext.Current.Server.MapPath("~/Certificates");
            var certificateFileName = Guid.NewGuid().ToString() + ".pfx";
            var certificateFilePath = Path.Combine(certificatePathString, certificateFileName);

            SelfSignedCertificate.Save(certificateFilePath, certificateData);

            var newConfiguration = new ComponentSpace.SAML2.Configuration.SAMLConfiguration();

            var localIdp = new ComponentSpace.SAML2.Configuration.LocalIdentityProviderConfiguration();
            localIdp.Name = idpConfig.IdpName;
            localIdp.Description = idpConfig.IdpDescription;
            localIdp.LocalCertificatePassword = idpConfig.IdpCertificateParameters.PrivateKeyPassword;
            localIdp.LocalCertificateFile = Path.Combine("Certificates", certificateFileName);
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
                SubjectPublicKey = ExportPublicKeyToPEMFormat(cert.PublicKey.Key as RSA)
            };
        }

        public static String ExportPublicKeyToPEMFormat(RSA csp)
        {
            TextWriter outputStream = new StringWriter();

            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);

                    //All Parameter Must Have Value so Set Other Parameter Value Whit Invalid Data  (for keeping Key Structure  use "parameters.Exponent" value for invalid data)
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.D
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.P
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.Q
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.DP
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.DQ
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.InverseQ

                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                outputStream.WriteLine("-----BEGIN PUBLIC KEY-----");
                // Output as Base64 with lines chopped at 64 characters
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                }
                outputStream.WriteLine("-----END PUBLIC KEY-----");

                return outputStream.ToString();

            }
        }

        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }
    }
}