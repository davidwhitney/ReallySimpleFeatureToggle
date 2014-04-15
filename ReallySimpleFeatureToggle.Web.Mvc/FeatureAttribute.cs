using System;
using System.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public class FeatureAttribute : ActionFilterAttribute
    {
        public string Feature { get; set; }
        
        public static Func<IFeatureConfiguration> GetFeatureConfiguration { get; set; }

        public FeatureAttribute(string feature)
        {
            Feature = feature;
        }

        public FeatureAttribute(Enum feature) : this(feature.ToString())
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var config = GetFeatureConfiguration();
            if (config.IsAvailable(Feature))
            {
                return;
            }

            filterContext.Result = new EmptyResult();
            filterContext.HttpContext.Response.StatusCode = 404;
        }
    }
}
