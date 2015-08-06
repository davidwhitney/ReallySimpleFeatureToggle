using System.Linq;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;

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

        [Test]
        public void FeatureSettings_WhenFeatureChangesStateAtRuntime_RespectsChange()
        {
            var fConfig = new ReallySimpleFeature();
            fConfig.Configure.WithFeatures(features =>
            {
                features.Add(new Feature("abc").ThatIsEnabled());
            });

            Assert.That(fConfig.GetFeatureConfiguration().IsAvailable("abc"), Is.EqualTo(true));

            // Modify raw config
            fConfig.FeatureSettings.First().State = State.Disabled;

            // Reeval
            Assert.That(fConfig.GetFeatureConfiguration().IsAvailable("abc"), Is.EqualTo(false));
        }
    }
}