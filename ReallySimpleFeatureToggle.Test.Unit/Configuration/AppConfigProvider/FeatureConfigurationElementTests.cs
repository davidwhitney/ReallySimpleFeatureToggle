using System;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Configuration.AppConfigProvider;

namespace ReallySimpleFeatureToggle.Test.Unit.Configuration.AppConfigProvider
{
    [TestFixture]
    public class FeatureConfigurationElementTests
    {
        private FeatureConfigurationElement _element;

        [SetUp]
        public void SetUp()
        {
            _element = new FeatureConfigurationElement();
        }

        [Test]
        public void Dependencies_SetGet_DoesntThrow()
        {
            _element.Dependencies = new[] {"a"};

            Assert.That(_element.Dependencies[0], Is.EqualTo("a"));
        }

        [Test]
        public void EndDtg_SetGet_DoesntThrow()
        {
            var date = DateTime.Now;

            _element.EndDtg = date;

            Assert.That(_element.EndDtg, Is.EqualTo(date));
        }

        [Test]
        public void StartDtg_SetGet_DoesntThrow()
        {
            var date = DateTime.Now;

            _element.StartDtg = date;

            Assert.That(_element.StartDtg, Is.EqualTo(date));
        }

        [Test]
        public void SupportedTenants_SetGet_DoesntThrow()
        {
            _element.SupportedTenants = new[]{"abc"};

            Assert.That(_element.SupportedTenants[0], Is.EqualTo("abc"));
        }

        [Test]
        public void ExcludedTenants_SetGet_DoesntThrow()
        {
            _element.ExcludedTenants = new[]{"abc"};

            Assert.That(_element.ExcludedTenants[0], Is.EqualTo("abc"));
        }

        [Test]
        public void Name_SetGet_DoesntThrow()
        {
            _element.Name = "aha";

            Assert.That(_element.Name, Is.EqualTo("aha"));
        }

        [Test]
        public void RandomPercentageEnabled_SetGet_DoesntThrow()
        {
            _element.RandomPercentageEnabled = 50;

            Assert.That(_element.RandomPercentageEnabled, Is.EqualTo(50));
        }

        [Test]
        public void State_SetGet_DoesntThrow()
        {
            _element.State = State.Enabled;

            Assert.That(_element.State, Is.EqualTo(State.Enabled));
        }
    }
}
