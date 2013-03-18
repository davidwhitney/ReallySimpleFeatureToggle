using System;

namespace ReallySimpleFeatureToggle
{
    public class FeatureNotConfiguredException : Exception
    {
        public FeatureNotConfiguredException(string feature, Exception innerException)
            : base(string.Format("Feature configuration not found for \"{0}\".", feature), innerException)
        {
        }
    }
}