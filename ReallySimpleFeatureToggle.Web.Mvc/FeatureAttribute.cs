using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public class Something
    {
        public void Method()
        {
            if (IsEnabled("new-algo"))
            {
                // New Implementation
            }
            else
            {
                // Old one
            }
        }

        private bool IsEnabled(string newAlgo)
        {
            return true;
        }
    }



    public class FeatureAttribute : ActionFilterAttribute
    {
        public string Feature { get; set; }
        
        public static Func<IFeatureConfiguration> GetFeatureConfiguration { get; set; }

        public FeatureAttribute()
        {
        }

        public FeatureAttribute(string feature)
        {
            Feature = feature;
        }

        public FeatureAttribute(Enum feature) : this(feature.ToString())
        {
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var candidates = new List<string> {Feature};
            var config = GetFeatureConfiguration();

            if (string.IsNullOrWhiteSpace(Feature))
            {
                candidates.Clear();
                candidates.Add(filterContext.ActionDescriptor.ActionName);
                candidates.Add(
                    filterContext.ActionDescriptor.ControllerDescriptor.ControllerName +
                    filterContext.ActionDescriptor.ActionName);
            }

            if (candidates.Count == 1)
            {
                if (config.IsAvailable(Feature))
                {
                    return;
                }

                DisableResult(filterContext);
                return;
            }

            InferFeatureNameAndDisableIfAppropriate(filterContext, config, candidates);
        }

        private static void InferFeatureNameAndDisableIfAppropriate(ActionExecutingContext filterContext,
            IFeatureConfiguration config, List<string> candidates)
        {
            foreach (var key in config.Keys)
            {
                foreach (var candidate in candidates)
                {
                    if (string.Equals(key, candidate, StringComparison.InvariantCultureIgnoreCase)
                        && !config.IsAvailable(key))
                    {
                        DisableResult(filterContext);
                    }
                }
            }
        }

        private static void DisableResult(ActionExecutingContext filterContext)
        {
            filterContext.Result = new EmptyResult();
            filterContext.HttpContext.Response.StatusCode = 404;
        }
    }
}
