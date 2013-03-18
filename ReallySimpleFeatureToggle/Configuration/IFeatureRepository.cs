using System.Collections.Generic;

namespace ReallySimpleFeatureToggle.Configuration
{
    public interface IFeatureRepository
    {
        ICollection<IFeature> GetFeatureSettings();
    }
}