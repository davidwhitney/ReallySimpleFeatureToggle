using System;
using System.Web;

namespace ReallySimpleFeatureToggle.FeatureStateEvaluation
{
    public class EvaluationContext
    {
        public string Tenant { get; set; }
        public DateTime CurrentDateTime { get; set; }
        public AppDomain Application { get; set; }
        public HttpContextBase HttpContext { get; set; }

        public EvaluationContext()
        {
            CurrentDateTime = DateTime.Now;
            Application = AppDomain.CurrentDomain;
            
            if (System.Web.HttpContext.Current != null)
            {
                HttpContext = new HttpContextWrapper(System.Web.HttpContext.Current);
            }
        }
    }
}