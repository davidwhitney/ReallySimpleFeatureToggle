﻿using System;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Configuration;
using ReallySimpleFeatureToggle.Test.Unit.TestHelpers;

namespace ReallySimpleFeatureToggle.Test.Unit.Configuration
{
    [TestFixture]
    class FeatureTests
    {
        [Test]
        public void IsAvailable_StartAfterEndDate_DoesNotThrowException()
        {
            Assert.DoesNotThrow(
                () =>
                    new Feature(TestFeatures.TestFeature1)
                    {
                        StartDtg = DateTime.Now.AddDays(1),
                        EndDtg = DateTime.Now.AddDays(2),
                    });
        }

        [Test]
        public void Ctor_WithEnum_CreatesFeature()
        {
            var feature = new Feature(FeatureForTest.MyFeature);

            Assert.That(feature.Name, Is.EqualTo("MyFeature"));
        }

        public enum FeatureForTest
        {
            MyFeature
        }
    }
}
