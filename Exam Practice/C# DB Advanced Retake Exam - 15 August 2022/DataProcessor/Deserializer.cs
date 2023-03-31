namespace Trucks.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.IO.Pipes;
    using System.Text;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Data.Models.Utilities;
    using ImportDto;
    using Newtonsoft.Json;


    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedDespatcher
            = "Successfully imported despatcher - {0} with {1} trucks.";

        private const string SuccessfullyImportedClient
            = "Successfully imported client - {0} with {1} trucks.";

        public static string ImportDespatcher(TrucksContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            var sb = new StringBuilder();

            var dispatherDtos = xmlHelper.Deserializer<DispatcherImportDto[]>(xmlString, "Despatchers");

            var dispatchers = new HashSet<Despatcher>();

            foreach (var dto in dispatherDtos)
            {
                if (!IsValid(dto)
                    || string.IsNullOrWhiteSpace(dto.Position))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                var dispatcher = new Despatcher()
                {
                    Name = dto.Name,
                    Position = dto.Position
                };

                foreach (var truckDto in dto.Trucks)
                {

                    bool isValidCategory = Enum.TryParse<CategoryType>(truckDto.CategoryType, out CategoryType categoryType);

                    bool isValidMake = Enum.TryParse<MakeType>(truckDto.MakeType, out MakeType makeType);

                    if (!IsValid(truckDto)
                        || !isValidCategory
                        || !isValidMake
                        || string.IsNullOrWhiteSpace(truckDto.VinNumber))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    var truck = new Truck()
                    {
                        RegistrationNumber = truckDto.RegistrationNumber,
                        VinNumber = truckDto.VinNumber,
                        TankCapacity = truckDto.TankCapacity,
                        CargoCapacity = truckDto.CargoCapacity,
                        CategoryType = categoryType,
                        MakeType = makeType
                    };

                    dispatcher.Trucks.Add(truck);
                }

                dispatchers.Add(dispatcher);

                sb.AppendLine(string.Format(SuccessfullyImportedDespatcher, dispatcher.Name, dispatcher.Trucks.Count));
            }
            context.Despatchers.AddRange(dispatchers);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }
        public static string ImportClient(TrucksContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var clientDtos = JsonConvert.DeserializeObject<ClientImportDto[]>(jsonString);

            var clients = new HashSet<Client>();

            foreach (var clientDto in clientDtos)
            {
                if (!IsValid(clientDto)
                    || clientDto.Type == "usual")
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Client client = new Client()
                {
                    Name = clientDto.Name,
                    Nationality = clientDto.Nationality,
                    Type = clientDto.Type
                };

                foreach (var truckId in clientDto.Trucks.Distinct())
                {
                    if (!IsValid(truckId)
                        || !context.Trucks.Any(x => x.Id == truckId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ClientTruck clientTruck = new ClientTruck()
                    {
                        Client = client,
                        TruckId = truckId
                    };

                    client.ClientsTrucks.Add(clientTruck);
                }

                clients.Add(client);
                sb.AppendLine(string.Format(SuccessfullyImportedClient, client.Name, client.ClientsTrucks.Count));
            }

            context.Clients.AddRange(clients);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}