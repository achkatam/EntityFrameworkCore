namespace CarDealer;

using AutoMapper;

using Models;
using Utilities;
using DTOs.Import;

using Data;

public class StartUp
{
    public static void Main()
    {
        CarDealerContext dbContext = new CarDealerContext();

        // 10. Import Suppliers
        string inputXml = File.ReadAllText("../../../Datasets/suppliers.xml");
        string result = ImportSuppliers(dbContext, inputXml);

        Console.WriteLine(result);
    }

    public static string ImportSuppliers(CarDealerContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();

        XmlHelper xmlHelper = new XmlHelper();

        ImportSupplierDto[] supplierDtos =
            xmlHelper.Deserialize<ImportSupplierDto[]>(inputXml, "Suppliers");

        ICollection<Supplier> validSuppliers = new HashSet<Supplier>();

        foreach (var supplierDto in supplierDtos)
        {
            if (String.IsNullOrEmpty(supplierDto.Name))
            {
                continue;
            }

            // Manual mapping without AutoMappe
            //Supplier supplier = new Supplier()
            //{
            //    Name = supplierDto.Name,
            //    IsImporter = supplierDto.IsImporter
            //};

            Supplier supplier = mapper.Map<Supplier>(supplierDto);

            validSuppliers.Add(supplier);
        }

        context.Suppliers.AddRange(validSuppliers);
        context.SaveChanges();

        return $"Successfully imported {validSuppliers.Count}";
    }

    private static IMapper CreateMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
}