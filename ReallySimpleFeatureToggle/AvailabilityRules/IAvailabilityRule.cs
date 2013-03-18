using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    /// <summary>
    /// All registered IAvailabilityRules must return true for a feature to be available
    /// </summary>
    public interface IAvailabilityRule
    {
        bool IsAvailable(IFeature feature, EvaluationContext context);
    }
}