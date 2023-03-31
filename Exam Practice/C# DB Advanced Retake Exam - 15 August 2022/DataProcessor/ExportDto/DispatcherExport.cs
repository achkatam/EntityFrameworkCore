namespace Trucks.DataProcessor.ExportDto
{
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class DispatcherExport
    {
        [XmlAttribute("TrucksCount")]
        public int TrucksCount { get; set; }

        [XmlElement("DespatcherName")]
        public string DispatcherName { get; set; }

        [XmlArray("Trucks")]
        public TruckExport[] Trucks { get; set; }
    }

    [XmlType("Truck")]
    public class TruckExport
    {
        [XmlElement("RegistrationNumber")]
        public string RegistrationNumber { get; set; } = null!;

        [XmlElement("Make")] 
        public string MakeType { get; set; } = null!;
    }
}
