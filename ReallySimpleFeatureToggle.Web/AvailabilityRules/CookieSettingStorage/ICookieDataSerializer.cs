namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public interface ICookieDataSerializer
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string value) where T : class;
    }
}