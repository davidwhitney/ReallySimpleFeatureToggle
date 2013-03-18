using System.Linq;
using ReallySimpleFeatureToggle.FeatureOverrides;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class CookieSettingStorage : IFeatureOverrideRule
    {
        private readonly IFeatureOptionsCookieParser _featureOptionsCookieParser;

        public CookieSettingStorage(IFeatureOptionsCookieParser featureOptionsCookieParser)
        {
            _featureOptionsCookieParser = featureOptionsCookieParser;
        }

        public void Apply(FeatureConfiguration manifest, EvaluationContext context)
        {
            var featuresCookie = _featureOptionsCookieParser.GetFeatureOptionsCookie();

            foreach (var item in featuresCookie.FeatureStates.Where(item => manifest.ContainsKey(item.Key)))
            {
                manifest[item.Key].IsAvailable = item.Value;
            }
        }
    }
}