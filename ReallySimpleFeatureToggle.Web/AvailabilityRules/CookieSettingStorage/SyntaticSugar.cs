using System;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
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