using System.Collections.Generic;

namespace ReallySimpleFeatureToggle.Configuration.FluentConfigProvider
{
    public class FluentConfigurationRepository : IFeatureRepository
    {
        private readonly List<IFeature> _newFeatures;

        public FluentConfigurationRepository(List<IFeature> newFeatures)
        {
            _newFeatures = newFeatures;
        }

        public ICollection<IFeature> GetFeatureSettings()
        {
            return _newFeatures;
        }
    }
}
