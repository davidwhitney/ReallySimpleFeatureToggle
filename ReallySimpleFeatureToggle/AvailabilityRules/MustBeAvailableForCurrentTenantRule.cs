using System.Linq;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.AvailabilityRules
{
    public class MustBeAvailableForCurrentTenantRule : IAvailabilityRule
    {
        public bool IsAvailable(IFeature feature, EvaluationContext context)
        {
            if (feature.ExcludedTenants.ToList().Any(x => x == context.Tenant))
            {
                return false;
            }

            return feature.SupportedTenants
                    .ToList()
                    .Any(x => 
                        string.IsNullOrWhiteSpace(x)
                        || x.ToLower() == Tenant.All.ToLower()
                        || x.ToLower() == context.Tenant.ToLower());
        }
    }
}