﻿using System;
using System.Collections.Generic;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage;

namespace ReallySimpleFeatureToggle.Web.Test.Unit.AvailabilityRules.CookieSettingStorage
{
    [TestFixture]
    public class DataContractConcatSerializerTests
    {
        private FeatureOptionsCookie _dto;
        private DataContractConcatSerializer _ser;
        private string _dtoAsRawCookie;

        [SetUp]
        public void SetUp()
        {
            _dto = new FeatureOptionsCookie
            {
                FeatureStates = new Dictionary<string, bool> {{"Feature", true}, {"Feature2", false}}
            };
            _dtoAsRawCookie = "Feature=True&Feature2=False";

            _ser = new DataContractConcatSerializer();
        }

        [Test]
        public void SerializeObject_NotAFeatureOptionsCookie_ThrowsArgEx()
        {
            var dto = new Dictionary<string,string>();

            var ex = Assert.Throws<ArgumentException>(() => _ser.SerializeObject(dto));

            Assert.That(ex.Message, Is.EqualTo("Concat serializer only supports serializing the FeatureOptionsCookie"));
        }

        [Test]
        public void DeserializeObject_TisNotFeatureOptionsCookie_ThrowsArgEx()
        {
            var ex = Assert.Throws<ArgumentException>(() => _ser.DeserializeObject<Dictionary<string, string>>("abc"));

            Assert.That(ex.Message, Is.EqualTo("Concat serializer only supports serializing the FeatureOptionsCookie"));
        }

        [Test]
        public void SerializeObject_ValidDto_Serializes()
        {
            var output = _ser.SerializeObject(_dto);

            Assert.That(output, Is.EqualTo(_dtoAsRawCookie));
        }

        [Test]
        public void DeserializeObject_ValidString_Deserializes()
        {
            var output = _ser.DeserializeObject<FeatureOptionsCookie>(_dtoAsRawCookie);

            Assert.That(output.FeatureStates.Count, Is.EqualTo(2));
            Assert.That(output.FeatureStates["Feature"], Is.True);
            Assert.That(output.FeatureStates["Feature2"], Is.False);
        }
    }
}
