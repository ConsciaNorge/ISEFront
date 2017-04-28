using ISEFront.Utility;
using CiscoISE;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace ISEFront.Controllers
{
    [CheckConfiguration("Dashboard", "FirstRun")]
    public class DashboardController : Controller
    {
        public DashboardController()
        {
            var clientHandler = new System.Net.Http.HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

        }

        // GET: Configuration
        [Dashboard(Title="Dashboard")]
        public ActionResult Index()
        {
            return View();
        }

        [Dashboard(Title = "Service Providers")]
        public ActionResult ServiceProviders()
        {
            return View();
        }

        [Dashboard(Title = "Identity Provider")]
        public ActionResult IdentityProvider()
        {
            return View();
        }

        //[Dashboard(Title = "First Run")]
        public ActionResult FirstRun()
        {
            return View();
        }

        [HttpGet]
        public FileResult idpmetadata()
        {
            var idpCertificate = ComponentSpace.SAML2.SAMLController.CertificateManager.GetLocalIdentityProviderSignatureCertificates("default", null).FirstOrDefault();
            if (idpCertificate == null)
                return null;

            var baseUrl = Request.Url.GetLeftPart(UriPartial.Authority);
            var metadata = IdpMetadata.GenerateIDPMetadata(baseUrl);

            return File(metadata, "application/xml", "idpmetadata.xml");
        }

        public FileResult IdpRootCertificate()
        {
            var x = ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration;

            var certFilename = HttpContext.Server.MapPath("~\\" + x.LocalCertificateFile);
            var certificateCollection = new X509Certificate2Collection();
            certificateCollection.Import(
                certFilename, 
                x.LocalCertificatePassword, 
                X509KeyStorageFlags.DefaultKeySet);

            var sortedCertificates = new List<X509Certificate2>();
            foreach(var current in certificateCollection)
            {
                var before = sortedCertificates.Find(n => n.IssuerName.Name == current.SubjectName.Name);
                if (before == null)
                    sortedCertificates.Add(current);
                else
                {
                    int insertIndex = sortedCertificates.IndexOf(before);
                    sortedCertificates.Insert(insertIndex, current);
                }
            }

            var rootCertificate = sortedCertificates.First();
            var bytes = rootCertificate.Export(X509ContentType.Cert);
            return File(bytes, "application/pkix-cert", rootCertificate.SerialNumber + ".cer");
        }

        [Dashboard(Title = "Cisco ISE")]
        public ActionResult CiscoISE()
        {
            return View();
        }
    }
}