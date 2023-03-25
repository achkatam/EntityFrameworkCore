namespace Footballers.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Data.Utilities;
    using Newtonsoft.Json;


    public class ImportTeamJsonDto
    {
        [RegularExpression(ValidationConstants.REGEX_PATTERN)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [MinLength(ValidationConstants.NATIONALITY_NAME_MIN)]
        [MaxLength(ValidationConstants.NATIONALITY_NAME_MAX)]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Trophies")]
        public int Trophies { get; set; }


        [JsonProperty("Footballers")]
        public int[] FootballersIds { get; set; }
    }
}
