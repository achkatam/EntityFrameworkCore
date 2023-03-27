namespace Theatre.DataProcessor.ExportDto;

using System.Xml.Serialization;

[XmlType("Play")]
public class ExportPlayDto
{
    [XmlAttribute("Title")] 
    public string Title { get; set; } = null!;

    [XmlAttribute("Duration")]
    public string Duration { get; set; } = null!;

    [XmlAttribute("Rating")] 
    public string Rating { get; set; } = null!;

    [XmlAttribute("Genre")]
    public string Genre { get; set; } = null!;

    [XmlArray("Actors")]
    public ExportCastDto[] Actors { get; set; } = null!;
}

[XmlType("Actor")]
public class ExportCastDto
{
    [XmlAttribute("FullName")]
    public string FullName { get; set; } = null!;

    [XmlAttribute("MainCharacter")] 
    public string MainCharacter { get; set; } = null!;
}