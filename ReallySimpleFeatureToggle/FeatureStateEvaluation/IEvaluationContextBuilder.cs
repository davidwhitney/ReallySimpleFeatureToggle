namespace ReallySimpleFeatureToggle.FeatureStateEvaluation
{
    public interface IEvaluationContextBuilder
    {
        EvaluationContext Create(string tenant);
    }
}