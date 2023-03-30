namespace VaporStore.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Purchase")]
    public class PurchaseExportDto
    {
        [XmlElement("Card")]
        public string CardNumber { get; set; } = null!;

        [XmlElement("CVC")]
        public string Cvc { get; set; } = null!;

        [XmlElement("Date")]
        public string Date { get; set; } = null!;

        [XmlElement("Game")]
        public GameExportDto Games { get; set; } = null!;
    }
}
