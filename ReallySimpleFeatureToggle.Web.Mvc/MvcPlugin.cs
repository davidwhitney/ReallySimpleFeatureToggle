using System;
using System.Web;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Extensibility;

namespace ReallySimpleFeatureToggle.Web.Mvc
{
    public class MvcPlugin : IPluginBootstrapper
    {

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

        }

        public void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi)
        {
            FeatureAttribute.GetFeatureConfiguration = () => configurationApi.And().GetFeatureConfiguration(_getTenantForRequest());
            WhenEnabled.GetFeatureConfiguration = () => configurationApi.And().GetFeatureConfiguration(_getTenantForRequest());
        }

    }
}