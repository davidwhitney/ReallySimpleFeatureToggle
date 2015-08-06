using System;
using System.Collections;
using System.Collections.Generic;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle
{
    public class ReallySimpleFeature : IReallySimpleFeatureToggle
    {
        private static readonly Lazy<ReallySimpleFeature> Lazy = new Lazy<ReallySimpleFeature>(() => new ReallySimpleFeature());
        public static ReallySimpleFeature Toggles { get { return Lazy.Value; } }

        private readonly ReallySimpleFeatureToggleConfigurationApi _configurationApi;

        public ReallySimpleFeature()
        {
            _configurationApi = new ReallySimpleFeatureToggleConfigurationApi(this);
        }

        public IFeatureConfiguration GetFeatureConfiguration(string tenant = Tenant.All)
        {
            var evaluator = new FeatureEvaluator(_configurationApi.FeatureConfigRepository,
                _configurationApi.DefaultAvailabilityRules, _configurationApi.OverrideRules,
                _configurationApi.FeatureNotConfiguredBehaviour, _configurationApi.EvaluationContextBuilder);

            return evaluator.LoadConfiguration(tenant);
        }

        public ICollection<IFeature> FeatureSettings
        {
            get { return _configurationApi.FeatureConfigRepository.GetFeatureSettings(); }
        }

        public IReallySimpleFeatureToggleConfigurationApi Configure
        {
            get { return _configurationApi; }
        }
    }
}