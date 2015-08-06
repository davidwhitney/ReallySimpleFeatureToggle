using System.Collections.Generic;
using ReallySimpleFeatureToggle.Configuration;

namespace ReallySimpleFeatureToggle
{
    public interface IReallySimpleFeatureToggle
    {
        IReallySimpleFeatureToggleConfigurationApi Configure { get; }
        ICollection<IFeature> FeatureSettings { get; }
        IFeatureConfiguration GetFeatureConfiguration(string tenant = Tenant.All);
    }
}