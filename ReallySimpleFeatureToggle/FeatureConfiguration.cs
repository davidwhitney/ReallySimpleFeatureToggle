using System;
using System.Collections.Generic;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;

namespace ReallySimpleFeatureToggle
{
    public class FeatureConfiguration : Dictionary<string, ActiveSettings>, IFeatureConfiguration
    {
        public string Tenant { get; set; }
        private IFeatureNotConfiguredBehaviour _featureNotConfiguredBehaviour;

        public bool IsAvailable(Enum feature)
        {
            return IsAvailable(feature.ToString());
        }

        public bool IsAvailable(string feature)
        {
            return ContainsKey(feature)
                ? this[feature].IsAvailable
                : FeatureNotConfiguredBehaviour.GetFeatureAvailabilityWhenFeatureWasNotConfigured(feature);
        }

        public IFeatureNotConfiguredBehaviour FeatureNotConfiguredBehaviour
        {
            get { return _featureNotConfiguredBehaviour ?? new ThrowANotConfiguredException(); }
            set { _featureNotConfiguredBehaviour = value; }
        }
    }
}