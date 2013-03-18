using System;
using System.Collections.Generic;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;

namespace ReallySimpleFeatureToggle
{
    public class FeatureConfiguration : Dictionary<string, ActiveSettings>, IFeatureConfiguration
    {
        private IFeatureNotConfiguredBehaviour _featureNotConfiguredBehaviour;

        public bool IsAvailable(Enum feature)
        {
            return IsAvailable(feature.ToString());
        }

        public bool IsAvailable(string feature)
        {
            try
            {
                return this[feature].IsAvailable;
            }
            catch (KeyNotFoundException e)
            {
                return FeatureNotConfiguredBehaviour.GetFeatureAvailabilityWhenFeatureWasNotConfigured(feature, e);
            }
        }

        public IFeatureNotConfiguredBehaviour FeatureNotConfiguredBehaviour
        {
            get { return _featureNotConfiguredBehaviour ?? new ThrowANotConfiguredException(); }
            set { _featureNotConfiguredBehaviour = value; }
        }
    }
}