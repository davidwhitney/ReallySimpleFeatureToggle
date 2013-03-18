using System;
using System.Collections.Generic;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.FeatureOverrides;

namespace ReallySimpleFeatureToggle
{
    public interface IReallySimpleFeatureToggleConfigurationApi
    {
        IReallySimpleFeatureToggleConfigurationApi GlobalAvailabilityRules(Action<IList<IAvailabilityRule>> mutator);
        IReallySimpleFeatureToggleConfigurationApi GlobalOverrides(Action<IList<IFeatureOverrideRule>> mutator);
        IReallySimpleFeatureToggleConfigurationApi WithFeatureConfigurationSource(IFeatureRepository repo, TimeSpan? cacheDuration = null);
        IReallySimpleFeatureToggleConfigurationApi WithFeatures(Action<IList<IFeature>> featureSettingsMutator);
        
        /// <summary>
        /// Defaults to ThrowANotConfiguredException. Options: "FeatureShouldBeEnabled", "FeatureShouldBeDisabled".
        /// </summary>
        IReallySimpleFeatureToggleConfigurationApi WhenFeatureRequestedIsNotConfigured(IFeatureNotConfiguredBehaviour notConfiguredBehaviour);

        IReallySimpleFeatureToggle And();
    }
}