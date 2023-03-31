namespace Trucks.DataProcessor
{
    using Data;
    using Data.Models.Enums;
    using Data.Models.Utilities;
    using ExportDto;
    using Newtonsoft.Json;

    public class Serializer
    {
        public static string ExportDespatchersWithTheirTrucks(TrucksContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var dispatchers = context.Despatchers
                .Where(d => d.Trucks.Any())
                .ToArray()
                .Select(d => new DispatcherExport()
                {
                    TrucksCount = d.Trucks.Count,
                    DispatcherName = d.Name,
                    Trucks = d.Trucks
                        .Select(t => new TruckExport()
                        {
                            RegistrationNumber = t.RegistrationNumber,
                            MakeType = t.MakeType.ToString()
                        })
                        .OrderBy(t => t.RegistrationNumber)
                        .ToArray()
                })
                .OrderByDescending(d => d.TrucksCount)
                .ThenBy(d => d.DispatcherName)
                .ToArray();

            return xmlHelper.Serialize<DispatcherExport[]>(dispatchers, "Despatchers");
        }

        public static string ExportClientsWithMostTrucks(TrucksContext context, int capacity)
        {
            var clients = context.Clients
                .Where(c => c.ClientsTrucks.Any(t => t.Truck.TankCapacity >= capacity))
                .ToArray()
                .Select(c => new ClientExportDto()
                {
                    Name = c.Name,
                    Trucks = c.ClientsTrucks
                        .Where(c => c.Truck.TankCapacity >= capacity)
                        .Select(t => new TruckExportDto()
                        {
                            RegistrationNumber = t.Truck.RegistrationNumber,
                            VinNumber = t.Truck.VinNumber,
                            TankCapacity = t.Truck.TankCapacity,
                            CargoCapacity = t.Truck.CargoCapacity,
                            CategoryType = t.Truck.CategoryType.ToString(),
                            MakeType = t.Truck.MakeType.ToString(),
                        })
                        .OrderBy(t => t.MakeType.ToString())
                        .ThenByDescending(t => t.CargoCapacity)
                        .ToArray()
                })
                .OrderByDescending(c => c.Trucks.Count())
                .ThenBy(c => c.Name)
                .Take(10)
                .ToArray();
            

            return JsonConvert.SerializeObject(clients, Formatting.Indented);
        }
    }
}
