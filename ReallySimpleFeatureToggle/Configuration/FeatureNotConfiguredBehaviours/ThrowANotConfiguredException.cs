using System;

namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public class ThrowANotConfiguredException : IFeatureNotConfiguredBehaviour
    {
        public bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(string featureName)
        {
            throw new FeatureNotConfiguredException(featureName);
        }
    }
}