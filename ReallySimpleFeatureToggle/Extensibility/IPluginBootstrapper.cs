namespace ReallySimpleFeatureToggle.Extensibility
{
    public interface IPluginBootstrapper
    {
        void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi);
    }
}