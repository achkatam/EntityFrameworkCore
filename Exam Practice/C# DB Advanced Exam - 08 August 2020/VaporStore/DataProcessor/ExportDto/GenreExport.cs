namespace VaporStore.DataProcessor.ExportDto
{
    using Newtonsoft.Json;

    public class GenreExport
    {
        [JsonProperty("Id")]
        public int Id { get; set; }

        [JsonProperty("Genre")]
        public string Genre { get; set; } = null!;

        [JsonProperty("Games")]
        public GameJsonExportDto[] Games { get; set; } = null!;

        [JsonProperty("TotalPlayers")]
        public int TotalPlayers { get; set; }
    }
}
