using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ReallySimpleFeatureToggle.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.ExampleMvcSite
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ReallySimpleFeature.Toggles.Configure
                .WithPlugin(new WebPlugin())
                .WithPlugin(new MvcPlugin());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
