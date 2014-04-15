using System.Linq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Web.AvailabilityRules;
using ReallySimpleFeatureToggle.Web.FeatureOverrides;

namespace ReallySimpleFeatureToggle.Web.Test.Unit
{
    [TestFixture]
    public class WebPluginTests
    {
        private ReallySimpleFeature _featureSet;
        private ReallySimpleFeatureToggleConfigurationApi _configurationApi;

        [SetUp]
        public void SetUp()
        {
            _featureSet = new ReallySimpleFeature();
            _configurationApi = _featureSet.Configure as ReallySimpleFeatureToggleConfigurationApi;
        }

        [Test]
        public void EnableQueryStringOverride_True_PluginAddsFeatureOverride()
        {
            var plugin = new WebPlugin(enabledQueryStringOverride: true);

            plugin.Configure(_configurationApi);

            Assert.That(_configurationApi.OverrideRules[0], Is.TypeOf<QueryStringOverrideRule>());
        }

        [Test]
        public void EnableQueryStringOverride_False_NoOverrideAdded()
        {
            var plugin = new WebPlugin(enabledQueryStringOverride: false);

            plugin.Configure(_configurationApi);

            Assert.That(_configurationApi.OverrideRules, Is.Empty);
        }

        [Test]
        public void PersistRandomPercentageResultToCookie_True_PluginRewiresRandomPercentageRule()
        {
            var plugin = new WebPlugin(persistRandomPercentageResultToCookie: true);

            plugin.Configure(_configurationApi);

            Assert.That(_configurationApi.DefaultAvailabilityRules.Count(x => x.GetType() == typeof(MustBeEnabledOrEnabledForPercentageStickyRule)), Is.EqualTo(1));
            Assert.That(_configurationApi.DefaultAvailabilityRules.Count(x => x.GetType() == typeof(MustBeEnabledOrEnabledForPercentageRule)), Is.EqualTo(0));
        }

        [Test]
        public void PersistRandomPercentageResultToCookie_False_PluginAddsFeatureOverride()
        {
            var plugin = new WebPlugin(persistRandomPercentageResultToCookie: false);

            plugin.Configure(_configurationApi);

            Assert.That(_configurationApi.DefaultAvailabilityRules.Count(x => x.GetType() == typeof(MustBeEnabledOrEnabledForPercentageStickyRule)), Is.EqualTo(0));
            Assert.That(_configurationApi.DefaultAvailabilityRules.Count(x => x.GetType() == typeof(MustBeEnabledOrEnabledForPercentageRule)), Is.EqualTo(1));
        }
    }
}
