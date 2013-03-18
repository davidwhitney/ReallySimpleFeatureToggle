using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.FeatureOverrides
{
    public interface IFeatureOverrideRule
    {
        void Apply(FeatureConfiguration manifest, EvaluationContext context);
    }
}