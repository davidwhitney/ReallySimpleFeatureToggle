using System.Web;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Web.FeatureStateEvaluation
{
    public class EvaluationContextForWebProjects : EvaluationContext
    {
        public HttpContextBase HttpContext { get; set; }
    }
}