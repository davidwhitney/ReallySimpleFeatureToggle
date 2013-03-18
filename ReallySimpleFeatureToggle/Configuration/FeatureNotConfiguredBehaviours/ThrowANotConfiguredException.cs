using System;

namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public class ThrowANotConfiguredException : IFeatureNotConfiguredBehaviour
    {
        public bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(string featureName, Exception anyException)
        {
            throw new FeatureNotConfiguredException(featureName, anyException);
        }

        public bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(IFeature feature, Exception anyException)
        {
            throw new FeatureNotConfiguredException(feature.Name, anyException);

        }
    }
}