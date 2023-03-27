namespace Theatre.DataProcessor.ImportDto;

using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

public class ImportTicketDto
{
    [Range(1.00, 100.00)]
    [JsonProperty("Price")]
    public decimal Price { get; set; }

    [Range(1, 10)]  
    [JsonProperty("RowNumber")]
    public sbyte RowNumber { get; set; }

    [JsonProperty("PlayId")]
    public int PlayId { get; set; }
}