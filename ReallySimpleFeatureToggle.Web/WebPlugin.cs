using ReallySimpleFeatureToggle.Extensibility;
using ReallySimpleFeatureToggle.Web.FeatureOverrides;

namespace ReallySimpleFeatureToggle.Web
{
    public class WebPlugin : IPluginBootstrapper 
    {
        public void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi)
        {
            configurationApi.Parent.Configure.GlobalOverrides(rules => rules.Add(new QueryStringOverrideRule()));
        }
    }
}