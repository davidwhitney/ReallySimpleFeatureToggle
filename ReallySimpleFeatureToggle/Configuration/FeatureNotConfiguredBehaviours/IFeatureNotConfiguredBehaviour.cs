using System;

namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public interface IFeatureNotConfiguredBehaviour
    {
        bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(string featureName, Exception anyException);
        bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(IFeature feature, Exception anyException);
    }
}
