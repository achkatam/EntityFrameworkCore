namespace Theatre.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using Data.Utilities;
using Newtonsoft.Json;

 
public class ImportTheatreDto
{
    [StringLength(ValidationConstants.THEATER_NAME_MAX, MinimumLength = ValidationConstants.FULLNAME_NAME_MIN)]
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [Range(1, 10)]
    [JsonProperty("NumberOfHalls")]
    public sbyte NumberOfHalls { get; set; }

    [StringLength(ValidationConstants.DIRECTOR_MAX, MinimumLength = ValidationConstants.DIRECTOR_MIN)]
    [JsonProperty("Director")]
    public string Director { get; set; } = null!;

    [JsonProperty("Tickets")]
    public ImportTicketDto[] Tickets { get; set; } = null!;
}