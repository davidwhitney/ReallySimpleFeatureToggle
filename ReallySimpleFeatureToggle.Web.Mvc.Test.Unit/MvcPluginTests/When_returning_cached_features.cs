using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Web.FeatureStateEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.MvcPluginTests
{
    [TestFixture]
    public class When_returning_cached_features
    {
        string _testTenant;
        private Mock<IEvaluationContextBuilder> _mockEvaluationContext;

        [SetUp]
        public void SetUp()
        {

            _testTenant = "DEV1";
            _mockEvaluationContext = new Mock<IEvaluationContextBuilder>();
            _mockEvaluationContext.Setup(x => x.Create(It.IsAny<string>())).Returns(new EvaluationContext());
            HttpContext.Current = new HttpContext(new HttpRequest("", "http://www.bing.com", ""), new HttpResponse(null));

            var mvcPlugin = new MvcPlugin(_testTenant);
            ReallySimpleFeature.Toggles.Configure
                .WithPlugin(mvcPlugin)
                .WithEvaluationContextOf<EvaluationContextForWebProjects>(_mockEvaluationContext.Object);
        }

        public void Because()
        {
            WhenEnabled.GetFeatureConfiguration();
        }

        [Test]
        public void Should_pass_tenant_to_context_builder()
        {
            Because();

            _mockEvaluationContext.Verify(x => x.Create(_testTenant));
        }
    }
}
