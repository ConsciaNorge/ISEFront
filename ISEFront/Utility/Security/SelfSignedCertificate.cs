using System;
using System.IO;

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Crypto.Parameters;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;

namespace ISEFront.Utility.Security
{
    /// <summary>
    /// <para>Certificate generation functions.</para>
    /// </summary>
    /// <references>
    /// <link>http://stackoverflow.com/questions/36712679/bouncy-castles-x509v3certificategenerator-setsignaturealgorithm-marked-obsolete</link>
    /// <link>https://boredwookie.net/blog/m/test-tool-certificate-generator</link>
    /// <link>https://boredwookie.net/blog/m/bouncy-castle-create-a-basic-certificate</link>
    /// </references>
    public class SelfSignedCertificate
    {
        /// <summary>
        /// Returns an instance of a random number generator for use with cryptographic functions.
        /// </summary>
        /// <returns>A new instance of the random number generator,</returns>
        /// <remarks>
        ///     It may be of interest to write this code as a singleton
        /// </remarks>
        private static SecureRandom RandomNumberGenerator()
        {
            // This version of the code is designed for .NET 4.5
            var randomGenerator = new CryptoApiRandomGenerator();
            var randomNumberGenerator = new SecureRandom(randomGenerator);

            // This version of the code is universal but could be less than optimal.
            // TODO: It may be of interest to identify whether there is a good cryptographic
            //       random number generator for use with .NET Core
            //var randomNumberGenerator = new SecureRandom();

            return randomNumberGenerator;
        }

        /// <summary>
        /// Returns a random number to use as a serial number within certificates.
        /// </summary>
        /// <returns>A random number between 1 and 2^63-1.</returns>
        /// <remarks>
        ///     http://openssl.6102.n7.nabble.com/Max-length-of-serial-number-td841.html
        ///     A comment suggests that INTEGER as specified in X.509 v3 is an integer of arbitrary size.
        ///     I should purchase a copy of the standard from :
        ///     http://www.itu.int/rec/T-REC-X.509/en
        ///     so that I can see if there's a section on best practices regarding size. Serial numbers
        ///     don't strike me a point of weakness within certificates, but since the serial number
        ///     introduces intentional entropy to the certificate, bigger may be better.
        /// </remarks>
        private static BigInteger GenerateSerialNumber()
        {
            return BigIntegers.CreateRandomInRange(
                BigInteger.One,
                BigInteger.ValueOf(Int64.MaxValue),
                RandomNumberGenerator()
                );
        }

        /// <summary>
        /// Returns a pseudo-randomly generated key-pair for use with certificates.
        /// </summary>
        /// <param name="keyStrength">The desired length of returned keys.</param>
        /// <returns>A randomly generated key-pair.</returns>
        /// <remarks>
        ///     See https://en.wikipedia.org/wiki/Key_size#Asymmetric_algorithm_key_lengths for information
        ///     regarding key length.
        /// </remarks>
        private static AsymmetricCipherKeyPair GenerateKeyPair(int keyStrength)
        {
            var keyGenerationParameters = new KeyGenerationParameters(RandomNumberGenerator(), keyStrength);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);

            return keyPairGenerator.GenerateKeyPair();
        }

