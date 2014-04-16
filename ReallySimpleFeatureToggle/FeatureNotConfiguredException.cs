using System;

namespace ReallySimpleFeatureToggle
{
    public class FeatureNotConfiguredException : Exception
    {
        public FeatureNotConfiguredException(string feature)
            : base(string.Format("Feature configuration not found for \"{0}\".", feature))
        {
        }
    }
}