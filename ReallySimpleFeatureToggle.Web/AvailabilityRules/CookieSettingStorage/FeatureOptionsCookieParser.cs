using System;
using System.Web;
using Newtonsoft.Json;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class FeatureOptionsCookieParser : IFeatureOptionsCookieParser
    {
        private readonly HttpContextBase _context;
        public static string CookieName = "FeatureOptions";

        public FeatureOptionsCookieParser(HttpContextBase context)
        {
            _context = context;
        }

        public FeatureOptionsCookie GetFeatureOptionsCookie()
        {
            var cookie = _context.Request.Cookies[CookieName];
            var defaultCookie = new FeatureOptionsCookie();

            if (cookie == null)
            {
                return defaultCookie;
            }

            try
            {
                return JsonConvert.DeserializeObject<FeatureOptionsCookie>(cookie.Value) ?? defaultCookie;
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
            var cookieContent = JsonConvert.SerializeObject(featureOptions);
            var cookie = new HttpCookie(CookieName, cookieContent) { Expires = 365.Days().Hence(), Path = "/" };
            _context.Response.SetCookie(cookie);
        }

        public void ClearSavedOptions()
        {
            if (_context.Request.Cookies[CookieName] == null)
            {
                return;
            }

            var cookie = new HttpCookie(CookieName) { Expires = 1.Day().Ago() };
            _context.Response.Cookies.Add(cookie);
        }
    }

    internal static class SyntaticSugar
    {
        public static TimeSpan Day(this int i)
        {
            if (i != 1)
            {
                throw new ArgumentOutOfRangeException("i", "Value must be 1.");
            }

            return TimeSpan.FromDays(i);
        }

        public static TimeSpan Days(this int i)
        {
            return TimeSpan.FromDays(i);
        }

        public static DateTime Ago(this TimeSpan t)
        {
            return DateTime.Now - t;
        }

        public static DateTime Hence(this TimeSpan t)
        {
            return DateTime.Now + t;
        }
    }
}