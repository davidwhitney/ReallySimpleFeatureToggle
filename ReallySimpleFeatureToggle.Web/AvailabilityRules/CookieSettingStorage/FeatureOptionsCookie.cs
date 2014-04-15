using System.Collections.Generic;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class FeatureOptionsCookie
    {
        public Dictionary<string, bool> FeatureStates { get; set; }

        public FeatureOptionsCookie()
        {
            FeatureStates = new Dictionary<string, bool>();
        }
    }
}