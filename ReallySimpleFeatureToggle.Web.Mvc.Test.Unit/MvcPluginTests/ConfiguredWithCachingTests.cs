using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.MvcPluginTests
{
    [TestFixture]
    public class ConfiguredWithCachingTests
    {

        private ReallySimpleFeature _featureSet;
        private ReallySimpleFeatureToggleConfigurationApi _configurationApi;
        private Mock<IFeatureRepository> _mockRepository;
        private List<IFeature> _testFeatures;

        [SetUp]
        public void SetUp()
        {
            _testFeatures = new List<IFeature> { new Feature ("Test Feature")};

            _mockRepository = new Mock<IFeatureRepository>();
            _mockRepository.Setup(x => x.GetFeatureSettings()).Returns(() => _testFeatures);

            _featureSet = new ReallySimpleFeature();
            _configurationApi = _featureSet.Configure.WithFeatureConfigurationSource(_mockRepository.Object, TimeSpan.FromSeconds(10)) as ReallySimpleFeatureToggleConfigurationApi;
        }


        [Test]
        public void WhenPluginInstalled_ConfiguredWithCaching_CachedItemsReturned()
        {
            var plugin = new MvcPlugin(() => "DEV1");
            plugin.Configure(_configurationApi);

            var featureConfig1 = FeatureAttribute.GetFeatureConfiguration();
            var featureConfig2 = FeatureAttribute.GetFeatureConfiguration();

            Assert.That(featureConfig1.First().Key, Is.EqualTo(featureConfig2.First().Key));
        }
        [Test]
        public void WhenPluginInstalled_ConfiguredWithCaching_GetFeatureSettingsIsCalledOnce()
        {
            var plugin = new MvcPlugin(() => "DEV1");
            plugin.Configure(_configurationApi);

            var featureConfig1 = FeatureAttribute.GetFeatureConfiguration();
            var featureConfig2 = FeatureAttribute.GetFeatureConfiguration();

            _mockRepository.Verify(x => x.GetFeatureSettings(), Times.Once());
        }
    }
}
