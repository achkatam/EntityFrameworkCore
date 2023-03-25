namespace Footballers.Data.Utilities
{
    using System.Text;
    using System.Xml.Serialization;

    public class XmlHelper
    {
        public T Deserializer<T>(string inputXml, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            using StringReader reader = new StringReader(inputXml);

            T dtos = (T)serializer.Deserialize(reader);

            return dtos;
        }

        public string Serialize<T>(T dataTransferObjects, string xmlRootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttributeName));

            var builder = new StringBuilder();

            using var write = new StringWriter(builder);
            serializer.Serialize(write, dataTransferObjects, GetXmlNamespaces());

            return builder.ToString();
        }

        public string Serialize<T>(T[] dataTransferObjects, string xmlRootAttributeName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T[]), new XmlRootAttribute(xmlRootAttributeName));

            var builder = new StringBuilder();

            using var writer = new StringWriter(builder);
            serializer.Serialize(writer, dataTransferObjects, GetXmlNamespaces());

            return builder.ToString();
        }

        private XmlSerializerNamespaces GetXmlNamespaces()
        {
            XmlSerializerNamespaces xmlNamespaces = new XmlSerializerNamespaces();
            xmlNamespaces.Add(string.Empty, string.Empty);
            return xmlNamespaces;
        }
    }
}
