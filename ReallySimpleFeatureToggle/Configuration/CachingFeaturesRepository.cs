using System;
using System.Collections.Generic;
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

            var settings = _inner.GetFeatureSettings();
            _cache.AddOrGetExisting("settings", settings,
                new CacheItemPolicy {SlidingExpiration = _expiry});
            
            return settings;
        }
    }
}
 