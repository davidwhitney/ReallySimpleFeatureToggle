using System;

namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public class ThrowANotConfiguredException : IFeatureNotConfiguredBehaviour
    {
        public bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(string featureName, Exception anyException)
        {
            throw new FeatureNotConfiguredException(featureName, anyException);
        }
    }
}