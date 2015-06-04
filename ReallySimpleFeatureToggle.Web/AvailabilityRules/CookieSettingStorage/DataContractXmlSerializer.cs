using System.IO;

namespace ReallySimpleFeatureToggle.Web.AvailabilityRules.CookieSettingStorage
{
    public class DataContractXmlSerializer : ICookieDataSerializer
    {
        public string SerializeObject(object obj)
        {
            using (var stream1 = new MemoryStream())
            {
                var contractJsonSerializer = new System.Runtime.Serialization.DataContractSerializer(obj.GetType());
                contractJsonSerializer.WriteObject(stream1, obj);
                stream1.Position = 0;

                using (var reader = new StreamReader(stream1))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public T DeserializeObject<T>(string value)
        {
            using (var stream = new MemoryStream())
            using (var sw = new StreamWriter(stream))
            {
                sw.Write(value);
                sw.Flush();

                stream.Position = 0;

                var contractJsonSerializer = new System.Runtime.Serialization.DataContractSerializer(typeof(T));
                return (T)contractJsonSerializer.ReadObject(stream);
            }
        }
    }
}