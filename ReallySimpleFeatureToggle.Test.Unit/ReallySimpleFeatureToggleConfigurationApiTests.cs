using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;

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

        [Test]
        public void WhenFeatureRequestedIsNotConfigured_PassedStrategy_WiresUp()
        {
            var mock = new Mock<IFeatureNotConfiguredBehaviour>();

            _config.WhenFeatureRequestedIsNotConfigured(mock.Object);

            Assert.That(_config.FeatureNotConfiguredBehaviour, Is.EqualTo(mock.Object));
        }

        [Test]
        public void GlobalAvailabilityRules_Modified_MutatesGlobalRules()
        {
            _config.GlobalAvailabilityRules(x => x.Clear());

            Assert.That(_config.DefaultAvailabilityRules, Is.Empty);
        }

        [Test]
        public void GlobalOverrides_Modified_MutatesGlobalRules()
        {
            _config.GlobalOverrides(x => x.Clear());

            Assert.That(_config.OverrideRules, Is.Empty);
        }

        [Test]
        public void WithFeatures_StoresFluentlyCreatedFeaturesInInMemoryRepository()
        {
            _config.WithFeatures(x => x.Add(Feature.Called("MyFeature").ThatIsEnabled()));

            var features = _config.FeatureConfigRepository.GetFeatureSettings();

            Assert.That(features.Count, Is.EqualTo(1));
            Assert.That(features.First().Name, Is.EqualTo("MyFeature"));
            Assert.That(features.First().State, Is.EqualTo(State.Enabled));
        }
    }
}
