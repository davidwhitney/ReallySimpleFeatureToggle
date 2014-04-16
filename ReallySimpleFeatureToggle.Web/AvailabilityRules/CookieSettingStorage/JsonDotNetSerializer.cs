using Newtonsoft.Json;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class JsonDotNetSerializer : IJsonSerializer
    {
        public string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public T DeserializeObject<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }
    }
}