using System.Collections.Generic;
using System.Web;
using NUnit.Framework;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.MvcPluginTests
{
    [TestFixture]
    public class MultiTenantSupportTests
    {
        private ReallySimpleFeature _featureSet;
        private ReallySimpleFeatureToggleConfigurationApi _configurationApi;

        [SetUp]
        public void SetUp()
        {
            _featureSet = new ReallySimpleFeature();
            _configurationApi = _featureSet.Configure as ReallySimpleFeatureToggleConfigurationApi;
            //MvcPlugin.HttpContext = new HttpContextWrapper(new HttpContext(new HttpRequest("", "http://www.bing.com", ""), new HttpResponse(null)));
        }

        [Test]
        public void InstallingPlugin_ConfiguresStaticFeatureFactoryPropertiesOnAttributesAndHelpers()
        {
            var plugin = new MvcPlugin();
            plugin.Configure(_configurationApi);

            Assert.That(FeatureAttribute.GetFeatureConfiguration, Is.Not.Null);
            Assert.That(WhenEnabled.GetFeatureConfiguration, Is.Not.Null);
        }

        [Test]
        public void WhenPluginInstalledWithTenantFunc_SubsequentlyReturnedFeatureConfigurationsAreForThatTenant()
        {
            var plugin = new MvcPlugin(()=>"DEV1");
            plugin.Configure(_configurationApi);

            var featureConfig = FeatureAttribute.GetFeatureConfiguration();

            Assert.That(featureConfig.Tenant, Is.EqualTo("DEV1"));
        }


        [Test]
        public void WhenPluginInstalledWithTenantFunc_SubsequentlyReturnedFeatureConfigurationsFromWhenHelperAreForThatTenant()
        {
            var plugin = new MvcPlugin(()=>"DEV1");
            plugin.Configure(_configurationApi);

            var featureConfig = WhenEnabled.GetFeatureConfiguration();

            Assert.That(featureConfig.Tenant, Is.EqualTo("DEV1"));
        }

        [Test]
        public void WhenPluginInstalledWithTenantPerRequestRetriever_SubsequentlyReturnedFeatureConfigurationsAreForThatTenant()
        {
            var fakeTenantRetriever = new FakeTenantRetreiver("DEV1");
            new MvcPlugin(fakeTenantRetriever).Configure(_configurationApi);

            var featureConfig = WhenEnabled.GetFeatureConfiguration();

            Assert.That(featureConfig.Tenant, Is.EqualTo("DEV1"));
        }

        [Test]
        public void WhenPluginInstalled_MultipleRequestsForDifferentTenants_RetrievesCorrectConfiguration()
        {
            var fakeTenantRetriever = new RetrieverThatReturnsDifferentTenants("DEV1", "DEV2", "DEV3");
            new MvcPlugin(fakeTenantRetriever).Configure(_configurationApi);

            var featureConfig1 = WhenEnabled.GetFeatureConfiguration();
            Assert.That(featureConfig1.Tenant, Is.EqualTo("DEV1"));

            var featureConfig2 = WhenEnabled.GetFeatureConfiguration();
            Assert.That(featureConfig2.Tenant, Is.EqualTo("DEV2"));

            var featureConfig3 = WhenEnabled.GetFeatureConfiguration();
            Assert.That(featureConfig3.Tenant, Is.EqualTo("DEV3"));
        }
    }

    public class FakeTenantRetreiver : IRetrieveTheTenantForThisRequest
    {
        private readonly string _returnThis;
        public Dictionary<int, string> RequestNumberToTenant;

        public FakeTenantRetreiver(string returnThis)
        {
            _returnThis = returnThis;
        }

        public string GetCurrentTenant()
        {
            return _returnThis;
        }
    }

    public class RetrieverThatReturnsDifferentTenants : IRetrieveTheTenantForThisRequest
    {
        private readonly string[] _returnThis;
        private int _called;

        public RetrieverThatReturnsDifferentTenants(params string[] returnThis)
        {
            _returnThis = returnThis;
            _called = 0;
        }

        public string GetCurrentTenant()
        {
            var index = _called;
            _called++;
            return _returnThis[index];
        }
    }
}
