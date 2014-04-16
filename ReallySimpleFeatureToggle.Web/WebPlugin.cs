using System.Linq;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Extensibility;
using ReallySimpleFeatureToggle.Infrastructure;
using ReallySimpleFeatureToggle.Web.AvailabilityRules;
using ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage;
using ReallySimpleFeatureToggle.Web.FeatureOverrides;
using ReallySimpleFeatureToggle.Web.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Web
{
    public class WebPlugin : IPluginBootstrapper 
    {
        public bool EnabledQueryStringOverride { get; set; }
        public bool PersistRandomPercentageResultToCookie { get; set; }

        public WebPlugin(bool enabledQueryStringOverride = false, bool persistRandomPercentageResultToCookie = false)
        {
            EnabledQueryStringOverride = enabledQueryStringOverride;
            PersistRandomPercentageResultToCookie = persistRandomPercentageResultToCookie;
        }

        public void Configure(ReallySimpleFeatureToggleConfigurationApi configurationApi)
        {
            if (EnabledQueryStringOverride)
            {
                configurationApi.Parent.Configure.GlobalOverrides(rules => rules.Add(new QueryStringOverrideRule()));
            }

            if (PersistRandomPercentageResultToCookie)
            {
                configurationApi.Parent.Configure.GlobalAvailabilityRules(rules =>
                {
                    rules.Remove(rules.Single(x => x.GetType() == typeof (MustBeEnabledOrEnabledForPercentageRule)));
                    rules.Add(new MustBeEnabledOrEnabledForPercentageStickyRule(new RandomNumberGenerator(), new FeatureOptionsCookieParser()));
                });
            }

            configurationApi.Parent.Configure.CreateEvaluationContextBy(new EvaluationContextBuilderForWeb());
        }
    }
}