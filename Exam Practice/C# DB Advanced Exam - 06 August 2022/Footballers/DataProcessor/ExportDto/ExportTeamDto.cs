namespace Footballers.DataProcessor.ExportDto
{
    using Newtonsoft.Json;

    public class ExportTeamDto
    {
        [JsonProperty("Name")]  
        public string Name { get; set; } = null!;

        [JsonProperty("Footballers")]
        public ExportFootballerDto[] Footballers { get; set; } = null!;
    }

    public class ExportFootballerDto
    {
        [JsonProperty("FootballerName")]
        public string FootballerName { get; set; } = null!;

        [JsonProperty("ContractStartDate")]
        public string ContractStartDate { get; set; } = null!;

        [JsonProperty("ContractEndDate")]
        public string ContractEndDate { get; set; } = null!;

        [JsonProperty("BestSkillType")]     
        public string BestSkillType { get; set; } = null!;


        [JsonProperty("PositionType")]
        public string PositionType { get; set; } = null!;
    }
}
