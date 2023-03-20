namespace CarDealer;

using DTOs.Import.Sales;
using DTOs.Import.Customer;
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

        string filePath = File.ReadAllText("../../../Datasets/sales.xml");
        string result = ImportSales(dbContext, filePath);

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
        // dbContext.SaveChanges();

        return $"Successfully imported {parts.Count}";
    }

    public static string? ImportCars(CarDealerContext dbContext, string inputXml)
    {
        return null;
    }

    public static string ImportCustomers(CarDealerContext dbContext, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var customerDto = xmlHelper.Deserializer<ImportCustomerDto[]>(inputXml, "Customers");

        var customers = new HashSet<Customer>();

        foreach (var dto in customerDto)
        {
            Customer customer = mapper.Map<Customer>(dto);

            customers.Add(customer);
        }

        dbContext.Customers.AddRange(customers);
        // dbContext.SaveChanges();

        return $"Successfully imported {customers.Count()}";
    }

    public static string ImportSales(CarDealerContext dbContext, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var saleDto = xmlHelper.Deserializer<ImportSaleDto[]>(inputXml, "Sales");

        var sales = new HashSet<Sale>();

        foreach (var dto in saleDto)
        {
            if (!dbContext.Cars.Any(c => c.Id == dto.CarId))
                continue;

            Sale sale = new Sale()
            {
                CarId = dto.CarId,
                CustomerId = dto.CustomerId,
                Discount = dto.Discount
            };

            sales.Add(sale);
        }

        dbContext.Sales.AddRange(sales);
        dbContext.SaveChanges();

        return $"Successfully imported {sales.Count}";
    }

    private static IMapper CreateMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
}