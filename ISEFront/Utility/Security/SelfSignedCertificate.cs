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
        ///// <summary>
        ///// Generate a certificate signed by a generated root certificate as a byte array formatted as PKCS12.
        ///// </summary>
        ///// <param name="certificateSubjectName">The subject name of the certificate. Will be prepended with "CN=".</param>
        ///// <param name="rootCertificateSubjectName">The subject name of the root certificate. Will be prepended with "CN=".</param>
        ///// <param name="validFrom">Date from which the certificate is valid.</param>
        ///// <param name="validUntil">Date until which the certificate is valid.</param>
        ///// <param name="password">The password to protect the private key of the certificate with.</param>
        ///// <param name="hashType">The hash type to generate the certificate signatures with.</param>
        ///// <param name="keyStrength">The key length of the certificate. This is applied to both root and the certificate itself.</param>
        ///// <returns>An X.509 v3 certificate as a byte array formatted as PKCS12.</returns>
        //public static byte[] GenerateSelfSigned(
        //    string certificateSubjectName,
        //    string rootCertificateSubjectName,
        //    DateTimeOffset validFrom,
        //    DateTimeOffset validUntil,
        //    string password,
        //    string hashType = "SHA512withRSA",
        //    int keyStrength = 2048
        //    )
        //{
        //    // Generate the public and private keys for the certificate
        //    var certificateKeyPair = GenerateKeyPair(keyStrength);

        //    // Generate the public and private keys for the root certificate
        //    var rootCertificateKeyPair = GenerateKeyPair(keyStrength);

        //    // Prepend "CN=" to the start of the certificate's subject name
        //    var certSubjectDNString = /*"CN=" +*/ certificateSubjectName;

        //    // Prepend "CN=" to the start of the root certificate's subject name
        //    var rootCertSubjectDNString = /*"CN=" +*/ rootCertificateSubjectName;

        //    // Generate the root certificate to sign with
        //    var rootCertificate = GenerateCertificate(
        //            rootCertSubjectDNString,
        //            rootCertSubjectDNString,
        //            keyStrength,
        //            hashType,
        //            validFrom,
        //            validUntil,
        //            rootCertificateKeyPair.Public,
        //            rootCertificateKeyPair.Private
        //        );

        //    // Generate the certificate
        //    var certificate = GenerateCertificate(
        //            certSubjectDNString,
        //            rootCertSubjectDNString,
        //            keyStrength,
        //            hashType,
        //            validFrom,
        //            validUntil,
        //            certificateKeyPair.Public,
        //            rootCertificateKeyPair.Private
        //        );

        //    // Package the certificate as PKCS12
        //    return EncodeSelfSignedCertificatePkcs12(
        //            certificate,
        //            rootCertificate,
        //            certificateKeyPair.Private,
        //            password
        //        );
        //}

        //public static byte[] GenerateSelfSigned2(
        //    string certificateSubjectName,
        //    string rootCertificateSubjectName,
        //    DateTimeOffset validFrom,
        //    DateTimeOffset validUntil,
        //    string password,
        //    string hashType = "SHA512withRSA",
        //    int keyStrength = 2048
        //    )
        //{
        //    // Generate the public and private keys for the certificate
        //    var certificateKeyPair = GenerateKeyPair(keyStrength);

        //    // Prepend "CN=" to the start of the certificate's subject name
        //    var certSubjectDNString = /*"CN=" +*/ certificateSubjectName;

        //    // Generate the certificate
        //    var certificate = GenerateCertificate(
        //            certSubjectDNString,
        //            certSubjectDNString,
        //            keyStrength,
        //            hashType,
        //            validFrom,
        //            validUntil,
        //            certificateKeyPair.Public,
        //            certificateKeyPair.Private
        //        );

        //    // Package the certificate as PKCS12
        //    return EncodeSelfSignedCertificatePkcs12(
        //            certificate,
        //            certificateKeyPair.Private,
        //            password
        //        );
        //}

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

        ///// <summary>
        ///// Returns a random number to use as a serial number within certificates.
        ///// </summary>
        ///// <returns>A random number between 1 and 2^63-1.</returns>
        ///// <remarks>
        /////     http://openssl.6102.n7.nabble.com/Max-length-of-serial-number-td841.html
        /////     A comment suggests that INTEGER as specified in X.509 v3 is an integer of arbitrary size.
        /////     I should purchase a copy of the standard from :
        /////     http://www.itu.int/rec/T-REC-X.509/en
        /////     so that I can see if there's a section on best practices regarding size. Serial numbers
        /////     don't strike me a point of weakness within certificates, but since the serial number
        /////     introduces intentional entropy to the certificate, bigger may be better.
        ///// </remarks>
        //private static BigInteger GenerateSerialNumber()
        //{
        //    return BigIntegers.CreateRandomInRange(
        //        BigInteger.One,
        //        BigInteger.ValueOf(Int64.MaxValue),
        //        RandomNumberGenerator()
        //        );
        //}

        ///// <summary>
        ///// Returns a pseudo-randomly generated key-pair for use with certificates.
        ///// </summary>
        ///// <param name="keyStrength">The desired length of returned keys.</param>
        ///// <returns>A randomly generated key-pair.</returns>
        ///// <remarks>
        /////     See https://en.wikipedia.org/wiki/Key_size#Asymmetric_algorithm_key_lengths for information
        /////     regarding key length.
        ///// </remarks>
        //private static AsymmetricCipherKeyPair GenerateKeyPair(int keyStrength)
        //{
        //    var keyGenerationParameters = new KeyGenerationParameters(RandomNumberGenerator(), keyStrength);
        //    var keyPairGenerator = new RsaKeyPairGenerator();
        //    keyPairGenerator.Init(keyGenerationParameters);

        //    return keyPairGenerator.GenerateKeyPair();
        //}

        ///// <summary>
        ///// Packages a self-signed certificate chain as a PKCS#12 byte array.
        ///// </summary>
        ///// <param name="certificate">The signed certificate.</param>
        ///// <param name="rootCertificate">The certificate referenced by the issuer DN within the certificate.</param>
        ///// <param name="certificatePrivateKey">The private key of the signed certificate.</param>
        ///// <param name="password">The password to protect the private key of the certificate with.</param>
        ///// <returns>A byte array formatted as PKCS#12</returns>
        //private static byte[] EncodeSelfSignedCertificatePkcs12(
        //        Org.BouncyCastle.X509.X509Certificate certificate,
        //        Org.BouncyCastle.X509.X509Certificate rootCertificate,
        //        AsymmetricKeyParameter certificatePrivateKey,
        //        string password
        //    )
        //{
        //    // Create the PKCS12 store
        //    var store = new Pkcs12StoreBuilder().Build();

        //    // Add the root certificate to the store
        //    var rootCertEntry = new X509CertificateEntry(rootCertificate);
        //    store.SetCertificateEntry(rootCertificate.SubjectDN.ToString(), rootCertEntry);

        //    // Add the certificate to the store
        //    var certEntry = new X509CertificateEntry(certificate);
        //    store.SetCertificateEntry(certificate.SubjectDN.ToString(), certEntry);

        //    // Add a key entry
        //    var keyEntry = new AsymmetricKeyEntry(certificatePrivateKey);
        //    store.SetKeyEntry(certificate.SubjectDN.ToString() + "_key", keyEntry, new X509CertificateEntry[] { certEntry });

        //    // Extract the byte equivalent form of the PKCS12 store to the output byte array
        //    using (var stream = new MemoryStream())
        //    {
        //        // Save the pkcs12 to a memory stream
        //        store.Save(stream, password.ToCharArray(), RandomNumberGenerator());

        //        // Return to the start of the stream before reading
        //        stream.Position = 0;

        //        // Pull the bytes out of the stream
        //        var encoded = new byte[stream.Length];
        //        stream.Read(encoded, 0, (int)stream.Length);

        //        return encoded;
        //    }
        //}

        ///// <summary>
        ///// Packages a self-signed certificate chain as a PKCS#12 byte array.
        ///// </summary>
        ///// <param name="certificate">The signed certificate.</param>
        ///// <param name="certificatePrivateKey">The private key of the signed certificate.</param>
        ///// <param name="password">The password to protect the private key of the certificate with.</param>
        ///// <returns>A byte array formatted as PKCS#12</returns>
        //private static byte[] EncodeSelfSignedCertificatePkcs12(
        //        Org.BouncyCastle.X509.X509Certificate certificate,
        //        AsymmetricKeyParameter certificatePrivateKey,
        //        string password
        //    )
        //{
        //    // Create the PKCS12 store
        //    var store = new Pkcs12StoreBuilder().Build();

        //    // Add the certificate to the store
        //    var certEntry = new X509CertificateEntry(certificate);
        //    store.SetCertificateEntry(certificate.SubjectDN.ToString(), certEntry);

        //    // Add a key entry
        //    var keyEntry = new AsymmetricKeyEntry(certificatePrivateKey);
        //    store.SetKeyEntry(certificate.SubjectDN.ToString() /*+ "_key"*/, keyEntry, new X509CertificateEntry[] { certEntry });

        //    // Extract the byte equivalent form of the PKCS12 store to the output byte array
        //    using (var stream = new MemoryStream())
        //    {
        //        // Save the pkcs12 to a memory stream
        //        store.Save(stream, password.ToCharArray(), RandomNumberGenerator());

        //        // Return to the start of the stream before reading
        //        stream.Position = 0;

        //        // Pull the bytes out of the stream
        //        var encoded = new byte[stream.Length];
        //        stream.Read(encoded, 0, (int)stream.Length);

        //        return encoded;
        //    }
        //}

        ///// <summary>
        ///// Generates an X.509 v3 certificate for the given values
        ///// </summary>
        ///// <param name="certificateSubjectName">The certificate subject name.</param>
        ///// <param name="issuersSubjectName">The subject name of the issuer certificate.</param>
        ///// <param name="keyStrength">The key-length in bits to use for generating the certificate.</param>
        ///// <param name="hashType">The hash type for generating signatures.</param>
        ///// <param name="validFrom">The "valid from" date.</param>
        ///// <param name="validUntil">The "valid until" date.</param>
        ///// <param name="certificatePublicKey">The public key of the certificate.</param>
        ///// <param name="issuersPrivateKey">The private key to sign the certificate with.</param>
        ///// <returns>An X.509 v3 certificate</returns>
        //private static Org.BouncyCastle.X509.X509Certificate GenerateCertificate(
        //    string certificateSubjectName,
        //    string issuersSubjectName,
        //    int keyStrength,
        //    string hashType,
        //    DateTimeOffset validFrom,
        //    DateTimeOffset validUntil,
        //    AsymmetricKeyParameter certificatePublicKey,
        //    AsymmetricKeyParameter issuersPrivateKey
        //    )
        //{
        //    // The Certificate Generator
        //    var certificateGenerator = new X509V3CertificateGenerator();

        //    // Serial Number
        //    certificateGenerator.SetSerialNumber(GenerateSerialNumber());

        //    // Subject names
        //    certificateGenerator.SetIssuerDN(new X509Name(issuersSubjectName));
        //    certificateGenerator.SetSubjectDN(new X509Name(certificateSubjectName));

        //    // Valid For
        //    certificateGenerator.SetNotBefore(validFrom.DateTime);
        //    certificateGenerator.SetNotAfter(validUntil.DateTime);

        //    // Certificate's public Key
        //    certificateGenerator.SetPublicKey(certificatePublicKey);

        //    // Generate the Certificate
        //    ISignatureFactory signatureFactory = new Asn1SignatureFactory(
        //        hashType,
        //        issuersPrivateKey,
        //        RandomNumberGenerator()
        //        );

        //    // selfsign certificate
        //    return certificateGenerator.Generate(signatureFactory);
        //}

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

            //addCertToStore(
            //    caRootCertificate,
            //    StoreName.Root,
            //    StoreLocation.LocalMachine);

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

            //addCertToStore(
            //    certificate,
            //    StoreName.My,
            //    StoreLocation.CurrentUser
            //    );

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
            // Generating Random Numbers
            CryptoApiRandomGenerator randomGenerator = new CryptoApiRandomGenerator();
            SecureRandom random = new SecureRandom(randomGenerator);

            // The Certificate Generator
            X509V3CertificateGenerator certificateGenerator = new X509V3CertificateGenerator();

            // Serial Number
            BigInteger serialNumber = BigIntegers.CreateRandomInRange(BigInteger.One, BigInteger.ValueOf(Int64.MaxValue), random);
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
            var keyGenerationParameters = new KeyGenerationParameters(random, keyStrength);
            var keyPairGenerator = new RsaKeyPairGenerator();
            keyPairGenerator.Init(keyGenerationParameters);
            subjectKeyPair = keyPairGenerator.GenerateKeyPair();

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

        private static X509Certificate2 SetPrivateKey(
            X509Certificate2 x509,
            AsymmetricKeyParameter privateKey
            )
        {
            PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);

            Asn1Sequence seq = (Asn1Sequence)Asn1Object.FromByteArray(info.ParsePrivateKey().GetDerEncoded());
            if (seq.Count != 9)
                throw new PemException("malformed sequence in RSA private key");

            var rsa = RsaPrivateKeyStructure.GetInstance(seq);
            RsaPrivateCrtKeyParameters rsaparams =
                new RsaPrivateCrtKeyParameters(
                    rsa.Modulus,
                    rsa.PublicExponent,
                    rsa.PrivateExponent,
                    rsa.Prime1,
                    rsa.Prime2,
                    rsa.Exponent1,
                    rsa.Exponent2,
                    rsa.Coefficient);

            x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);

            return x509;
        }

        //public static bool addCertToStore(X509Certificate2 cert, StoreName st, StoreLocation sl)
        //{
        //    bool bRet = false;

        //    try
        //    {
        //        X509Store store = new X509Store(st, sl);
        //        store.Open(OpenFlags.ReadWrite);
        //        store.Add(cert);

        //        store.Close();
        //    }
        //    catch
        //    {

        //    }

        //    return bRet;
        //}
    }
}