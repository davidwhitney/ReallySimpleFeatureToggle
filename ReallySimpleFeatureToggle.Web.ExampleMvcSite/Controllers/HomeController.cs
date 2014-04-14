using System.Web.Mvc;
using ReallySimpleFeatureToggle.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.ExampleMvcSite.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Feature("DisabledFeature")]
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