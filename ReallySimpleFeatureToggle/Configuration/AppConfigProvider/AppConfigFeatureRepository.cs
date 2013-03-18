using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class AppConfigFeatureRepository : IFeatureRepository
    {
        public ICollection<IFeature> GetFeatureSettings()
        {
            var cfg = (IFeatureConfigurationSection)ConfigurationManager.GetSection("features");
            var featureSettings = cfg.FeatureSettings.Cast<FeatureConfigurationElement>().ToList();
            return featureSettings.Select(fcse => new Feature(fcse.Name)
                {
                    Dependencies = fcse.Dependencies,
                    State = fcse.State,
                    SupportedTenants = fcse.SupportedTenants,
                    ExcludedTenants = fcse.ExcludedTenants,
                    StartDtg = fcse.StartDtg,
                    EndDtg = fcse.EndDtg,
                    RandomPercentageEnabled = fcse.RandomPercentageEnabled,
                }).Cast<IFeature>().ToList();
        }
    }
}