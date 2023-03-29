namespace VaporStore.DataProcessor.ImportDto
{ 
    using Newtonsoft.Json;

    public class GameImportDto
    {
        [JsonProperty("Name")]
        public string Name { get; set; } = null!;

        [JsonProperty("Price")]
        public decimal Price { get; set; }

        [JsonProperty("ReleaseDate")]
        public string ReleaseDate { get; set; } = null!;
        [JsonProperty("Developer")]
        public string Developer { get; set; } = null!;

        [JsonProperty("Genre")]
        public string Genre { get; set; } = null!;

        [JsonProperty("Tags")]
        public string[] Tags { get; set; } = null!;
    } 
}
