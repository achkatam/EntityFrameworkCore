namespace ProductShop.DTOs.Export;

using System.Xml.Serialization;

[XmlType("Users")]
public class ExportUserAndProductsDto
{
    [XmlElement("count")]
    public int Count { get; set; }

    [XmlArray("users")]
    public UserDto[] Users { get; set; }
}