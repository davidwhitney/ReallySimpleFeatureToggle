using System.IO;
using System.Runtime.Serialization.Json;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
#if NET45
    public class DataContractSerializer : ICookieDataSerializer
    {
        private readonly DataContractJsonSerializerSettings _settings;

        public DataContractSerializer()
        {
            _settings = new DataContractJsonSerializerSettings { UseSimpleDictionaryFormat = true };
        }

        public string SerializeObject(object obj)
        {
            using (var stream1 = new MemoryStream())
            {
                var contractJsonSerializer = new DataContractJsonSerializer(obj.GetType(), _settings);
                contractJsonSerializer.WriteObject(stream1, obj);
                stream1.Position = 0;

                using (var reader = new StreamReader(stream1))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public T DeserializeObject<T>(string value) where T : class
        {
            using (var stream = new MemoryStream())
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(value);
                sw.Flush();

                stream.Position = 0;
                
                var contractJsonSerializer = new DataContractJsonSerializer(typeof(T), _settings);
                return (T)contractJsonSerializer.ReadObject(stream);
            }
        }
    }
#endif
}