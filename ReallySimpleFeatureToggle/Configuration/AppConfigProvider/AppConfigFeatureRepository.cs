using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ReallySimpleFeatureToggle.AvailabilityRules;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class AppConfigFeatureRepository : IFeatureRepository
    {
        private readonly DynamicAvailabilityRuleCompiler _compiler;

        public AppConfigFeatureRepository()
        {
            _compiler = new DynamicAvailabilityRuleCompiler();
        }

        public ICollection<IFeature> GetFeatureSettings()
        {
            var cfg = (IFeatureConfigurationSection)ConfigurationManager.GetSection("features");
            var featureSettings = cfg.FeatureSettings.Cast<FeatureConfigurationElement>().ToList();
            return featureSettings.Select(fcse =>
            {
                var feature = new Feature(fcse.Name)
                {
                    Dependencies = fcse.Dependencies,
                    State = fcse.State,
                    SupportedTenants = fcse.SupportedTenants,
                    ExcludedTenants = fcse.ExcludedTenants,
                    StartDtg = fcse.StartDtg,
                    EndDtg = fcse.EndDtg,
                    RandomPercentageEnabled = fcse.RandomPercentageEnabled,
                };

                foreach (var dynamicRule in fcse.Rules.Cast<DynamicRuleConfigurationElement>())
                {
                    var rule = _compiler.TryCompile(dynamicRule.Rule);

                    if (rule != null)
                    {
                        feature.AdditionalRules.Add(rule);
                    }
                }

                return feature;

            }).Cast<IFeature>().ToList();
        }
    }
}