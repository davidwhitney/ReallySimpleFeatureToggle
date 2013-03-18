using System;
using System.Collections.Generic;

namespace ReallySimpleFeatureToggle
{
    public interface IFeatureConfiguration : IDictionary<string, ActiveSettings>
    {
        bool IsAvailable(Enum feature);
        bool IsAvailable(string featureName);
    }
}