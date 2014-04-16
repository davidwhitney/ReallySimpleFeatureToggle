using System.Web;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Web.FeatureStateEvaluation
{
    public class EvaluationContextBuilderForWeb : IEvaluationContextBuilder
    {
        public EvaluationContext Create(string tenant)
        {
            return new EvaluationContextForWebProjects
            {
                HttpContext = new HttpContextWrapper(HttpContext.Current)
            };
        }
    }
}