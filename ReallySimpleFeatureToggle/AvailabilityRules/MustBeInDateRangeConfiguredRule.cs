using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class MustBeInDateRangeConfiguredRule : IAvailabilityRule
    {
        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            return feature.StartDtg <= context.CurrentDateTime && feature.EndDtg > context.CurrentDateTime;
        }
    }
}