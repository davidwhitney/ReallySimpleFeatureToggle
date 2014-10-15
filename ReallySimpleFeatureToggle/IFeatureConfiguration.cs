using System;
using System.Collections.Generic;

namespace ReallySimpleFeatureToggle
{
    public interface IFeatureConfiguration : IDictionary<string, ActiveSettings>
    {
        string Tenant { get; set; }
        bool IsAvailable(Enum feature);
        bool IsAvailable(string featureName);
    }
}