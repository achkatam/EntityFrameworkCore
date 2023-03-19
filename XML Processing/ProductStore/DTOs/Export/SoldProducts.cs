namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

public class SoldProducts
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("products")]
    public ProductDto[] Products { get; set; }
}