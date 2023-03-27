namespace Theatre.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using Data.Utilities;

[XmlType("Play")]
public class ImportPlayDto
{
    [StringLength(ValidationConstants.PLAY_TITLE_MAX, MinimumLength = ValidationConstants.PLAY_TITLE_MIN)]
    [XmlElement("Title")]
    public string Title { get; set; } = null!;

    [XmlElement("Duration")]
    public string Duration { get; set; } = null!;

    [Required]
    [Range(0.00, 10.00)]
    [XmlElement("Raiting")]
    public float Rating { get; set; }

    [XmlElement("Genre")]
    public string Genre { get; set; } = null!;

    [MaxLength(ValidationConstants.DESCRIPTION_MAX)]
    [XmlElement("Description")]
    public string Description { get; set; } = null!;

    [StringLength(ValidationConstants.SCREENWRITER_MAX, MinimumLength = ValidationConstants.SCREENWRITER_MIN)]
    [XmlElement("Screenwriter")]
    public string Screenwriter { get; set; } = null!;
}