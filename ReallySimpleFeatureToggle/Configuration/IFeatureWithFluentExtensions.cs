using System;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Configuration
{
    public interface IFeatureWithFluentExtensions : IFeature
    {
        IFeatureWithFluentExtensions ThatIsEnabled();
        IFeatureWithFluentExtensions ThatIsDisabled();
        IFeatureWithFluentExtensions EnabledForPercentage(int percentage);
        IFeatureWithFluentExtensions OnlyAvailableBetween(DateTime startTime, DateTime endTime);

        IFeatureWithFluentExtensions WithCustomAvailabilityRule(Func<IFeature, EvaluationContext, bool> evaluation);
        IFeatureWithFluentExtensions WithCustomAvailabilityRule(IAvailabilityRule rule);
    }
}