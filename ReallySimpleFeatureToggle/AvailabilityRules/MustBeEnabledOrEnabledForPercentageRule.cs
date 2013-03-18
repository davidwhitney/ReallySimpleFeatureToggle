using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Infrastructure;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class MustBeEnabledOrEnabledForPercentageRule : IAvailabilityRule
    {
        private readonly IRandomNumberGenerator _randomNumberGenerator;

        public MustBeEnabledOrEnabledForPercentageRule(IRandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
        }

        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            if(feature.State == State.EnabledForPercentage)
            {
                var number = _randomNumberGenerator.GetRandomNumberBetween(0, 100);
                var decision = number < feature.RandomPercentageEnabled;
                return decision;
            }

            return feature.State == State.Enabled;
        }
    }
}