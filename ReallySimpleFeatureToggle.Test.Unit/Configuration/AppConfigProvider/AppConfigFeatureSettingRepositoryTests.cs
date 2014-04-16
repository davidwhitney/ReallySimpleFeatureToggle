using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.AppConfigProvider;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Test.Unit.Configuration.AppConfigProvider
{
    [TestFixture]
    public class AppConfigFeatureSettingRepositoryTests
    {
        private AppConfigFeatureRepository _wcfsr;
        private ICollection<IFeature> _configSettings;

        [SetUp]
        public void SetUp()
        {
            _wcfsr = new AppConfigFeatureRepository(new DynamicAvailabilityRuleCompiler(() => new DefaultEvaluationContextBuilder()));
            _configSettings = _wcfsr.GetFeatureSettings();
        }

        [Test]
        public void FeatureWithNoStateInConfig_IsEnabledByDefault()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "NoStateFeature");

            Assert.That(feature.State, Is.EqualTo(State.Enabled));
        }

        [Test]
        public void FeatureEnabledInConfig_IsEnabledByDefault()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "EnabledFeature");

            Assert.That(feature.State, Is.EqualTo(State.Enabled));
        }

        [Test]
        public void FeatureDisabledInConfig_IsDisabledByDefault()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "DisabledFeature");

            Assert.That(feature.State, Is.EqualTo(State.Disabled));
        }

        [Test]
        public void SupportedAllFeatureInConfig_SupportsAllTenants()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "SupportedAllFeature");

            Assert.That(feature.SupportedTenants.Length, Is.EqualTo(1));
            Assert.That(feature.SupportedTenants[0], Is.EqualTo("All"));
        }

        [Test]
        public void SupportedStringEmptyFeatureInConfig_SupportsAllTenants()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "SupportedStringEmpty");

            Assert.That(feature.SupportedTenants.Length, Is.EqualTo(1));
            Assert.That(feature.SupportedTenants[0], Is.EqualTo("All"));
        }

        [Test]
        public void FeatureWithDependencyInConfig_HasDependencyListed()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "FeatureWithDependency");

            Assert.That(feature.Dependencies[0], Is.EqualTo("FeatureWithDependant"));
        }

        [Test]
        public void FeatureForMultipleTenantsInConfig_HasTenantsListed()
        {
            var feature = _configSettings.SingleOrDefault(x => x.Name == "FeatureForMultipleTenants");

            Assert.That(feature.SupportedTenants[0], Is.EqualTo("Tenant1"));
            Assert.That(feature.SupportedTenants[1], Is.EqualTo("Tenant2"));
            Assert.That(feature.SupportedTenants[2], Is.EqualTo("Tenant3"));
        }
    }
}
