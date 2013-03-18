using System;
using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;

namespace ReallySimpleFeatureToggle.Test.Unit.Configuration
{
    [TestFixture]
    public class CachingFeaturesRepositoryTests
    {
        private Mock<IFeatureRepository> _mockInner;

        [SetUp]
        public void Setup()
        {
            _mockInner = new Mock<IFeatureRepository>();
            _mockInner.Setup(x => x.GetFeatureSettings()).Returns(new List<IFeature>());

        }

        [Test]
        public void GetFeatureSettings_WhenCalledTwice_ReturnsItemsFromInternalCache()
        {
            var repo = new CachingFeaturesRepository(_mockInner.Object, new TimeSpan(0));

            repo.GetFeatureSettings();
            repo.GetFeatureSettings();

            _mockInner.Verify(x => x.GetFeatureSettings(), Times.AtMostOnce());
        }

        [Test]
        public void GetFeatureSettings_WhenCalledTwiceAndCacheHasExpired_ReturnsItemsFromSource()
        {
            var repo = new CachingFeaturesRepository(_mockInner.Object, new TimeSpan(50));

            repo.GetFeatureSettings();

            Thread.Sleep(100);

            repo.GetFeatureSettings();

            _mockInner.Verify(x => x.GetFeatureSettings(), Times.Exactly(2));
        }
    }
}
