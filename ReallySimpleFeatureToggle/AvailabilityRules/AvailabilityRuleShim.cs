using System;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class AvailabilityRuleShim : IAvailabilityRule
    {
        public Func<IFeature, EvaluationContext, bool> Evaluation { get; set; }

        public AvailabilityRuleShim(Func<IFeature, EvaluationContext, bool> evaluation)
        {
            Evaluation = evaluation;
        }

        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            return Evaluation(feature, context);
        }
    }
}