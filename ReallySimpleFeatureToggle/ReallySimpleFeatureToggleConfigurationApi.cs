using System;
using System.Collections.Generic;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.AppConfigProvider;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.Configuration.FluentConfigProvider;
using ReallySimpleFeatureToggle.Extensibility;
using ReallySimpleFeatureToggle.FeatureOverrides;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Infrastructure;

namespace ReallySimpleFeatureToggle
{
    public class ReallySimpleFeatureToggleConfigurationApi : IReallySimpleFeatureToggleConfigurationApi
    {
        public ReallySimpleFeature Parent { get; set; }

        public IFeatureRepository FeatureConfigRepository { get; private set; }
        public IList<IAvailabilityRule> DefaultAvailabilityRules { get; private set; }
        public IList<IFeatureOverrideRule> OverrideRules { get; private set; }
        public IFeatureNotConfiguredBehaviour FeatureNotConfiguredBehaviour { get; private set; }

        public Type EvaluationContextType { get; set; }
        public IEvaluationContextBuilder EvaluationContextBuilder { get; private set; }

        public ReallySimpleFeatureToggleConfigurationApi(ReallySimpleFeature reallySimpleFeature)
        {
            Parent = reallySimpleFeature;

            var compiler = new DynamicAvailabilityRuleCompiler(() => EvaluationContextType);
            var appConfigRepo = new AppConfigFeatureRepository(compiler);

            FeatureConfigRepository = new CachingFeaturesRepository(appConfigRepo, new TimeSpan(0, 0, 30));
            DefaultAvailabilityRules = new List<IAvailabilityRule>
            {
                new MustBeAvailableForCurrentTenantRule(),
                new MustBeEnabledOrEnabledForPercentageRule(new RandomNumberGenerator()),
                new MustBeInDateRangeConfiguredRule()
            };

            OverrideRules = new List<IFeatureOverrideRule>();
            FeatureNotConfiguredBehaviour = new ThrowANotConfiguredException();

            EvaluationContextType = typeof (EvaluationContext);
            EvaluationContextBuilder = new DefaultEvaluationContextBuilder();
        }

        public IReallySimpleFeatureToggleConfigurationApi GlobalAvailabilityRules(Action<IList<IAvailabilityRule>> mutator)
        {
            mutator(DefaultAvailabilityRules);
            return this;
        }

        public IReallySimpleFeatureToggleConfigurationApi GlobalOverrides(Action<IList<IFeatureOverrideRule>> mutator)
        {
            mutator(OverrideRules);
            return this;
        }

        public IReallySimpleFeatureToggleConfigurationApi WithFeatureConfigurationSource(IFeatureRepository repo, TimeSpan? cacheDuration = null)
        {
            FeatureConfigRepository = repo;
            
            if (cacheDuration != null)
            {
                FeatureConfigRepository = new CachingFeaturesRepository(repo, cacheDuration.Value);
            }

            return this;
        }

        public IReallySimpleFeatureToggleConfigurationApi WithFeatures(Action<IList<IFeature>> featureSettingsMutator)
        {
            var newFeatures = new List<IFeature>();
            featureSettingsMutator(newFeatures);
            FeatureConfigRepository = new FluentConfigurationRepository(newFeatures);

            return this;
        }

        public IReallySimpleFeatureToggleConfigurationApi WhenFeatureRequestedIsNotConfigured(IFeatureNotConfiguredBehaviour notConfiguredBehaviour)
        {
            FeatureNotConfiguredBehaviour = notConfiguredBehaviour;
            return this;
        }

        public IReallySimpleFeatureToggleConfigurationApi WithEvaluationContextOf<TEvaluationContextType>(IEvaluationContextBuilder factoryFunc) where TEvaluationContextType : EvaluationContext
        {
            EvaluationContextType = typeof (TEvaluationContextType);
            EvaluationContextBuilder = factoryFunc ?? new DefaultEvaluationContextBuilder();
            return this;
        }

        public IReallySimpleFeatureToggle And()
        {
            return Parent;
        }

        public IReallySimpleFeatureToggleConfigurationApi WithPlugin(IPluginBootstrapper pluginBootstrapper)
        {
            pluginBootstrapper.Configure(this);
            return this;
        }
    }
}