namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Data.Utilities;

    [XmlType("Purchase")]
    public class PurchaseImportDto
    {
        [XmlAttribute("title")]
        public string GameTitle { get; set; } = null!;

        [XmlElement("Type")] 
        public string Type { get; set; } = null!;

        [RegularExpression(ValidationConstants.PURCHASE_REGEX)]
        [XmlElement("Key")]
        public string Key { get; set; } = null!;

        [XmlElement("Card")]
        public string Card { get; set; } = null!;

        [XmlElement("Date")]
        public string Date { get; set; } = null!;
    }
}
