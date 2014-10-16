using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var repo = new CachingFeaturesRepository(_mockInner.Object, new TimeSpan(0, 0, 0, 0, 50));

            repo.GetFeatureSettings();
            repo.GetFeatureSettings();

            _mockInner.Verify(x => x.GetFeatureSettings(), Times.AtMostOnce());
        }

        [Test]
        public void GetFeatureSettings_WhenCalledTwiceAndCacheHasExpired_ReturnsItemsFromSource()
        {
            var repo = new CachingFeaturesRepository(_mockInner.Object, new TimeSpan(0, 0, 0, 0, 50));

            repo.GetFeatureSettings();

            Thread.Sleep(100);

            repo.GetFeatureSettings();

            _mockInner.Verify(x => x.GetFeatureSettings(), Times.Exactly(2));
        }

        [Test]
        public void GetFeatureSettings_WhenCalledThreeTimes_CacheHasAbsoluteExpiry()
        {
            var cachingFeaturesRepository = new CachingFeaturesRepository(_mockInner.Object, new TimeSpan(0, 0, 0, 2));

            Debug.WriteLine("Call 1 - should cache items");
            cachingFeaturesRepository.GetFeatureSettings();
            var stopwatch = Stopwatch.StartNew();

            Thread.Sleep(1000);
            Debug.WriteLine("{0}ms : Call 2 - should return from cache", stopwatch.ElapsedMilliseconds);
            cachingFeaturesRepository.GetFeatureSettings();

            Thread.Sleep(500);
            Debug.WriteLine("{0}ms : Call 3 - should extend expiration and return from cache", stopwatch.ElapsedMilliseconds);
            cachingFeaturesRepository.GetFeatureSettings();

            Thread.Sleep(700);
            Debug.WriteLine("{0}ms : Call 4 - Cache should've expired and need a refresh", stopwatch.ElapsedMilliseconds);
            cachingFeaturesRepository.GetFeatureSettings();
            
            
            stopwatch.Stop();
            _mockInner.Verify(innerRepository => innerRepository.GetFeatureSettings(), Times.Exactly(2));
        }

    }
}
