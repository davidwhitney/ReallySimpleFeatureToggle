using System;

namespace ReallySimpleFeatureToggle.FeatureStateEvaluation
{
    public class EvaluationContext
    {
        public string Tenant { get; set; }
        public DateTime CurrentDateTime { get; set; }

        public EvaluationContext()
        {
            CurrentDateTime = DateTime.Now;
        }
    }
}