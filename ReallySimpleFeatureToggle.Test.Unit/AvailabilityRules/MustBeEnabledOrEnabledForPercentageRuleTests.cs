using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Infrastructure;

namespace ReallySimpleFeatureToggle.Test.Unit.AvailabilityRules
{
    [TestFixture]
    public class MustBeEnabledOrEnabledForPercentageRuleTests
    {
        private MustBeEnabledOrEnabledForPercentageRule _rule;

        [SetUp]
        public void SetUp()
        {
            _rule = new MustBeEnabledOrEnabledForPercentageRule(new RandomNumberGenerator());
        }

        [Test]
        public void IsAvailable_WhenFeatureDisabled_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                State = State.Disabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAvailable_WhenFeatureEnabled_ReturnsTrue()
        {
            var setting = new Feature("TestFeature")
            {
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.True);
        }
    }
}