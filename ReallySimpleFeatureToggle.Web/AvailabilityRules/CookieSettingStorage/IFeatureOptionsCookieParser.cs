namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public interface IFeatureOptionsCookieParser
    {
        FeatureOptionsCookie GetFeatureOptionsCookie();
        void SetFeatureOptionsCookie(FeatureOptionsCookie featureOptions);
        void AddFeatureSetting(string feature, bool isEnabled);
        void ClearFeatureSetting(string feature);
        void ClearSavedOptions();
    }
}