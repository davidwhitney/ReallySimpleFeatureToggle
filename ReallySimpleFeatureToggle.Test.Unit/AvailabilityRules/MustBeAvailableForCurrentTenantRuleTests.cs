using NUnit.Framework;
using ReallySimpleFeatureToggle.AvailabilityRules;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;

namespace ReallySimpleFeatureToggle.Test.Unit.AvailabilityRules
{
    [TestFixture]
    public class MustBeAvailableForCurrentTenantRuleTests
    {
        private MustBeAvailableForCurrentTenantRule _rule;

        [SetUp]
        public void SetUp()
        {
            _rule = new MustBeAvailableForCurrentTenantRule();
        }

        [Test]
        public void IsAvilable_WhenTenantMatches_ReturnsTrue()
        {
            var setting = new Feature("TestFeature")
            {
                State = State.Enabled,
                SupportedTenants = new[] {TestTenants.Tenant2, TestTenants.Tenant3}
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext { Tenant = TestTenants.Tenant2 });
            
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAvilable_WhenTenantDoesNotMatch_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                State = State.Enabled,
                SupportedTenants = new[] {TestTenants.Tenant2, TestTenants.Tenant3}
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext { Tenant = TestTenants.Tenant1 });

            Assert.That(result, Is.False);
        }

        [Test]
        public void IsAvilable_WhenTenantIsAll_ReturnsTrue()
        {
            var setting = new Feature("TestFeature")
            {
                State = State.Enabled,
                SupportedTenants = new[] {TestTenants.All}
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext { Tenant = TestTenants.Tenant1 });

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsAvilable_WhenTenantIsExcluded_ReturnsFalse()
        {
            var setting = new Feature("TestFeature")
            {
                State = State.Enabled,
                SupportedTenants = new[] {TestTenants.All},
                ExcludedTenants = new[] {TestTenants.Tenant3}
            };

            var result = _rule.IsAvailable(setting, new EvaluationContext { Tenant = TestTenants.Tenant3 });

            Assert.That(result, Is.False);
        }
    }
}