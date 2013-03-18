using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Test.Unit.TestHelpers
{
    public class TestAvailabilityRuleThatReturns : IAvailabilityRule
    {
        private readonly bool _value;

        public TestAvailabilityRuleThatReturns(bool value)
        {
            _value = value;
        }

        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            return _value;
        }
    }
}