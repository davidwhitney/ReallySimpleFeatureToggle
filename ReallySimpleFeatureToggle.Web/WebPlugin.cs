using System.Linq;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Extensibility;
using ReallySimpleFeatureToggle.Infrastructure;
using ReallySimpleFeatureToggle.Web.AvailabilityRules;
using ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage;
using ReallySimpleFeatureToggle.Web.FeatureOverrides;

namespace ReallySimpleFeatureToggle.Web
{
    public class WebPlugin : IPluginBootstrapper 
    {
        public void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi)
        {
            configurationApi.Parent.Configure.GlobalOverrides(rules => rules.Add(new QueryStringOverrideRule()));

            configurationApi.Parent.Configure.GlobalAvailabilityRules(rules =>
            {
                rules.Remove(rules.Single(x => x.GetType() == typeof (MustBeEnabledOrEnabledForPercentageRule)));
                rules.Add(new MustBeEnabledOrEnabledForPercentageStickyRule(new RandomNumberGenerator(), new FeatureOptionsCookieParser()));
            });
        }
    }
}