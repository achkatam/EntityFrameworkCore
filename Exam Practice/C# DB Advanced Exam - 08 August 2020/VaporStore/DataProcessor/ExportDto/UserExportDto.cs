namespace VaporStore.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("User")]
    public class UserExportDto
    {
        [XmlAttribute("username")]
        public string Username { get; set; } = null!;

        [XmlArray("Purchases")]
        public PurchaseExportDto[] Purchases { get; set; } = null!;

        [XmlElement("TotalSpent")] 
        public decimal TotalSpent { get; set; }
    }
}
