using System.Collections.Specialized;
using NUnit.Framework;
using ReallySimpleFeatureToggle.FeatureStateEvaluation;
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
            var config = new FeatureConfiguration { { "feature", new ActiveSettings { IsAvailable = false } } };
            
            @override.Apply(config, new EvaluationContext(), queryString);

            Assert.That(config["feature"].IsAvailable, Is.False);
        }

        [Test]
        public void Apply_FeatureExists_FeatureIsEnabled()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection { { ExpectedQueryStringKey, TestFeatures.International } };
            var config = new FeatureConfiguration { { TestFeatures.International, new ActiveSettings { IsAvailable = false } } };

            @override.Apply(config, new EvaluationContext(), queryString);

            Assert.That(config[TestFeatures.International].IsAvailable, Is.True);
        }

        [Test]
        public void Apply_FeatureExistsAndIsEnabled_FeatureIsEnabled()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection { { ExpectedQueryStringKey, TestFeatures.International } };
            var config = new FeatureConfiguration { { TestFeatures.International, new ActiveSettings { IsAvailable = true} } };

            @override.Apply(config, new EvaluationContext(), queryString);

            Assert.That(config[TestFeatures.International].IsAvailable, Is.True);
        }

        [Test]
        public void Apply_BothFeaturesExist_BothFeaturesEnabled()
        {
            var @override = new QueryStringOverrideRule();
            var queryString = new NameValueCollection {{ExpectedQueryStringKey, TestFeatures.International + "," + TestFeatures.TestFeature5}};
            var config = new FeatureConfiguration
            {
                {TestFeatures.International, new ActiveSettings {IsAvailable = false}},
                {TestFeatures.TestFeature5, new ActiveSettings {IsAvailable = false}}
            };

            @override.Apply(config, new EvaluationContext(), queryString);

            Assert.That(config[TestFeatures.International].IsAvailable, Is.True);
            Assert.That(config[TestFeatures.TestFeature5].IsAvailable, Is.True);
        }
    }
}