        /// <summary>Write out the byte stream to the file system.</summary>
        /// <param name="outputFilePath">The path to where to store the file.</param>
        /// <param name="rawCertificateData">The binary data of the certificate to write to file.</param>
        /// <remarks>
        ///     This is a highly generic function which should probably just be moved to a utility class.
        /// </remarks>
        public static void Save(
            string outputFilePath,
            byte[] rawCertificateData
            )
        {
            var fi = new FileInfo(outputFilePath);
            if(!fi.Directory.Exists)
            {
                if(!fi.Directory.Parent.Exists)
                {
                    System.Diagnostics.Trace.WriteLine("Certificate storage directory cannot be created since the parent path does not exist");
                    throw new ArgumentException("Certificate storage directory cannot be created since the parent path does not exist", "outputFilePath");
                }
                fi.Directory.Create();
            }

            using (FileStream outStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.ReadWrite))
            {
                outStream.Write(rawCertificateData, 0, rawCertificateData.Length);
            }
        }

        public static byte[] GenerateSelfSigned(
                string certificateSubjectName,
                string rootCertificateSubjectName,
                DateTimeOffset validFrom,
                DateTimeOffset validUntil,
                string password,
                string hashType = "SHA512withRSA",
                int keyStrength = 2048
            )
        {
            //generate a root CA cert and obtain the privateKey
            AsymmetricCipherKeyPair caKeyPair = null;
            var caRootCertificate =
                GenerateCertificate(
                    rootCertificateSubjectName,
                    rootCertificateSubjectName,
                    validFrom,
                    validUntil,
                    null,
                    ref caKeyPair);

            //generate cert based on the CA cert privateKey
            AsymmetricCipherKeyPair certificateKeyPair = null;
            var certificate =
                GenerateCertificate(
                    certificateSubjectName,
                    rootCertificateSubjectName,
                    validFrom,
                    validUntil,
                    caKeyPair,
                    ref certificateKeyPair,
                    true);

            var x509Collection = new X509Certificate2Collection(new X509Certificate2[] { certificate, caRootCertificate });
            return x509Collection.Export(X509ContentType.Pkcs12, password);
        }

        private static X509Certificate2 GenerateCertificate(
            string subjectName, 
            string issuerName,
            DateTimeOffset notBefore,
            DateTimeOffset notAfter,
            AsymmetricCipherKeyPair issuerKeyPair,
            ref AsymmetricCipherKeyPair subjectKeyPair,
            bool includePrivateKey = false,
            string signatureAlgorithm = "SHA256WithRSA",
            int keyStrength = 2048)
        {
            // The Certificate Generator
            X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();

            // Serial Number
            var serialNumber = GenerateSerialNumber();
            certificateGenerator.SetSerialNumber(serialNumber);

            // Issuer and Subject Name
            X509Name subjectDN = new X509Name(subjectName);
            X509Name issuerDN = new X509Name(issuerName);
            certificateGenerator.SetIssuerDN(issuerDN);
            certificateGenerator.SetSubjectDN(subjectDN);

            // Valid For
            certificateGenerator.SetNotBefore(notBefore.Date);
            certificateGenerator.SetNotAfter(notAfter.Date);

            // Subject Public Key
            subjectKeyPair = GenerateKeyPair(keyStrength);

            certificateGenerator.SetPublicKey(subjectKeyPair.Public);

            if (issuerKeyPair == null)
                issuerKeyPair = subjectKeyPair;

            // Generate the Certificate
            ISignatureFactory signatureFactory = new Asn1SignatureFactory(
                signatureAlgorithm,
                issuerKeyPair.Private,
                RandomNumberGenerator()
                );

            // selfsign certificate
            var certificate = certificateGenerator.Generate(signatureFactory);

            // merge into X509Certificate2
            X509Certificate2 x509 = new X509Certificate2(certificate.GetEncoded());

            if (includePrivateKey)
                SetPrivateKey(x509, subjectKeyPair.Private);

            return x509;
        }

        public static RSA ToRSA(RsaKeyParameters rsaKey)
        {
            RSAParameters rp = DotNetUtilities.ToRSAParameters(rsaKey);

            RSACryptoServiceProvider rsaCsp = new RSACryptoServiceProvider();
            rsaCsp.ImportParameters(rp);

            return rsaCsp;
        }

        private static X509Certificate2 SetPrivateKey(
            X509Certificate2 x509,
            AsymmetricKeyParameter privateKey
            )
        {
            var rsaPrivateKey = DotNetUtilities.ToRSA(privateKey as RsaPrivateCrtKeyParameters);

            // Setup RSACryptoServiceProvider with "KeyContainerName" set
            var csp = new CspParameters();
            csp.KeyContainerName = "KeyContainer";

            var rsaPrivate = new RSACryptoServiceProvider(csp);

            // Import private key from BouncyCastle's rsa
            rsaPrivate.ImportParameters(rsaPrivateKey.ExportParameters(true));

            x509.PrivateKey = rsaPrivateKey; 

            //PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            //Asn1Sequence seq = (Asn1Sequence)Asn1Object.FromByteArray(info.ParsePrivateKey().GetDerEncoded());
            //if (seq.Count != 9)
            //    throw new PemException("malformed sequence in RSA private key");

            //var rsa = RsaPrivateKeyStructure.GetInstance(seq);
            //RsaPrivateCrtKeyParameters rsaparams =
            //    new RsaPrivateCrtKeyParameters(
            //        rsa.Modulus,
            //        rsa.PublicExponent,
            //        rsa.PrivateExponent,
            //        rsa.Prime1,
            //        rsa.Prime2,
            //        rsa.Exponent1,
            //        rsa.Exponent2,
            //        rsa.Coefficient);

            //CspParameters cspParams = new CspParameters();
            //cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
            //RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(cspParams);

            //x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);

            return x509;
        }
    }
}