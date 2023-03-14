﻿namespace CarDealer.DTOs.Import;

using Newtonsoft.Json;

[JsonObject]
public class ImportSupplierDTO
{
    [JsonProperty("name")]
    public string Name { get; set; } = null!;

    [JsonProperty("isImporter")]
    public bool IsImporter { get; set; }
}
