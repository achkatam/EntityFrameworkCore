namespace VaporStore.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Game")]
    public class GameExportDto
    {
        [XmlAttribute("title")]
        public string GameTitle { get; set; } = null!;

        [XmlElement("Genre")]
        public string Genre { get; set; } = null!;

        [XmlElement("Price")]
        public decimal Price { get; set; }
    }
}
