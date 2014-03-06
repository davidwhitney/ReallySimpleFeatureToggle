using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;

namespace ReallySimpleFeatureToggle.Test.Unit
{
    [TestFixture]
    public class FeatureConfigurationTests
    {
        [Test]
        public void IsEnabledInFeatureManifest_ReturnsTrue_WhenFeatureIsAvailable()
        {
            var config = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };

            Assert.That(config.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void NotConfigured_WhenSetToThrow_ThrowsWhenFeatureIsAvailableCalled()
        {
            var config = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };

            Assert.Throws<FeatureNotConfiguredException>(() => config.IsAvailable("nothere"));
        }

        [Test]
        public void NotConfigured_WhenSetToReturnTrue_ReturnsTrueWhenFeatureIsAvailableCalled()
        {
            var config = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };
            config.FeatureNotConfiguredBehaviour = new FeatureShouldBeEnabled();

            Assert.That(config.IsAvailable("nothere"));
        }

        [Test]
        public void NotConfigured_WhenSetToReturnFalse_ReturnsFalseWhenFeatureIsAvailableCalled()
        {
            var config = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };
            config.FeatureNotConfiguredBehaviour = new FeatureShouldBeDisabled();

            Assert.That(config.IsAvailable("nothere"), Is.False);
        }

        [Test]
        public void IsAvailable_WhenInvokedWithString_FlagsCorrectAvailability()
        {
            var config = new FeatureConfiguration {{TestFeatures.TestFeature5, new ActiveSettings {IsAvailable = true}}};
            
            var available = config.IsAvailable(TestFeatures.TestFeature5);

            Assert.That(available, Is.True);
        }

        [Test]
        public void IsAvailable_WhenInvokedWithEnum_FlagsCorrectAvailability()
        {
            var config = new FeatureConfiguration {{TestFeatures.TestFeature5, new ActiveSettings {IsAvailable = true}}};
            
            var available = config.IsAvailable(TestFeaturesEnum.TestFeature5);

            Assert.That(available, Is.True);
        }

        private enum TestFeaturesEnum { TestFeature5 }
    }
}