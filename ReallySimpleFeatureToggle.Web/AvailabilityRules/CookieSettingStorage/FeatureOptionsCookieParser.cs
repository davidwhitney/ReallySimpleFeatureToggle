using System.Web;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class FeatureOptionsCookieParser : IFeatureOptionsCookieParser
    {
        private readonly IJsonSerializer _serializer;
        public static string CookieName = "FeatureOptions";

        public FeatureOptionsCookieParser(IJsonSerializer serializer = null)
        {
            _serializer = serializer ?? new DataContractSerializer();
        }

        public FeatureOptionsCookie GetFeatureOptionsCookie()
        {
            return GetFeatureOptionsCookie(new HttpContextWrapper(HttpContext.Current));
        }

        public FeatureOptionsCookie GetFeatureOptionsCookie(HttpContextBase context)
        {
            var cookie = context.Request.Cookies[CookieName];
            var defaultCookie = new FeatureOptionsCookie();

            if (cookie == null)
            {
                return defaultCookie;
            }

            try
            {
                return _serializer.DeserializeObject<FeatureOptionsCookie>(cookie.Value) ?? defaultCookie;
            }
            catch // Just in case our cookie format changes
            {
                ClearSavedOptions();
                return defaultCookie;
            }
        }

        public void AddFeatureSetting(string feature, bool isEnabled)
        {
            var cookie = GetFeatureOptionsCookie();
            cookie.FeatureStates[feature] = isEnabled;
            SetFeatureOptionsCookie(cookie);
        }

        public void ClearFeatureSetting(string feature)
        {
            var cookie = GetFeatureOptionsCookie();
            if (cookie.FeatureStates.ContainsKey(feature))
            {
                cookie.FeatureStates.Remove(feature);
                SetFeatureOptionsCookie(cookie);
            }
        }

        public void SetFeatureOptionsCookie(FeatureOptionsCookie featureOptions)
        {
            SetFeatureOptionsCookie(new HttpContextWrapper(HttpContext.Current), featureOptions);
        }

        public void SetFeatureOptionsCookie(HttpContextBase context, FeatureOptionsCookie featureOptions)
        {
            var cookieContent = _serializer.SerializeObject(featureOptions);
            var cookie = new HttpCookie(CookieName, cookieContent) { Expires = 365.Days().Hence(), Path = "/" };
            context.Response.SetCookie(cookie);
        }

        public void ClearSavedOptions()
        {
            ClearSavedOptions(new HttpContextWrapper(HttpContext.Current));
        }

        public void ClearSavedOptions(HttpContextBase context)
        {
            if (context.Request.Cookies[CookieName] == null)
            {
                return;
            }

            var cookie = new HttpCookie(CookieName) { Expires = 1.Day().Ago() };
            context.Response.Cookies.Add(cookie);
        }
    }
}