namespace VaporStore.DataProcessor.ImportDto
{
    using System.ComponentModel.DataAnnotations;
    using Data.Utilities;
    using Newtonsoft.Json;

    public class UserImportDto
    {
        [RegularExpression(ValidationConstants.FULLNAME_PATTERN)]
        [JsonProperty("FullName")]
        public string FullName { get; set; } = null!;

        [StringLength(ValidationConstants.USERNAME_MAX, MinimumLength = ValidationConstants.USERNAME_MIN)]
        [JsonProperty("Username")]
        public string Username { get; set; } = null!;

        [JsonProperty("Email")]
        public string Email { get; set; } = null!;

        [Range(ValidationConstants.AGE_MIN, ValidationConstants.AGE_MAX)]
        [JsonProperty("Age")]
        public int Age { get; set; }

        [JsonProperty("Cards")]
        public CardImportDto[] Cards { get; set; } = null!;
    }

    public class CardImportDto
    {
        [RegularExpression(ValidationConstants.CARD_REGEX)]
        [JsonProperty("Number")]
        public string Number { get; set; } = null!;

        [RegularExpression(ValidationConstants.CARD_CVC)]
        [JsonProperty("CVC")]
        public string Cvc { get; set; } = null!;

        [JsonProperty("Type")]
        public string Type { get; set; } = null!;
    }
}
