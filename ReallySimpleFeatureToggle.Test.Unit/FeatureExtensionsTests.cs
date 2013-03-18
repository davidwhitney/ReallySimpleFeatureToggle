using NUnit.Framework;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;

namespace ReallySimpleFeatureToggle.Test.Unit
{
    [TestFixture]
    public class FeatureExtensionsTests
    {
        private FeatureConfiguration _featureConfiguration;

        [SetUp]
        public void SetUp()
        {
            _featureConfiguration = new FeatureConfiguration
                {
                    {
                        TestFeatures.TestFeature5,
                        new ActiveSettings
                            {
                                IsAvailable = true
                            }
                    }
                };
        }

        [Test]
        public void IsAvailable_WhenInvokedWithString_FlagsCorrectAvailability()
        {
            var available = _featureConfiguration.IsAvailable(TestFeatures.TestFeature5);

            Assert.That(available, Is.True);
        }

        private enum TestFeaturesEnum { TestFeature5 }

        [Test]
        public void IsAvailable_WhenInvokedWithEnum_FlagsCorrectAvailability()
        {
            var available = _featureConfiguration.IsAvailable(TestFeaturesEnum.TestFeature5);

            Assert.That(available, Is.True);
        }
    }
}