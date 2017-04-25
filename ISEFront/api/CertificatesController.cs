using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ISEFront.api.ViewModels
{
    public class CertificatesController : ApiController
    {
        public CertificatesController()
        {
            ComponentSpace.SAML2.SAMLController.Initialize();
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
    }
}