using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ISEFront.Tests.Security
{
    [TestClass]
    public class CertificateTest
    {
        //[TestMethod]
        //public void TestMethod1()
        //{
            //Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair caKeyPair = null;
            //System.Security.Cryptography.X509Certificates.X509Certificate2 caRootCertificate =
            //    Utility.Security.SelfSignedCertificate.GenerateSelfSignedCertificate(
            //        "CN=MYTESTCA",
            //        "CN=MYTESTCA",
            //        notBefore,
            //        notAfter,
            //        null,
            //        ref caKeyPair);

            ////generate cert based on the CA cert privateKey
            //Org.BouncyCastle.Crypto.AsymmetricCipherKeyPair certificateKeyPair = null;
            //System.Security.Cryptography.X509Certificates.X509Certificate2 MyCert = 
            //    Utility.Security.SelfSignedCertificate.GenerateSelfSignedCertificate(
            //        "CN=127.0.01", 
            //        "CN=MYTESTCA",
            //        notBefore,
            //        notAfter,
            //        caKeyPair,
            //        ref certificateKeyPair,
            //        true);

            //var foo = MyCert.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Pkcs12, "bob");
            //var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(foo, "bob");

            ////var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(@"c:\Temp\Development\ComponentSpace SAML v2.0 for .NET\Examples\Metadata\MetadataExample\idp.pfx", "password");
            ////var cert = new System.Security.Cryptography.X509Certificates.X509Certificate2(@"C:\Users\darren\Documents\Visual Studio 2017\Projects\ISEFront\ISEFront\Certificates\cbd7b1e1-cb4b-451f-ae8f-262fe94ac5ea.pfx", "");
            //System.Security.Cryptography.RSACryptoServiceProvider csp = (System.Security.Cryptography.RSACryptoServiceProvider)cert.PrivateKey;
            //string id = System.Security.Cryptography.CryptoConfig.MapNameToOID("SHA256");
            //csp.SignData(System.IO.File.ReadAllBytes(@"c:\temp\ugh.xml"), id);
//        }
    }
}
