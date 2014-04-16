using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    [DataContract(Name = "FeatureOptionsCookie")]
    public class FeatureOptionsCookie
    {
        [DataMember(Name = "featureStates")]
        public Dictionary<string, bool> FeatureStates { get; set; }

        public FeatureOptionsCookie()
        {
            FeatureStates = new Dictionary<string, bool>();
        }
    }
}