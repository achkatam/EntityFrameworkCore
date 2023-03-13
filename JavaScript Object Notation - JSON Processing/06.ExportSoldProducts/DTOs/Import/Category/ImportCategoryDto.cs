namespace ProductShop.DTOs.Import.Category;

using Newtonsoft.Json;

[JsonObject]
public class ImportCategoryDto
{
    [JsonProperty("name")]
    public string? Name { get; set; }

}
