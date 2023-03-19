namespace CarDealer.DTOs.Import;

using System.Xml.Serialization;

[XmlType("Car")]
public class ImportCarDto
{
    [XmlElement("make")]
    public string Make { get; set; } = null!;

    [XmlElement("model")]
    public string Model { get; set; } = null!;

    [XmlElement("traveledDistance")]
    public int TraveledDistance { get; set; }

    [XmlArray("parts")] public ImportPartCarDto[] Parts { get; set; } = null!;
}