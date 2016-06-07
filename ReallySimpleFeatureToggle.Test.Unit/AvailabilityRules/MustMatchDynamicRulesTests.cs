using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Test.Unit.AvailabilityRules
{
    [TestFixture]
    public class MustMatchDynamicRulesTests
    {
        private IFeature _feature;

        [SetUp]
        public void SetUp()
        {
            _feature = Feature.Called("Something");
        }

        [TestCase("test", true)]
        [TestCase("something else", false)]
        public void IsAvailable_GivenADynamicRule_CanCompileAndRun(string tenant, bool matches)
        {
            var evaluationContext = new EvaluationContext { Tenant = tenant };
            var compiler = new DynamicAvailabilityRuleCompiler(() => evaluationContext.GetType());
            var rule = compiler.TryCompile(@"context.Tenant == ""test""");

            var available = rule.IsAvailable(_feature, evaluationContext);

            Assert.That(available, Is.EqualTo(matches));
        }
    }
}
