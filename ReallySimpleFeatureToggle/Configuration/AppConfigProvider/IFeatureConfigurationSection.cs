namespace ReallySimpleFeatureToggle.Configuration.AppConfigProvider
{
    public interface IFeatureConfigurationSection
    {
        FeatureConfigurationElementCollection FeatureSettings { get; }
    }
}