using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.FeatureOverrides;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Infrastructure;
using ReallySimpleFeatureToggle.Web.AvailabilityRules;
using ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage;

namespace ReallySimpleFeatureToggle.Web.Test.Unit.AvailabilityRules
{
    [TestFixture]
    public class FeatureSettingExtensionsPersistentStateBasedRuleTests
    {
        private FeatureEvaluator _evaluator;
        private Mock<IFeatureOptionsCookieParser> _featureOptionsCookieParser;
        private Mock<IFeatureRepository> _featureSettingRepo;
        private IList<IFeature> _featureSettings;
        private List<IAvailabilityRule> _featureAvailabilityRules;

        [SetUp]
        public void SetUp()
        {
            _featureSettings = new List<IFeature>();
            _featureSettingRepo = new Mock<IFeatureRepository>();
            _featureSettingRepo.Setup(x => x.GetFeatureSettings()).Returns(_featureSettings);
                       
            _featureOptionsCookieParser = new Mock<IFeatureOptionsCookieParser>();
            _featureOptionsCookieParser.Setup(x => x.GetFeatureOptionsCookie()).Returns(new FeatureOptionsCookie());
            var stateRule = new MustBeEnabledOrEnabledForPercentageStickyRule(new RandomNumberGenerator(), _featureOptionsCookieParser.Object);

            _featureAvailabilityRules = new List<IAvailabilityRule> { stateRule };

            _evaluator = new FeatureEvaluator(_featureSettingRepo.Object,
                                                      _featureAvailabilityRules, new List<IFeatureOverrideRule>(), new ThrowANotConfiguredException());
        }

        [Test]
        public void IsAvailable_WhenFeatureDisabled_ReturnsFalse()
        {
            _featureSettings.Add(new Feature("TestFeature")
            {
                State = State.Disabled
            });

            var manifest = _evaluator.LoadConfiguration();
            var result = manifest[_featureSettings[0].Name].IsAvailable;

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAvailable_WhenFeatureEnabled_ReturnsTrue()
        {
            _featureSettings.Add(new Feature("TestFeature")
            {
                State = State.Enabled
            });

            var manifest = _evaluator.LoadConfiguration();
            var result = manifest[_featureSettings[0].Name].IsAvailable;

            Assert.That(result, Is.True);
        }
    }
}