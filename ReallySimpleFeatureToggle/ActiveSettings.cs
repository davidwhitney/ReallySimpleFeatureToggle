using System.Collections.Generic;

namespace ReallySimpleFeatureToggle
{
    public class ActiveSettings
    {
        public bool IsAvailable { get; set; }
        public IList<string> Dependencies { get; set; }
    }
}