using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using ReallySimpleFeatureToggle.Configuration;
using DynamicExpression = System.Linq.Dynamic.DynamicExpression;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class DynamicAvailabilityRuleCompiler
    {
        private readonly Func<Type> _evaluationContextType;
        private readonly ConcurrentDictionary<string, IAvailabilityRule> _cachedAvailabilityRules; 

        public DynamicAvailabilityRuleCompiler(Func<Type> evaluationContextType)
        {
            _evaluationContextType = evaluationContextType;
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
            var featureParameter = Expression.Parameter(typeof (IFeature), "feature");
            var contextParameter = Expression.Parameter(_evaluationContextType(), "context");

            var lambdaExpression = DynamicExpression.ParseLambda(new[] {featureParameter, contextParameter}, typeof (bool), expression);

            return new DynamicAvailabilityRule(expression, lambdaExpression.Compile());
        }
    }
}
