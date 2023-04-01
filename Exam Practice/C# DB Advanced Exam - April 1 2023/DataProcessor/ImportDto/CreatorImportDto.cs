namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using System.Xml.Serialization;
    using Data.Utilities;

    [XmlType("Creator")]
    public class CreatorImportDto
    {
        [MinLength(ValidationConstants.FIRST_NAME_MIN)]
        [MaxLength(ValidationConstants.FIRST_NAME_MAX)]
        [XmlElement("FirstName")]
        public string FirstName { get; set; } = null!;

        [MinLength(ValidationConstants.LAST_NAME_MIN)]
        [MaxLength(ValidationConstants.LAST_NAME_MAX)]
        [XmlElement("LastName")]  
        public string LastName { get; set; } = null!;

        [XmlArray("Boardgames")]
        public BoardgameImportDto[] Boardgames { get; set; }
    }

    [XmlType("Boardgame")]
    public class BoardgameImportDto
    {
        [MinLength(ValidationConstants.BOARDGAME_NAME_MIN)]
        [MaxLength(ValidationConstants.BOARDGAME_NAME_MAX)]
        [XmlElement("Name")]
        public string Name { get; set; } = null!;
        
        [Range(ValidationConstants.RATING_MIN, ValidationConstants.RATING_MAX)]
        [XmlElement("Rating")]
        public double Rating { get; set; }
        
        [Range(ValidationConstants.YEAR_MIN, ValidationConstants.YEAR_MAX)]
        [XmlElement("YearPublished")]
        public int YearPublished { get; set; }
        
        [XmlElement("CategoryType")]
        public string CategoryType { get; set; } = null!;

        [XmlElement("Mechanics")]
        public string Mechanics { get; set; } = null!;
    }
}
