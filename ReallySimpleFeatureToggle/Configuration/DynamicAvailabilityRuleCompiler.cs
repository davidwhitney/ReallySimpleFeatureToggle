using System;
using System.Linq.Expressions;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace ReallySimpleFeatureToggle.Configuration
{
    public class DynamicAvailabilityRuleCompiler
    {
        public IAvailabilityRule TryCompile(string expression)
        {
            try
            {
                return Compile(expression);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine("Failed to compile rule: '" + expression + "' because " + ex.Message);
                return null;
            }
        }

        public IAvailabilityRule Compile(string expression)
        {
            var featureParameter = Expression.Parameter(typeof(IFeature), "feature");
            var contextParameter = Expression.Parameter(typeof(EvaluationContext), "context");

            var lambdaExpression = DynamicExpression.ParseLambda(new[] { featureParameter, contextParameter }, typeof(bool), expression);
            var compiledExpression = lambdaExpression.Compile();

            return new DynamicAvailabilityRule(expression, compiledExpression);
        }
    }
}
