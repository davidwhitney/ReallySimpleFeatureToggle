using ReallySimpleFeatureToggle.FeatureOverrides;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Test.Unit.TestHelpers
{
    public class TestOverrideThatDoesNothing : IFeatureOverrideRule
    {
        public void Apply(FeatureConfiguration manifest, EvaluationContext context)
        {
            WasCalled = true;
        }

        public bool WasCalled { get; set; }
    }
}