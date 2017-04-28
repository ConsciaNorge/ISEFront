using ISEFront.Utility;
using CiscoISE;
using System;
using System.Linq;
using System.Web.Mvc;

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

        [Dashboard(Title = "Certificates")]
        public ActionResult Certificates()
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

        [Dashboard(Title = "First Run")]
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

        [Route("~/dashboard/idppubliccertificate/{serialNumber}")]
        public FileResult IdpPublicCertificate(string serialNumber)
        {
            var x = ComponentSpace.SAML2.SAMLController.Configuration.LocalIdentityProviderConfiguration;

            var certFilename = HttpContext.Server.MapPath("~\\" + x.LocalCertificateFile);
            var certificateCollection = new System.Security.Cryptography.X509Certificates.X509Certificate2Collection();
            certificateCollection.Import(
                certFilename, 
                x.LocalCertificatePassword, 
                System.Security.Cryptography.X509Certificates.X509KeyStorageFlags.DefaultKeySet);

            foreach(var certificate in certificateCollection)
            {
                if(certificate.SerialNumber.ToLower() == serialNumber.ToLower())
                {
                    var bytes = certificate.Export(System.Security.Cryptography.X509Certificates.X509ContentType.Cert);
                    return File(bytes, "application/pkix-cert", serialNumber + ".cer");
                }
            }
            return null;
        }

        [Dashboard(Title = "Cisco ISE")]
        public async System.Threading.Tasks.Task<ActionResult> CiscoISE()
        {
//            var connection = new ISEConnection(new Uri("https://hvciscoise.munchkinlan.com:9060"), "developer", "Minions12345");
            var connection = new ISEConnection(new Uri("https://hvciscoise.munchkinlan.com:9060"), "sponsoruser", "Minions12345");
            var users = await GuestUsers.Get(connection);
            var user = await GuestUsers.Get(connection, users[0]);
            if(user.Status == "SUSPENDED")
                await GuestUsers.Reinstate(connection, user);
            else
                await GuestUsers.Suspend(connection, user, "Dog ate my homework");
//            var user = await GuestUsers.GetByName(connection, "bminion");
            return View();
        }
    }
}