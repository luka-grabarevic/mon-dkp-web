using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace MonDKP.Lib
{
    public static class XmlSerializerHelper
    {
        public static T Deserialize<T>(string xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString))
                throw new SerializationException("XML string cannot be null or empty!");

            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var stringReader = new StringReader(xmlString))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    using (var xmlDictReader = XmlDictionaryReader.CreateDictionaryReader(xmlReader))
                    {
                        return (T) xmlSerializer.Deserialize(xmlDictReader);
                    }
                }
            }
        }
    }
}