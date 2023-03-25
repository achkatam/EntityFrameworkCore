namespace Footballers.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Coach")]
    public class ExportCoachDto
    {
        [XmlAttribute("FootballersCount")]
        public int Count { get; set; }

        [XmlElement("CoachName")]
        public string CoachName { get; set; } = null!;

        [XmlArray("Footballers")]
        public ExportFootballerXmlDto[] Footballers { get; set; } = null!;
    }

    [XmlType("Footballer")]
    public class ExportFootballerXmlDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;


        [XmlElement("Position")]
        public string Position { get; set; } = null!;
    }
}
