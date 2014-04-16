using System;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.AppConfigProvider;
using ReallySimpleFeatureToggle.Configuration.FeatureNotConfiguredBehaviours;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;

namespace ReallySimpleFeatureToggle.Test.Unit
{
    [TestFixture]
    public class ReallySimpleFeatureUsageExamples
    {
        [Test]
        public void ExampleOfOverrides()
        {
            var featureSet = new ReallySimpleFeature().Configure
                .GlobalAvailabilityRules(x =>
                {
                    x.Add(new TestAvailabilityRuleThatReturns(false));
                })
                .GlobalOverrides(x =>
                {
                    x.Add(new TestOverrideThatDoesNothing());
                })
                .WhenFeatureRequestedIsNotConfigured(new FeatureShouldBeEnabled())
                .WithFeatureConfigurationSource(new AppConfigFeatureRepository(), new TimeSpan(500))
                .And().GetFeatureConfiguration();

            var available = featureSet.IsAvailable(TestFeaturesFromAppConfig.EnabledFeature);

            Assert.That(available, Is.False);
        }

        [Test]
        public void ExampleOfFluentConfiguration()
        {
            var featureSet = new ReallySimpleFeature().Configure
                .WithFeatures(x =>
                {
                    x.Add(Feature.Called("Awesome").ThatIsEnabled()
                        .WithCustomAvailabilityRule((feature, context) =>
                        {
                            return true;
                        }));
                    x.Add(Feature.Called("Awesome2").ThatIsEnabled());
                    x.Add(Feature.Called("Awesome3").ThatIsDisabled());
                    x.Add(Feature.Called("Awesome4").EnabledForPercentage(50));
                    x.Add(Feature.Called("Awesome5").OnlyAvailableBetween(DateTime.Now, DateTime.Now.AddDays(5)));
                })
                .And().GetFeatureConfiguration();

            Assert.That(featureSet.IsAvailable("Awesome"), Is.True);
            Assert.That(featureSet.IsAvailable("Awesome3"), Is.False);
        }
    }
}
