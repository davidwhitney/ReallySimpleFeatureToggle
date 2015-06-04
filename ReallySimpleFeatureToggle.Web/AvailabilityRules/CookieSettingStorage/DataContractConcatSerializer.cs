using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class DataContractConcatSerializer : ICookieDataSerializer
    {
        public string SerializeObject(object obj)
        {
            var typed = (FeatureOptionsCookie) obj;
            var sb = new StringBuilder();
            foreach (var item in typed.FeatureStates)
            {
                sb.Append(HttpUtility.UrlEncode(item.Key) + "=" + HttpUtility.UrlEncode(item.Value.ToString()) + "&");
            }
            
            return sb.ToString().TrimEnd('&');
        }

        public T DeserializeObject<T>(string value) where T : class
        {
            var pairs = value.Split('&');
            var cookieData = new FeatureOptionsCookie();
            foreach (var thisPair in pairs.Select(pair => pair.Split('=')))
            {
                cookieData.FeatureStates.Add(thisPair[0], bool.Parse(thisPair[1]));
            }

            return cookieData as T;
        }
    }
}