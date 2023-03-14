namespace CarDealer;

using AutoMapper;
using DTOs.Import;
using Models;
using Newtonsoft.Json;
using Data;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext dbContext = new CarDealerContext();

        // 09. Import Suppliers
        // string inputJson = File.ReadAllText(@"../../../Datasets/suppliers.json");
        // string result = ImportSuppliers(dbContext, inputJson);

        // 10. Import parts
        string inputJson = File.ReadAllText(@"../../../Datasets/parts.json");
        string result = ImportParts(dbContext, inputJson);


        Console.WriteLine(result);
    }

    public static string ImportSuppliers(CarDealerContext dbContext, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var suppliersDtos = JsonConvert.DeserializeObject<ImportSupplierDTO[]>(inputJson);

        ICollection<Supplier> suppliers = new HashSet<Supplier>();

        foreach (var suppliersDto in suppliersDtos)
        {
            Supplier supplier = mapper.Map<Supplier>(suppliersDto);

            suppliers.Add(supplier);
        }

        dbContext.Suppliers.AddRange(suppliers);
        // dbContext.SaveChanges();

        return $"Successfully imported {suppliers.Count}.";
    }
    public static string ImportParts(CarDealerContext dbContext, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var partDtos = JsonConvert.DeserializeObject<ImportPartDto[]>(inputJson);

        ICollection<Part> parts = new HashSet<Part>();

        List<int> supplierListIds = dbContext.Suppliers
            .Select(s => s.Id)
            .ToList();

        foreach (var partDto in partDtos)
        {
            if (!supplierListIds.Contains(partDto.SupplierId))
            {
                continue;
            }

            Part part = mapper.Map<Part>(partDto);


            parts.Add(part);
        }

        dbContext.Parts.AddRange(parts);
        dbContext.SaveChanges();

        return $"Successfully imported {parts.Count}.";
    }

    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
    }
}