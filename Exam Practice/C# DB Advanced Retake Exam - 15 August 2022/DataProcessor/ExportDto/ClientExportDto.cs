namespace Trucks.DataProcessor.ExportDto
{
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    public class ClientExportDto
    {
        [JsonProperty("Name")]
        
        public string Name { get; set; }  

        [JsonProperty("Trucks")]
        public TruckExportDto[] Trucks { get; set; }
    }

    public class TruckExportDto
    {
        [JsonProperty("TruckRegistrationNumber")]

        public string RegistrationNumber { get; set; } = null!;

        [JsonProperty("VinNumber")] 
        public string VinNumber { get; set; } = null!;
         
        [JsonProperty("TankCapacity")]
        public int TankCapacity { get; set; }
         

        [JsonProperty("CargoCapacity")]
        public int CargoCapacity { get; set; }

        [JsonProperty("CategoryType")]
        public string CategoryType { get; set; } = null!;

         
        [JsonProperty("MakeType")]
        public string MakeType { get; set; } = null!;
    }
}
