using ReallySimpleFeatureToggle.Configuration;

namespace ReallySimpleFeatureToggle.FeatureStateEvaluation
{
    public interface IFeatureEvaluator
    {
        IFeatureConfiguration LoadConfiguration(string forTenant = Tenant.All);
    }
}