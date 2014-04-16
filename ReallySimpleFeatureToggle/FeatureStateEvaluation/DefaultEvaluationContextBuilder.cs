namespace ReallySimpleFeatureToggle.FeatureStateEvaluation
{
    public class DefaultEvaluationContextBuilder : IEvaluationContextBuilder
    {
        public EvaluationContext Create(string tenant)
        {
            return new EvaluationContext {Tenant = tenant};
        }
    }
}