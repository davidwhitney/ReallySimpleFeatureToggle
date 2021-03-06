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
            if (FeatureConfigRepository == null
                || !(FeatureConfigRepository is FluentConfigurationRepository))
            {
                FeatureConfigRepository = new FluentConfigurationRepository();
            }

            featureSettingsMutator(((FluentConfigurationRepository)FeatureConfigRepository).FeatureStorage);
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

    public static class ListExtensionsForFeatureBuilding
    {
        public static IList<IFeature> AddFeature(this IList<IFeature> featureCollection, Enum name, Action<IFeature> mutator = null)
        {
            return AddFeature(featureCollection, name.ToString(), mutator);
        }

        public static IList<IFeature> AddFeature(this IList<IFeature> featureCollection, string name, Action<IFeature> mutator = null)
        {
            return AddFeature(featureCollection, name, State.Enabled, mutator);
        }

        public static IList<IFeature> AddDisabledFeature(this IList<IFeature> featureCollection, Enum name, Action<IFeature> mutator = null)
        {
            return AddDisabledFeature(featureCollection, name.ToString(), mutator);
        }

        public static IList<IFeature> AddDisabledFeature(this IList<IFeature> featureCollection, string name, Action<IFeature> mutator = null)
        {
            return AddFeature(featureCollection, name, State.Disabled, mutator);
        }

        public static IList<IFeature> AddFeature(this IList<IFeature> featureCollection, Enum name, State state, Action<IFeature> mutator = null)
        {
            return AddFeature(featureCollection, name.ToString(), state, mutator);
        }

        public static IList<IFeature> AddFeature(this IList<IFeature> featureCollection, string name, State state, Action<IFeature> mutator = null)
        {
            var feature = new Feature(name) {State = state };
            (mutator ?? ((x => { }))).Invoke(feature);
            featureCollection.Add(feature);
            return featureCollection;
        }
    }
}