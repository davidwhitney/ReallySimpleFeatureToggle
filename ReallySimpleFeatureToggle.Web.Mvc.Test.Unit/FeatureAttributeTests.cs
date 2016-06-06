using System;
using System.Collections.Generic;
using System.Web.Mvc;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.Web.Mvc.Test.Unit.Fakes;

namespace ReallySimpleFeatureToggle.Web.Mvc.Test.Unit
{
    [TestFixture]
    public class FeatureAttributeTests
    {
        private ActionExecutingContext _ctx;
        private List<IFeature> _features;
        private FeatureAttribute _attr;
        private IFeatureNotConfiguredBehaviour _featureNotConfiguredBehaviour;

        [SetUp]
        public void Setup()
        {
            _attr = new FeatureAttribute();
            _ctx = new ActionExecutingContext
            {
                Controller = new FeatureAttributeTestController(),
                ActionDescriptor = new FakeActionDescriptor(typeof(FeatureAttributeTestController)),
                HttpContext = new FakeHttpContext()
            };

            _features = new List<IFeature>();
            _featureNotConfiguredBehaviour = new ThrowANotConfiguredException();

            FeatureAttribute.GetFeatureConfiguration = () => new ReallySimpleFeature().Configure
                .WithFeatures(fc => { _features.ForEach(fc.Add);})
                .WhenFeatureRequestedIsNotConfigured(_featureNotConfiguredBehaviour)
                .And().GetFeatureConfiguration();
        }

        [Test]
        public void CtorNoParams_FeatureIsUnknown_DoesntOverwriteResult()
        {
            _features.Clear();
            ((FakeActionDescriptor)_ctx.ActionDescriptor)._actionName = "Index";

            _attr.OnActionExecuting(_ctx);

            Assert.That(_ctx.Result, Is.Null);
        }

        /// <summary>
        /// The behahviour between inferred feature names, and supplied ones is different by design.
        /// Features that are "inferred" may be from a globally applied filter, so we don't throw
        /// if they're not available.
        /// 
        /// Features explicitly supplied, are deemed expected, and follow the unconfigured feature
        /// strategies.
        /// </summary>
        [TestCase("Index")]
        [TestCase("FeatureAttributeTestControllerIndex")]
        public void CtorNoParams_FeatureIsEnabled_DoesntOverwriteResult(string featureName)
        {
            _features.Add(Feature.Called(featureName).ThatIsEnabled());
            ((FakeActionDescriptor)_ctx.ActionDescriptor)._actionName = "Index";

            _attr.OnActionExecuting(_ctx);

            Assert.That(_ctx.Result, Is.Null);
        }

        [TestCase("Index")]
        [TestCase("FeatureAttributeTestControllerIndex")]
        public void CtorNoParams_FeatureIsDisabled_MatchesBasedOnControllerActionAndReturnsEmptyResult(string featureName)
        {
            _features.Add(Feature.Called(featureName).ThatIsDisabled());
            ((FakeActionDescriptor)_ctx.ActionDescriptor)._actionName = "Index";

            _attr.OnActionExecuting(_ctx);

            Assert.That(_ctx.Result, Is.TypeOf<EmptyResult>());
        }

        [Test]
        public void CtorSpecifiedFeature_FeatureIsEnabled_DoesNotOverrideResult()
        {
            _attr = new FeatureAttribute("Index");
            _features.Add(Feature.Called("Index").ThatIsEnabled());
            ((FakeActionDescriptor) _ctx.ActionDescriptor)._actionName = "Index";

            _attr.OnActionExecuting(_ctx);

            Assert.That(_ctx.Result, Is.Null);
        }

        [Test]
        public void CtorSpecifiedFeature_FeatureIsUnknownAndBehaviourConfiguredToThrow_Throws()
        {
            _featureNotConfiguredBehaviour = new ThrowANotConfiguredException();

            _attr = new FeatureAttribute("Index");
            ((FakeActionDescriptor) _ctx.ActionDescriptor)._actionName = "Index";

            Assert.Throws<FeatureNotConfiguredException>(() => _attr.OnActionExecuting(_ctx));
        }

        [Test]
        public void CtorSpecifiedFeature_FeatureIsUnknownAndBehaviourConfiguredToReturnFalse_ReturnsNull()
        {
            _featureNotConfiguredBehaviour = new ReturnBoolWhenFeatureNotConfigured(false);

            _attr = new FeatureAttribute("Index");
            ((FakeActionDescriptor) _ctx.ActionDescriptor)._actionName = "Index";

            _attr.OnActionExecuting(_ctx);
            
            Assert.That(_ctx.Result, Is.TypeOf<EmptyResult>());
        }

        [Test]
        public void CtorSpecifiedFeature_FeatureIsDisabled_ReturnsEmptyResult()
        {
            _attr = new FeatureAttribute("Index");
            _features.Add(Feature.Called("Index").ThatIsDisabled());
            ((FakeActionDescriptor)_ctx.ActionDescriptor)._actionName = "Index";

            _attr.OnActionExecuting(_ctx);

            Assert.That(_ctx.Result, Is.TypeOf<EmptyResult>());
        }
    }

    public class FeatureAttributeTestController : Controller
    {
        public ActionResult Index()
        {
            return new EmptyResult();
        }
    }
}
