namespace ProductShop.DTOs.Import.User
{
    using Newtonsoft.Json;

    [JsonObject]
    public class UserDTO
    {
        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("age")]
        public int Age { get; set; }
    }
}
