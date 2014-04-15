using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    internal static class WhenEnabled
    {
        internal static Func<IFeatureConfiguration> GetFeatureConfiguration { get; set; }

        internal static void ForFeature(string featureName, IDictionary<string, object> viewData, Action action)
        {
            var config = GetFeatureConfiguration();
            if (!config.IsAvailable(featureName))
            {
                return;
            }

            if (viewData.ContainsKey("FeatureName"))
            {
                viewData.Remove("FeatureName");
            }

            viewData["FeatureName"] = featureName;

            action();
        }

        internal static MvcHtmlString ForFeature(string featureName, IDictionary<string, object> viewData, Func<MvcHtmlString> action)
        {
            var config = GetFeatureConfiguration();
            if (!config.IsAvailable(featureName))
            {
                return new MvcHtmlString("");
            }

            if (viewData.ContainsKey("FeatureName"))
            {
                viewData.Remove("FeatureName");
            }

            viewData["FeatureName"] = featureName;

            return action();
        }
    }
}