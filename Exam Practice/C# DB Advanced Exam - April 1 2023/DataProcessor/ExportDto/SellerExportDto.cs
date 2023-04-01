namespace Boardgames.DataProcessor.ExportDto;

using Newtonsoft.Json;

public class SellerExportDto
{
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [JsonProperty("Website")]
    public string Website { get; set; }

    [JsonProperty("Boardgames")]
    public BoardgameJsonExport[] BoardgameJsonExports { get; set; }
}

public class BoardgameJsonExport
{
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [JsonProperty("Rating")]
    public double Rating { get; set; }

    [JsonProperty("Mechanics")]
    public string Mechanics { get; set; } = null!;

    [JsonProperty("Category")]
    public string Category { get; set; } = null!;
}