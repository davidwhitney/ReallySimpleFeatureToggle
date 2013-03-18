using ReallySimpleFeatureToggle.Configuration;

namespace ReallySimpleFeatureToggle
{
    public interface IReallySimpleFeatureToggle
    {
        IReallySimpleFeatureToggleConfigurationApi Configure { get; }
        IFeatureConfiguration GetFeatureConfiguration(string tenant = Tenant.All);
    }
}