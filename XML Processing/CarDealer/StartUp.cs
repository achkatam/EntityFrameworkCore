namespace CarDealer;

using DTOs.Import;
using Models;

using Utilities;
using AutoMapper;
using Data;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext dbContext = new CarDealerContext();

        string filePath = File.ReadAllText("../../../Datasets/parts.xml");
        string result = ImportParts(dbContext, filePath);

        Console.WriteLine(result);
    }

    public static string ImportSuppliers(CarDealerContext dbContext, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var suppliersDtos = xmlHelper.Deserializer<ImportSupplierDto[]>(inputXml, "Suppliers");

        var suppliers = suppliersDtos
            .Select(s => new Supplier()
            {
                Name = s.Name,
                IsImporter = s.IsImporter
            })
            .ToList();

        dbContext.Suppliers.AddRange(suppliers);
        // dbContext.SaveChanges();

        return $"Successfully imported {suppliers.Count}";
    }

    public static string ImportParts(CarDealerContext dbContext, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var partDtos = xmlHelper.Deserializer<ImportPartDto[]>(inputXml, "Parts");
        var allSuppliersIds = dbContext.Suppliers.Select(x => x.Id)
            .ToList();

        var parts = partDtos
            .Where(x => allSuppliersIds.Contains(x.SupplierId))
            .Select(p => new Part()
            {
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                SupplierId = p.SupplierId
            })
            .ToList();

        dbContext.Parts.AddRange(parts);
        dbContext.SaveChanges();

        return $"Successfully imported {parts.Count}";
    }

    private static IMapper CreateMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
}