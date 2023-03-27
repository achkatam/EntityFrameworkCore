namespace Theatre.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Data.Utilities;

[XmlType("Cast")]
public class ImportCastDto
{
    [StringLength(ValidationConstants.FULLNAME_NAME_MAX, MinimumLength = ValidationConstants.FULLNAME_NAME_MIN)]
    [XmlElement("FullName")]
    public string FullName { get; set; } = null!;

    [XmlElement("IsMainCharacter")]
    public bool IsMainCharacter { get; set; } 

    [RegularExpression(ValidationConstants.REGEX_PATTERN)]
    [XmlElement("PhoneNumber")] 
    public string PhoneNumber { get; set; } = null!;

    [XmlElement("PlayId")]
    public int PlayId { get; set; }  
}