using System.Web;
using ReallySimpleFeatureToggle.Extensibility;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public class MvcPlugin : IPluginBootstrapper
    {
        internal static string _tenant;
        public MvcPlugin(string tenant = null)
        {
            _tenant = tenant;
        }

        private static string FeatureCacheKey
        {
            get { return "ReallySimpleFeatureToggle.Web.Mvc.MvcPlugin.FeatureCache"; }
        }

        public void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi)
        {
            FeatureAttribute.GetFeatureConfiguration = ReturnRequestCachedFeatures;
            WhenEnabled.GetFeatureConfiguration = ReturnRequestCachedFeatures;
        }

        private static IFeatureConfiguration ReturnRequestCachedFeatures()
        {
            if (HttpContext.Current.Items.Contains(FeatureCacheKey))
            {
                return (IFeatureConfiguration)HttpContext.Current.Items[FeatureCacheKey];
            }

            var config = ReallySimpleFeature.Toggles.GetFeatureConfiguration(_tenant);
            HttpContext.Current.Items[FeatureCacheKey] = config;
            
            return config;
        }
    }
}