namespace ReallySimpleFeatureToggle.Configuration
{    
    public class Tenant
    {
        public const string All = "All";

        public static string Parse(string tenant)
        {
            return string.IsNullOrWhiteSpace(tenant) ? All : tenant;
        }
    }
}