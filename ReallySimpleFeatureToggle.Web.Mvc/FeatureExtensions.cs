using System;
using System.Web.Mvc;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public static class FeatureExtensions
    {
        public static Func<IFeatureConfiguration> GetFeatureConfiguration { get; set; }

        public static bool FeatureAvailable(this HtmlHelper helper, string featureName)
        {
            return GetFeatureConfiguration().IsAvailable(featureName);
        }

        public static bool FeatureAvailable(this HtmlHelper helper, Enum featureName)
        {
            return GetFeatureConfiguration().IsAvailable(featureName);
        }
    }
}