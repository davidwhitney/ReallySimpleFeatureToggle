using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class DynamicAvailabilityRuleCompiler
    {
        private readonly Func<IEvaluationContextBuilder> _funcToGetConfiguredEvaluationContextTypeFrom;
        private readonly ConcurrentDictionary<string, IAvailabilityRule> _cachedAvailabilityRules; 

        public DynamicAvailabilityRuleCompiler(Func<IEvaluationContextBuilder> funcToGetConfiguredEvaluationContextTypeFrom)
        {
            _funcToGetConfiguredEvaluationContextTypeFrom = funcToGetConfiguredEvaluationContextTypeFrom;
            _cachedAvailabilityRules = new ConcurrentDictionary<string, IAvailabilityRule>();
        }

        public IAvailabilityRule TryCompile(string expression)
        {
            try
            {
                return _cachedAvailabilityRules.GetOrAdd(expression, Compile);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Failed to compile rule: '" + expression + "' because " + ex.Message);
                return null;
            }
        }

        private IAvailabilityRule Compile(string expression)
        {
            var instanceTypeToMapIntoLambda = _funcToGetConfiguredEvaluationContextTypeFrom().Create(Tenant.All).GetType();

            var featureParameter = Expression.Parameter(typeof (IFeature), "feature");
            var contextParameter = Expression.Parameter(instanceTypeToMapIntoLambda, "context");

            var lambdaExpression = DynamicExpression.ParseLambda(new[] {featureParameter, contextParameter}, typeof (bool), expression);

            return new DynamicAvailabilityRule(expression, lambdaExpression.Compile());
        }
    }
}
