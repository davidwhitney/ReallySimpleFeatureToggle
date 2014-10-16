using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Caching;

namespace ReallySimpleFeatureToggle.Configuration
{
    public class CachingFeaturesRepository : IFeatureRepository
    {
        private readonly IFeatureRepository _inner;
        private readonly TimeSpan _expiry;
        private readonly MemoryCache _cache;

        public CachingFeaturesRepository(IFeatureRepository inner, TimeSpan expiry)
        {
            _inner = inner;
            _expiry = expiry;
            _cache = new MemoryCache("featuresettings");
        }

        public ICollection<IFeature> GetFeatureSettings()
        {
            var cacheItem = _cache.GetCacheItem("settings");

            if (cacheItem != null && cacheItem.Value != null)
            {
                return cacheItem.Value as ICollection<IFeature>;
            }

            var cachingPolicy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTime.UtcNow.Add(_expiry),
                RemovedCallback = arguments => Debug.WriteLine("CachingFeaturesRepository cache expiry")
            };

            var settings = _inner.GetFeatureSettings();
            _cache.AddOrGetExisting("settings", settings, cachingPolicy);
            return settings;
        }
    }
}
 