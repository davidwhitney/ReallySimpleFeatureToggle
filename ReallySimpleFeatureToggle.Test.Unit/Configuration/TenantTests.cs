using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;

namespace ReallySimpleFeatureToggle.Test.Unit.Configuration
{
    [TestFixture]
    public class TenantTests
    {
        [TestCase("tenant1", "tenant1")]
        [TestCase("", Tenant.All)]
        [TestCase(null, Tenant.All)]
        public void Parse_ForVariousPermutations_CorrectsString(string tenantRequested, string tenantExpected)
        {
            var correctString = Tenant.Parse(tenantExpected);

            Assert.That(correctString, Is.EqualTo(tenantExpected));
        }
    }
}
