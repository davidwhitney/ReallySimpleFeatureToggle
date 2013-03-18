using System.Collections.Specialized;
using System.Linq;
using System.Web;
using ReallySimpleFeatureToggle.FeatureOverrides;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Web.FeatureOverrides
{
    public class QueryStringOverrideRule : IFeatureOverrideRule
    {
        private const string QueryStringKey = "enableFeatures";

        public void Apply(FeatureConfiguration manifest, EvaluationContext context)
        {
            var qs = HttpContext.Current.Request.QueryString;
            if (!qs.AllKeys.AsEnumerable().Contains(QueryStringKey))
            {
                return;
            }
            Apply(manifest, context, qs);
        }

        public void Apply(FeatureConfiguration manifest, EvaluationContext context, NameValueCollection qs)
        {
            var qsValue = qs[QueryStringKey] ?? "";
            var qsItems = qsValue.Split(',');
            foreach (var featureName in qsItems)
            {
                if (!manifest.ContainsKey(featureName))
                {
                    continue;
                }
                
                manifest[featureName].IsAvailable = true;
            }
        }
    }
}