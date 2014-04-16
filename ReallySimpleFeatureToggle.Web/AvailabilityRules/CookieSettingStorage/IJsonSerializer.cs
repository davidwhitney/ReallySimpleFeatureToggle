namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public interface IJsonSerializer
    {
        string SerializeObject(object obj);
        T DeserializeObject<T>(string value);
    }
}