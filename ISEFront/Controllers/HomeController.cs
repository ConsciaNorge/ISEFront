using ISEFront.Utility.Configuration;
using System.Web.Mvc;

namespace ISEFront.Controllers
{
    [CheckConfiguration("Dashboard", "FirstRun")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}