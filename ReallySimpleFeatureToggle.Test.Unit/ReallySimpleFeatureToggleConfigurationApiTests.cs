using System;
using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;

namespace ReallySimpleFeatureToggle.Test.Unit
{
    [TestFixture]
    public class ReallySimpleFeatureToggleConfigurationApiTests
    {
        private ReallySimpleFeatureToggleConfigurationApi _config;

        [SetUp]
        public void SetUp()
        {
            _config = new ReallySimpleFeatureToggleConfigurationApi(new ReallySimpleFeature());
        }

        [Test]
        public void WithFeatureConfigurationSource_PassedRepository_WiresUp()
        {
            var mockSource = new Mock<IFeatureRepository>();

            _config.WithFeatureConfigurationSource(mockSource.Object);

            Assert.That(_config.FeatureConfigRepository, Is.EqualTo(mockSource.Object));
        }

        [Test]
        public void WithFeatureConfigurationSource_PassedCacheDuration_DecoratesRepoInCachingWrapper()
        {
            var mockSource = new Mock<IFeatureRepository>();

            _config.WithFeatureConfigurationSource(mockSource.Object, new TimeSpan(200));

            Assert.That(_config.FeatureConfigRepository, Is.TypeOf<CachingFeaturesRepository>());
        }
    }
}
