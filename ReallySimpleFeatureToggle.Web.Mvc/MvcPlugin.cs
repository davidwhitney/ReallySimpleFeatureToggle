using System;
using System.Web;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Extensibility;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public class MvcPlugin : IPluginBootstrapper
    {
        public static HttpContextWrapper HttpContext { get; set; }

        private readonly Func<string> _getTenantForRequest;
        private const string FeatureCacheKey = "ReallySimpleFeatureToggle.Web.Mvc.MvcPlugin.FeatureCache";

        public MvcPlugin():this(() => Tenant.All)
        {
        }

        public MvcPlugin(IRetrieveTheTenantForThisRequest tenantProvider) : this(tenantProvider.GetCurrentTenant)
        {
        }

        public MvcPlugin(Func<string> getTenantForRequest)
        {
            _getTenantForRequest = getTenantForRequest;

            if (System.Web.HttpContext.Current != null)
            {
                HttpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }

        public void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi)
        {
            FeatureAttribute.GetFeatureConfiguration = () => ReturnRequestCachedFeatures(_getTenantForRequest);
            WhenEnabled.GetFeatureConfiguration = () => ReturnRequestCachedFeatures(_getTenantForRequest);
        }

        private static IFeatureConfiguration ReturnRequestCachedFeatures(Func<string> getTenant)
        {
            var tenant = getTenant();
            var tenantedCacheKey = FeatureCacheKey + "_" + tenant;

            if (HttpContext.Items.Contains(tenantedCacheKey))
            {
                return (IFeatureConfiguration)HttpContext.Items[tenantedCacheKey];
            }

            var config = ReallySimpleFeature.Toggles.GetFeatureConfiguration(tenant);
            HttpContext.Items[tenantedCacheKey] = config;
            
            return config;
        }
    }
}