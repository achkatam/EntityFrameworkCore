namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;

    [XmlType("Despatcher")]
    public class DispatcherImportDto
    {
        [MinLength(2)]
        [MaxLength(40)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Position")]
        public string Position { get; set; }

        [XmlArray("Trucks")]
        public TruckImportDto[] Trucks { get; set; }
    }

    [XmlType("Truck")]
    public class TruckImportDto
    {
        [RegularExpression(@"^[A-Z]{2}[0-9]{4}[A-Z]{2}$")]
        [XmlElement("RegistrationNumber")]
        [Required]
        public string RegistrationNumber { get; set; } = null!;

        [StringLength(17)]
        [XmlElement("VinNumber")]
        [Required]
        public string VinNumber { get; set; } = null!;

        [Range(950, 1420)]
        [XmlElement("TankCapacity")]
        public int TankCapacity { get; set; }

        [Range(5000, 29_000)]

        [XmlElement("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [XmlElement("CategoryType")]
        [Required]
        public string CategoryType { get; set; } = null!;

        [Required]
        [XmlElement("MakeType")]
        public string MakeType { get; set; } = null!;
    }
}