namespace Boardgames.DataProcessor.ExportDto;

using System.Xml.Serialization;

[XmlType("Creator")]
public class CreatorExportDto
{
    [XmlAttribute("BoardgamesCount")]
    public int Count { get; set; }

    [XmlElement("CreatorName")]
    public string CreatorName { get; set; } = null!;

    [XmlArray("Boardgames")]
    public BoardgameExportDto[] BoardgameExportDtos { get; set; }
}

[XmlType("Boardgame")]
public class BoardgameExportDto
{
    [XmlElement("BoardgameName")]
    public string BoardgameName { get; set; } = null!;


    [XmlElement("BoardgameYearPublished")]
    public int PublishedYear { get; set; }
}