using System;

namespace ReallySimpleFeatureToggle.Configuration
{
    public interface IFeature
    {
        string Name { get; set; }
        State State { get; set; }
        string[] Dependencies { get; set; }
        string[] SupportedTenants { get; set; }
        string[] ExcludedTenants { get; set; }
        DateTime StartDtg { get; set; }
        DateTime EndDtg { get; set; }

        int RandomPercentageEnabled { get; set; }
    }
}