using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Infrastructure;
using ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules
{
    public class MustBeEnabledOrEnabledForPercentageStickyRule : IAvailabilityRule
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;
        private readonly IFeatureOptionsCookieParser _featureOptionsCookieParser;

        public MustBeEnabledOrEnabledForPercentageStickyRule(IRandomNumberGenerator randomNumberGenerator, IFeatureOptionsCookieParser featureOptionsCookieParser)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _featureOptionsCookieParser = featureOptionsCookieParser;
        }

        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            if (feature.State == State.EnabledForPercentage)
            {
                var number = _randomNumberGenerator.GetRandomNumberBetween(0, 100);
                var decision = number < feature.RandomPercentageEnabled;

                var featureSettings = _featureOptionsCookieParser.GetFeatureOptionsCookie();
                if (!featureSettings.FeatureStates.ContainsKey(feature.Name))
                {
                    _featureOptionsCookieParser.AddFeatureSetting(feature.Name, decision);
                }

                return decision;
            }

            return feature.State == State.Enabled;
        }
    }
}