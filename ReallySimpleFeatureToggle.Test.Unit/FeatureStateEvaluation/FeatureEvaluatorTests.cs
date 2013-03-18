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
                                                             _featureAvailabilityRules, new List<IFeatureOverrideRule>(), new ThrowANotConfiguredException());
        }
        
        [Test]
        public void IsAvailable_NoCorrespondingFeatureSettingAndCookieAvailableAndDependenciesOk_ThrowsException()
        {
            _featureSettings.Add(new Feature("Blah"));
            
            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.Throws<FeatureNotConfiguredException>(() => manifest.IsAvailable(TestFeatures.TestFeature1));
        }  

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependenciesOk_ReturnsTrue()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1));

            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.That(manifest.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependencySettingsMissing_ThrowsException()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            
            Assert.Throws<FeatureNotConfiguredException>(() => _featureEvaluator.LoadConfiguration());
        }

        [Test]
        public void IsAvailable_PreviewableAndCookieAvailableAndDependencySettingsOK_ReturnsTrue()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5));

            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.That(manifest.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_DisabledAndCookieAvailableAndDependencySettingsOK_ReturnsFalse()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Disabled,
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5));

            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.That(manifest.IsAvailable(TestFeatures.TestFeature1), Is.False);
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOK_ReturnsTrue()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Enabled,
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5));
            
            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.That(manifest.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsNotOK_ReturnsFalse()
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
            
            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.That(!manifest.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsNotOK_ReturnsFalse2()
        {
            _featureSettings.Add(new Feature(TestFeatures.TestFeature1)
                {
                    State = State.Enabled,
                    Dependencies = new[] {TestFeatures.TestFeature5}
                });
            _featureSettings.Add(new Feature(TestFeatures.TestFeature5)
                {
                    State = State.Disabled
                });
            
            var manifest = _featureEvaluator.LoadConfiguration();

            Assert.That(!manifest.IsAvailable(TestFeatures.TestFeature1));
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

        [Test]
        public void IsAvailable_EnabledAndCookieAvailableAndDependencySettingsOKAndStartAfterEndDate_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () =>
                new Feature(TestFeatures.TestFeature1)
                {
                    StartDtg = DateTime.Now.AddDays(1),
                    EndDtg = DateTime.Now.AddDays(2),
                });
        }
    }
}