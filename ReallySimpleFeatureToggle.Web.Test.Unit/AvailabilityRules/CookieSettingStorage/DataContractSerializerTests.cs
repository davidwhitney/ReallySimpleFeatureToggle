using System.Collections.Generic;
using NUnit.Framework;
using ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage;

namespace ReallySimpleFeatureToggle.Web.Test.Unit.AvailabilityRules.CookieSettingStorage
{
    [TestFixture]
    public class DataContractSerializerTests
    {
        private FeatureOptionsCookie _dto;
        private DataContractSerializer _ser;
        private string _dtoAsJson;

        [SetUp]
        public void SetUp()
        {
            _dto = new FeatureOptionsCookie
            {
                FeatureStates = new Dictionary<string, bool> {{"Feature", true}, {"Feature2", false}}
            };
            _dtoAsJson = "{\"featureStates\":{\"Feature\":true,\"Feature2\":false}}";

            _ser = new DataContractSerializer();
        }

        [Test]
        public void SerializeObject_ValidDto_Serializes()
        {
            var output = _ser.SerializeObject(_dto);

            Assert.That(output, Is.EqualTo(_dtoAsJson));
        }

        [Test]
        public void DeserializeObject_ValidString_Deserializes()
        {
            var output = _ser.DeserializeObject<FeatureOptionsCookie>(_dtoAsJson);

            Assert.That(output.FeatureStates.Count, Is.EqualTo(2));
            Assert.That(output.FeatureStates["Feature"], Is.True);
            Assert.That(output.FeatureStates["Feature2"], Is.False);
        }
    }
}
