namespace Theatre.DataProcessor.ExportDto;

using Newtonsoft.Json;

public class ExportTheaterDto
{
    [JsonProperty("Name")]
    public string Name { get; set; } = null!;

    [JsonProperty("Halls")]
    public sbyte NumberOfHalls { get; set; }

    [JsonProperty("TotalIncome")]       
    public decimal TotalIncome { get; set; }

    public ExportTicketDto[] Tickets { get; set; } = null!;
}

public class ExportTicketDto
{
    [JsonProperty("Price")]
    public decimal Price { get; set; }

    [JsonProperty("RowNumber")]
    public sbyte RowNumber { get; set; }
}