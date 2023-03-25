namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Data.Utilities;

    [XmlType("Coach")]
    public class ImportCoachXmlDto
    {
        [MinLength(ValidationConstants.COACH_NAME_MIN)]
        [MaxLength(ValidationConstants.COACH_NAME_MAX)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [MinLength(ValidationConstants.NATIONALITY_NAME_MIN)]
        [MaxLength(ValidationConstants.NATIONALITY_NAME_MAX)]
        [XmlElement("Nationality")]
        public string Nationality { get; set; } = null!;

        public ImportFootballerDto[] Footballers { get; set; } = null!; 
    }

    [XmlType("Footballer")]
    public class ImportFootballerDto
    {
        [MinLength(ValidationConstants.FOOTBALLER_NAME_MIN)]
        [MaxLength(ValidationConstants.FOOTBALLER_NAME_MAX)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("ContractStartDate")]
        public string ContractStartDate { get; set; } = null!;


        [XmlElement("ContractEndDate")]
        public string ContractEndDate { get; set; } = null!;

        [XmlElement("BestSkillType")]
        public int BestSkillType { get; set; }

        [XmlElement("PositionType")]
        public int PositionType { get; set; }
    }
}
