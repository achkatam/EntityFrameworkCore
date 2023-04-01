namespace Boardgames.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Data.Utilities;
    using Newtonsoft.Json;

    public class SellerImportDto
    {
        [Required]
        [MinLength(ValidationConstants.SELLER_NAME_MIN)]
        [MaxLength(ValidationConstants.SELLER_NAME_MAX)]
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;


        [Required]
        [MinLength(ValidationConstants.ADDRESS_MIN)]
        [MaxLength(ValidationConstants.ADDRESS_MAX)]
        [JsonProperty("Address")]
        public string Address { get; set; } = null!;

        [Required]
        [JsonProperty("Country")]
        public string Country { get; set; } = null!;

        [Required]
        [RegularExpression(ValidationConstants.REGEX_PATTERN)]
        [JsonProperty("Website")]
        public string Website { get; set; } = null!;

        [Required]
        [JsonProperty("Boardgames")]
        public int[] Boardgames { get; set; } = null!;
    }
}