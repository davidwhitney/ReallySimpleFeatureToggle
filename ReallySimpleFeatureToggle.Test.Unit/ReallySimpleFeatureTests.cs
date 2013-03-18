using NUnit.Framework;

namespace ReallySimpleFeatureToggle.Test.Unit
{
    [TestFixture]
    public class ReallySimpleFeatureTests
    {
        [Test]
        public void Toggles_ReturnsSameInstanceRepeatedly()
        {
            Assert.That(ReallySimpleFeature.Toggles, Is.EqualTo(ReallySimpleFeature.Toggles));
        }
    }
}