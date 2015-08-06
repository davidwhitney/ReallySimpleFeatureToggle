using System.Collections.Generic;

namespace ReallySimpleFeatureToggle.Configuration.FluentConfigProvider
{
    public class FluentConfigurationRepository : IFeatureRepository
    {
        public List<IFeature> FeatureStorage { get; private set; }

        public FluentConfigurationRepository()
        {
            FeatureStorage = new List<IFeature>();
        }

        public ICollection<IFeature> GetFeatureSettings()
        {
            return FeatureStorage;
        }
    }
}
