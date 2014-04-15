using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    internal static class WhenEnabled
    {
        public const string FeatureNameKey = "FeatureName";
        internal static Func<IFeatureConfiguration> GetFeatureConfiguration { get; set; }

        internal static void ForFeature(string featureName, object values, Action action)
        {
            ForFeature(featureName, new RouteValueDictionary(values), action);
        }

        internal static void ForFeature(string featureName, IDictionary<string, object> viewData, Action action)
        {
            var config = GetFeatureConfiguration();
            if (!config.IsAvailable(featureName))
            {
                return;
            }

            if (viewData.ContainsKey(FeatureNameKey))
            {
                viewData.Remove(FeatureNameKey);
            }

            viewData[FeatureNameKey] = featureName;

            action();
        }

        internal static MvcHtmlString ForFeature(string featureName, object values, Func<MvcHtmlString> action)
        {
            return ForFeature(featureName, new RouteValueDictionary(values), action);
        }

        internal static MvcHtmlString ForFeature(string featureName, IDictionary<string, object> viewData, Func<MvcHtmlString> action)
        {
            var config = GetFeatureConfiguration();
            if (!config.IsAvailable(featureName))
            {
                return new MvcHtmlString("");
            }

            if (viewData.ContainsKey(FeatureNameKey))
            {
                viewData.Remove(FeatureNameKey);
            }

            viewData[FeatureNameKey] = featureName;

            return action();
        }
    }
}