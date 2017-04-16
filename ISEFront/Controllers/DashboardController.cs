using ISEFront.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ISEFront.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Configuration
        [Dashboard(Title="Dashboard Home")]
        public ActionResult Index()
        {
            return View();
        }

        [Dashboard(Title = "Certificates")]
        public ActionResult Certificates()
        {
            return View();
        }
    }
}