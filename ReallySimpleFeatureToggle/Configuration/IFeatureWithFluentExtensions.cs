using System;

namespace ReallySimpleFeatureToggle.Configuration
{
    public interface IFeatureWithFluentExtensions : IFeature
    {
        IFeatureWithFluentExtensions ThatIsEnabled();
        IFeatureWithFluentExtensions ThatIsDisabled();
        IFeatureWithFluentExtensions EnabledForPercentage(int percentage);
        IFeatureWithFluentExtensions OnlyAvailableBetween(DateTime startTime, DateTime endTime);
    }
}