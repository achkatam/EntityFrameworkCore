namespace CarDealer.Utilities;

using System.Xml.Serialization;



public   class XmlHelper
{
    public T Deserialize<T>(string inputXml, string rootName)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");


        XmlSerializer xmlSerializer =
            new XmlSerializer(typeof(T), xmlRoot);

        StringReader reader = new StringReader(inputXml);

        T suppliersDtos = (T)xmlSerializer.Deserialize(reader);

        return suppliersDtos;
    }

    public IEnumerable<T> DeserializeCollection<T>(string inputXml, string rootName)
    {
        XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");


        XmlSerializer xmlSerializer =
            new XmlSerializer(typeof(T[]), xmlRoot);

        StringReader reader = new StringReader(inputXml);

        T[] suppliersDtos = (T[])xmlSerializer.Deserialize(reader);

        return suppliersDtos;
    }
}