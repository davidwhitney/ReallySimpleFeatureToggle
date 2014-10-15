using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Web.FeatureStateEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.MvcPluginTests
{
    [TestFixture]
    public class When_constructing
    {
        string _testTenant;
        private Mock<IEvaluationContextBuilder> _mockEvaluationContext;

        [SetUp]
        public void Setup()
        {
            _testTenant = "DEV1";

        }

        public void Because()
        {
            var mvcPlugin = new MvcPlugin(_testTenant);
        }


        [Test]
        public void Should_set_static_tenant_field()
        {
            Because();

            Assert.That(MvcPlugin._tenant, Is.EqualTo(_testTenant));
        }
    }
}
