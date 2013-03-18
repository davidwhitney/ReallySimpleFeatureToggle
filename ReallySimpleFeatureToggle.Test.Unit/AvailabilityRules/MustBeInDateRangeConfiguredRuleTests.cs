using System;
using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;

namespace ReallySimpleFeatureToggle.Test.Unit.AvailabilityRules
{
    [TestFixture]
    public class MustBeInDateRangeConfiguredRuleTests
    {
        private MustBeInDateRangeConfiguredRule _rule;

        [SetUp]
        public void SetUp()
        {
            _rule = new MustBeInDateRangeConfiguredRule();
        }

        [Test]
        public void IsAvailable_WhenDateEarlierThanStartDate_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                StartDtg = DateTime.Now.AddHours(1),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAvailable_WhenDateLaterThanStartDate_ReturnsTrue()
        {
            var setting = new Feature("TestFeature")
            {
                StartDtg = DateTime.Now.AddHours(-1),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAvailable_WhenDateLaterThanEndDate_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                EndDtg = DateTime.Now.AddHours(-1),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAvailable_WhenDateEarlierThanEndDate_ReturnsTrue()
        {
            var setting = new Feature("TestFeature")
            {
                EndDtg = DateTime.Now.AddHours(1),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAvailable_WhenDateWithinDateRange_ReturnsTrue()
        {
            var setting = new Feature("TestFeature")
            {
                StartDtg = DateTime.Now.AddHours(-1),
                EndDtg = DateTime.Now.AddHours(1),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAvailable_WhenDateEarlierThanDateRange_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                StartDtg = DateTime.Now.AddHours(1),
                EndDtg = DateTime.Now.AddHours(2),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAvailable_WhenDateGreaterThanDateRange_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                StartDtg = DateTime.Now.AddHours(-2),
                EndDtg = DateTime.Now.AddHours(-1),
                State = State.Enabled
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext());

            Assert.That(result, Is.False);
        }
    }
}
