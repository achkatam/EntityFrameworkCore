namespace Trucks.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    public class ClientImportDto
    {
        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [MinLength(2)]
        [MaxLength(40)]
        [Required]
        [JsonProperty("Nationality")]
        public string Nationality { get; set; } = null!;

        [JsonProperty("Type")]
        [Required]
        public string Type { get; set; } = null!;


        [Required]
        [JsonProperty("Trucks")] 
        public int[] Trucks { get; set; } = null!;
    }
}
