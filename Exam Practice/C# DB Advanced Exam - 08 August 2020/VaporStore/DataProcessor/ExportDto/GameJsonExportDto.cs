namespace VaporStore.DataProcessor.ExportDto
{
    using Newtonsoft.Json;

    public class GameJsonExportDto
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; } = null!;

        [JsonProperty("Developer")]
        public string Developer { get; set; } = null!;

        [JsonProperty("Tags")]
        public string Tags { get; set; } = null!;

        [JsonProperty("Players")]
        public int Players { get; set; }
    }
}
