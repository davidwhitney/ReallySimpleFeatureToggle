using System;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.FeatureOverrides;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Infrastructure;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;

namespace ReallySimpleFeatureToggle.Test.Unit.FeatureStateEvaluation
{
    [TestFixture]
    public class FeatureEvaluatorTests
    {
        private IFeatureEvaluator _featureEvaluator;
        private Mock<IFeatureRepository> _featureSettingRepo;
        private IList<IFeature> _featureSettings;
        private List<IAvailabilityRule> _featureAvailabilityRules;

        [SetUp]
        public void SetUp()
        {
            _featureSettings = new List<IFeature>();
            _featureSettingRepo = new Mock<IFeatureRepository>();
            _featureSettingRepo.Setup(x => x.GetFeatureSettings()).Returns(_featureSettings);

            _featureAvailabilityRules = new List<IAvailabilityRule>
                {
                    new MustBeEnabledOrEnabledForPercentageRule(new RandomNumberGenerator()),
                    new MustBeInDateRangeConfiguredRule(),
                    new MustBeAvailableForCurrentTenantRule(),
                };

            _featureEvaluator = new FeatureEvaluator(_featureSettingRepo.Object,
                _featureAvailabilityRules, new List<IFeatureOverrideRule>(), new ThrowANotConfiguredException(), null);
        }

        [TestCase("tenant1", "tenant1")]
        [TestCase("", Tenant.All)]
        [TestCase(null, Tenant.All)]
        public void LoadConfiguration_ForTenant_ReturnsCorrectTenantInConfig(string tenantRequested, string tenantExpected)
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1));

            var config = _featureEvaluator.LoadConfiguration(tenantRequested);

            Assert.That(config.Tenant, Is.EqualTo(tenantExpected));
        }
        
        [Test]
        public void IsAvailable_NoCorrespondingFeatureSetting_ThrowsExceptionBecauseThatWasTheHandlerProvided()
        {
            _featureSettings.Add(new Feature("Blah"));
            
            var config = _featureEvaluator.LoadConfiguration();

            Assert.Throws<FeatureNotConfiguredException>(() => config.IsAvailable(TestFeatures.TestFeature1));
        }  


        [Test]
        public void IsAvailable_FeatureEnabled_ReturnsTrue()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1));

            var config = _featureEvaluator.LoadConfiguration();

            Assert.That(config.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_EnabledAndDependencySettingsMissing_ThrowsException()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            
            Assert.Throws<FeatureNotConfiguredException>(() => _featureEvaluator.LoadConfiguration());
        }

        [Test]
        public void IsAvailable_EnabledAndDependencySettingsOK_ReturnsTrue()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5));

            var config = _featureEvaluator.LoadConfiguration();

            Assert.That(config.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_DisabledAndDependencySettingsOK_ReturnsFalse()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Disabled,
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5));

            var config = _featureEvaluator.LoadConfiguration();

            Assert.That(config.IsAvailable(TestFeatures.TestFeature1), Is.False);
        }

        [Test]
        public void IsAvailable_EnabledAndDependencySettingsNotOK_ReturnsFalse()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Enabled,
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5)
                {
                    State = State.Disabled,
                });
            
            var config = _featureEvaluator.LoadConfiguration();

            Assert.That(!config.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_FeatureDependsOnItself_Throws()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Enabled,
                    Dependencies = new[] {TestFeatures.TestFeature1}
                });
            
            Assert.Throws<CircularDependencyException>(()=> _featureEvaluator.LoadConfiguration());
        } 

        [Test]
        public void IsAvailable_FeaturesDependOnEachOther_Throws()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Enabled,
                    Dependencies = new[] {TestFeatures.TestFeature2}
                });

            _featureSettings.Add(new Feature(TestFeatures.TestFeature2)
                {
                    State = State.Enabled,
                    Dependencies = new[] {TestFeatures.TestFeature1}
                });
            
            Assert.Throws<CircularDependencyException>(()=> _featureEvaluator.LoadConfiguration());
        }    
    }
}