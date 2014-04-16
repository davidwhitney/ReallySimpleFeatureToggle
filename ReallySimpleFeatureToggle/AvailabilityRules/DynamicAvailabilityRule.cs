using System;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class DynamicAvailabilityRule : IAvailabilityRule
    {
        public string Expression { get; private set; }
        private readonly Delegate _methoDelegate;

        public DynamicAvailabilityRule(string expression, Delegate methoDelegate)
        {
            Expression = expression;
            _methoDelegate = methoDelegate;
        }

        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            return (bool)_methoDelegate.DynamicInvoke(feature, context);
        }
    }
}