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
            var m = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };

            Assert.That(m.IsAvailable(TestFeatures.TestFeature1));
        }

        [Test]
        public void NotConfigured_WhenSetToThrow_ThrowsWhenFeatureIsAvailableCalled()
        {
            var m = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };

            Assert.Throws<FeatureNotConfiguredException>(()=>m.IsAvailable("nothere"));
        }

        [Test]
        public void NotConfigured_WhenSetToReturnTrue_ReturnsTrueWhenFeatureIsAvailableCalled()
        {
            var m = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };
            m.FeatureNotConfiguredBehaviour = new FeatureShouldBeEnabled();

            Assert.That(m.IsAvailable("nothere"));
        }

        [Test]
        public void NotConfigured_WhenSetToReturnFalse_ReturnsFalseWhenFeatureIsAvailableCalled()
        {
            var m = new FeatureConfiguration { { TestFeatures.TestFeature1, new ActiveSettings { IsAvailable = true, } } };
            m.FeatureNotConfiguredBehaviour = new FeatureShouldBeDisabled();

            Assert.That(m.IsAvailable("nothere"), Is.False);
        }
    }
}