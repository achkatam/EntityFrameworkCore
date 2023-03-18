namespace ProductShop.Utilities;

using System.Xml.Serialization;
public class XmlHelper
{
    public  T Deserializer<T>(string inputXml, string rootName)
    {
        XmlRootAttribute root = new XmlRootAttribute(rootName);
        XmlSerializer serializer = new XmlSerializer(typeof(T), root);

        using StringReader reader = new StringReader(inputXml);

        T dtos = (T)serializer.Deserialize(reader);

        return dtos;
    } 
}