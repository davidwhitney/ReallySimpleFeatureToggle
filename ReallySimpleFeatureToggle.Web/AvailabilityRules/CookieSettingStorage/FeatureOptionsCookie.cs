using System.Collections.Generic;
using Newtonsoft.Json;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class FeatureOptionsCookie
    {
        public Dictionary<string, bool> FeatureStates { get; set; }
        public Dictionary<string, object> FeaturePreferences { get; set; }

        public FeatureOptionsCookie()
        {
            FeatureStates = new Dictionary<string, bool>();
            FeaturePreferences = new Dictionary<string, object>();
        }

        public TOutputType PreferencesFor<TOutputType>(string feature) where TOutputType : new()
        {
            var prefsString = FeaturePreferences[feature].ToString();
            try
            {
                return JsonConvert.DeserializeObject<TOutputType>(prefsString);
            }
            catch
            {
                return default(TOutputType);
            }
        }
    }
}