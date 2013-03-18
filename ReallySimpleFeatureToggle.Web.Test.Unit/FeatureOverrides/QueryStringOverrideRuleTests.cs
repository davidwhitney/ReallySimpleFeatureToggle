using System.Collections.Specialized;
using NUnit.Framework;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
using ReallySimpleFeatureToggle.Test.Unit;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;
using ReallySimpleFeatureToggle.Web.FeatureOverrides;

namespace ReallySimpleFeatureToggle.Web.Test.Unit.FeatureOverrides
{
    [TestFixture]
    public class QueryStringOverrideRuleTests
    {
        const string ExpectedQueryStringKey = "enableFeatures";

        [Test]
        public void Apply_FeatureDoesntExist_NothingHappens()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection { { "somethingNotTheRightKey", "someRandomValue" } };
            var manifest = new FeatureConfiguration { { "feature", new ActiveSettings { IsAvailable = false } } };
            
            @override.Apply(manifest, new EvaluationContext(), queryString);

            Assert.That(manifest["feature"].IsAvailable, Is.False);
        }

        [Test]
        public void Apply_FeatureExists_FeatureIsEnabled()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection { { ExpectedQueryStringKey, TestFeatures.International } };
            var manifest = new FeatureConfiguration { { TestFeatures.International, new ActiveSettings { IsAvailable = false } } };

            @override.Apply(manifest, new EvaluationContext(), queryString);

            Assert.That(manifest[TestFeatures.International].IsAvailable, Is.True);
        }

        [Test]
        public void Apply_FeatureExistsAndIsEnabled_FeatureIsEnabled()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection { { ExpectedQueryStringKey, TestFeatures.International } };
            var manifest = new FeatureConfiguration { { TestFeatures.International, new ActiveSettings { IsAvailable = true} } };

            @override.Apply(manifest, new EvaluationContext(), queryString);

            Assert.That(manifest[TestFeatures.International].IsAvailable, Is.True);
        }

        [Test]
        public void Apply_BothFeaturesExist_BothFeaturesEnabled()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection {{ExpectedQueryStringKey, TestFeatures.International + "," + TestFeatures.TestFeature5}};
            var manifest = new FeatureConfiguration
            {
                {TestFeatures.International, new ActiveSettings {IsAvailable = false}},
                {TestFeatures.TestFeature5, new ActiveSettings {IsAvailable = false}}
            };

            @override.Apply(manifest, new EvaluationContext(), queryString);

            Assert.That(manifest[TestFeatures.International].IsAvailable, Is.True);
            Assert.That(manifest[TestFeatures.TestFeature5].IsAvailable, Is.True);
        }
    }
}