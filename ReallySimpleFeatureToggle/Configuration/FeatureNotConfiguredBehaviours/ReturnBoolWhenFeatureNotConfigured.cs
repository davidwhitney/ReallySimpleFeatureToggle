using System;

namespace ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours
{
    public class ReturnBoolWhenFeatureNotConfigured : IFeatureNotConfiguredBehaviour
    {
        private readonly bool _value;

        public ReturnBoolWhenFeatureNotConfigured(bool value)
        {
            _value = value;
        }

        public bool GetFeatureAvailabilityWhenFeatureWasNotConfigured(string featureName, Exception anyException)
        {
            return _value;
        }
    }
}