using System;
using System.Collections.Generic;
using System.Linq;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.FeatureOverrides;

namespace ReallySimpleFeatureToggle.FeatureStateEvaluation
{
    public class FeatureEvaluator : IFeatureEvaluator
    {
        private readonly IFeatureRepository _repository;
        private readonly IList<IAvailabilityRule> _availabilityRules;
        private readonly IEnumerable<IFeatureOverrideRule> _featureOverrides;
        private readonly IFeatureNotConfiguredBehaviour _featureNotConfiguredBehaviour;

        public FeatureEvaluator(IFeatureRepository repository, IList<IAvailabilityRule> availabilityRules, IEnumerable<IFeatureOverrideRule> featureOverrides, IFeatureNotConfiguredBehaviour featureNotConfiguredBehaviour)
        {
            _repository = repository;
            _availabilityRules = availabilityRules;
            _featureOverrides = featureOverrides;
            _featureNotConfiguredBehaviour = featureNotConfiguredBehaviour;
        }

        public IFeatureConfiguration LoadConfiguration(string forTenant = Tenant.All)
        {
            var context = new EvaluationContext { Tenant = forTenant };
            var featureConfiguration = new FeatureConfiguration();
            featureConfiguration.FeatureNotConfiguredBehaviour = _featureNotConfiguredBehaviour;

            var featureSettings = _repository.GetFeatureSettings();

            foreach (var feature in featureSettings)
            {
                var isAvailable = CalculateAvailability(feature, context, featureSettings, new List<IFeature>());

                featureConfiguration.Add(feature.Name,
                    new ActiveSettings
                    {
                        Dependencies = feature.Dependencies,
                        IsAvailable = isAvailable,
                    });
            }

            foreach (var rule in _featureOverrides)
            {
                rule.Apply(featureConfiguration, context);
            }
            
            return featureConfiguration;
        }

        private bool CalculateAvailability(IFeature featureToCheck, EvaluationContext context, ICollection<IFeature> allFeatureSettings,  ICollection<IFeature> featuresCurrentlyUnderAnalysis)
        {
            if (featuresCurrentlyUnderAnalysis.Contains(featureToCheck))
            {
                throw new CircularDependencyException();
            }

            featuresCurrentlyUnderAnalysis.Add(featureToCheck);

            foreach (var dependency in featureToCheck.Dependencies)
            {
                try
                {
                    var dependencySetting = allFeatureSettings.First(s => s.Name == dependency);

                    if (!CalculateAvailability(dependencySetting, context, allFeatureSettings, featuresCurrentlyUnderAnalysis))
                    {
                        return false;
                    }
                }
                catch (InvalidOperationException e)
                {
                    return _featureNotConfiguredBehaviour.GetFeatureAvailabilityWhenFeatureWasNotConfigured(dependency, e);
                }
            }

            featuresCurrentlyUnderAnalysis.Remove(featureToCheck);

            return _availabilityRules.All(rule => rule.IsAvailable(featureToCheck, context)) || _availabilityRules.Count == 0;
        }
    }
}