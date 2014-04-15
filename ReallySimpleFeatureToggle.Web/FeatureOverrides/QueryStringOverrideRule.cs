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
            if (!HttpContext.Current.Request.QueryString.AllKeys.Contains(QueryStringKey))
            {
                return;
            }

            Apply(manifest, context, HttpContext.Current.Request.QueryString);
        }

        public void Apply(FeatureConfiguration manifest, EvaluationContext context, NameValueCollection qs)
        {
            var qsValue = qs[QueryStringKey] ?? "";
            var qsItems = qsValue.Split(',');

            foreach (var featureName in qsItems.Where(manifest.ContainsKey))
            {
                manifest[featureName].IsAvailable = true;
            }
        }
    }
}