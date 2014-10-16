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
        private readonly IEvaluationContextBuilder _evaluationContextBuilder;

        public FeatureEvaluator(IFeatureRepository repository, 
                                IList<IAvailabilityRule> availabilityRules, 
                                IEnumerable<IFeatureOverrideRule> featureOverrides, 
                                IFeatureNotConfiguredBehaviour featureNotConfiguredBehaviour, 
                                IEvaluationContextBuilder evaluationContextBuilder)
        {
            _repository = repository;
            _availabilityRules = availabilityRules;
            _featureOverrides = featureOverrides;
            _featureNotConfiguredBehaviour = featureNotConfiguredBehaviour;
            _evaluationContextBuilder = evaluationContextBuilder ?? new DefaultEvaluationContextBuilder();
        }

        public IFeatureConfiguration LoadConfiguration(string forTenant = Tenant.All)
        {
            forTenant = Tenant.Parse(forTenant);
            var context = _evaluationContextBuilder.Create(forTenant);

            var featureConfiguration = new FeatureConfiguration
            {
                FeatureNotConfiguredBehaviour = _featureNotConfiguredBehaviour,
                Tenant = forTenant
            };

            var featureSettings = _repository.GetFeatureSettings();

            foreach (var feature in featureSettings)
            {
                var availability = CalculateAvailability(feature, context, featureSettings);
                var activeSettings = new ActiveSettings {Dependencies = feature.Dependencies, IsAvailable = availability};

                featureConfiguration.Add(feature.Name, activeSettings);
            }

            foreach (var rule in _featureOverrides)
            {
                rule.Apply(featureConfiguration, context);
            }
            
            return featureConfiguration;
        }

        private bool CalculateAvailability(IFeature featureToCheck, EvaluationContext context, ICollection<IFeature> allFeatureSettings, ICollection<IFeature> relatedFeatures = null)
        {
            relatedFeatures = relatedFeatures ?? new List<IFeature>();
            if (relatedFeatures.Contains(featureToCheck))
            {
                throw new CircularDependencyException();
            }

            relatedFeatures.Add(featureToCheck);

            foreach (var featureName in featureToCheck.Dependencies)
            {
                var dependency = allFeatureSettings.FirstOrDefault(s => s.Name == featureName);
                if (dependency == null)
                {
                    return _featureNotConfiguredBehaviour.GetFeatureAvailabilityWhenFeatureWasNotConfigured(featureName);
                }

                if (!CalculateAvailability(dependency, context, allFeatureSettings, relatedFeatures))
                {
                    return false;
                }
            }

            relatedFeatures.Remove(featureToCheck);

            var globalRulesPassed = _availabilityRules.All(rule => rule.IsAvailable(featureToCheck, context)) || _availabilityRules.Count == 0;
            var featureRulesPassed = featureToCheck.AdditionalRules.All(rule => rule.IsAvailable(featureToCheck, context)) || featureToCheck.AdditionalRules.Count == 0;
            
            return globalRulesPassed && featureRulesPassed;
        }
    }
}