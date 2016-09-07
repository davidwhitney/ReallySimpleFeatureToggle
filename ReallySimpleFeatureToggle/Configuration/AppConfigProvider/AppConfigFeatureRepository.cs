using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public class AppConfigFeatureRepository : IFeatureRepository
    {
        private readonly IFeatureConfigurationSection _cfg;
        private readonly DynamicAvailabilityRuleCompiler _compiler;

        public AppConfigFeatureRepository(DynamicAvailabilityRuleCompiler compiler = null)
            : this((IFeatureConfigurationSection)ConfigurationManager.GetSection("features"), compiler)
        {
        }

        public AppConfigFeatureRepository(IFeatureConfigurationSection cfg, DynamicAvailabilityRuleCompiler compiler = null)
        {
            _cfg = cfg;
            _compiler = compiler ?? new DynamicAvailabilityRuleCompiler(() => typeof(EvaluationContext));
        }

        public ICollection<IFeature> GetFeatureSettings()
        {
            if (_cfg == null)
            {
                throw ConfigMissingException();
            }

            var featureSettings = _cfg.FeatureSettings.Cast<FeatureConfigurationElement>().ToList();
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

        private static ConfigurationErrorsException ConfigMissingException()
        {
            return new ConfigurationErrorsException(
                "The AppConfig Feature Repository requires a valid Configuration section." +
                @"
<configSections>
    <section name=""features"" type=""ReallySimpleFeatureToggle.Configuration.AppConfigProvider.FeatureConfigurationSection, ReallySimpleFeatureToggle"" />
</configSections>
<features>
    <add name=""NoStateFeature"" />
</features>");
        }
    }
}