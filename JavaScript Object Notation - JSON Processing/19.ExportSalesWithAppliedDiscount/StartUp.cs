using System.Globalization;
using Microsoft.EntityFrameworkCore;

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
        // string inputJson = File.ReadAllText(@"../../../Datasets/parts.json");
        // string result = ImportParts(dbContext, inputJson);

        // 11. Import Cars
        //string inputJson = File.ReadAllText(@"../../../Datasets/cars.json");
        //string result = ImportCars(dbContext, inputJson);

        // 12. Import Customers
        //string inputJson = File.ReadAllText(@"../../../Datasets/customers.json");
        //string result = ImportCustomers(dbContext, inputJson);

        // 13. Import Sales
        //string inputJson = File.ReadAllText(@"../../../Datasets/sales.json");
        //string result = ImportCustomers(dbContext, inputJson);

        // 14. Export Order Customers
        // string result = GetOrderedCustomers(dbContext);
        // string outputFilePath = @"../../../Results/ordered-customers.json";
        // File.WriteAllText(outputFilePath, result);

        // 15. Export Toyota Cars
        // string result = GetCarsFromMakeToyota(dbContext);

        // 16.Export Local Suppliers
        // string result = GetLocalSuppliers(dbContext);
        // string path = @"../../../Results/local-suppliers.json";
        // File.WriteAllText(path, result);

        // 17. Export Cars with Their List of Parts
        //string result = GetCarsWithTheirListOfParts(dbContext);
        //string path = @"../../../Results/cars-suppliers.json";
        //File.WriteAllText(path, result);

        // 18. Export Total Sales by Customer
        //string result = GetTotalSalesByCustomer(dbContext);
        //string path = @"../../../Results/customers-total-sales.json";
        //File.WriteAllText(path, result);

        // 19. Export Sales with Applied Discount
        string result = GetSalesWithAppliedDiscount(dbContext);
        string path = @"../../../Results/sales-discounts.json";
        File.WriteAllText(path, result);

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
        //  dbContext.SaveChanges();

        return $"Successfully imported {parts.Count}.";
    }

    public static string ImportCars(CarDealerContext dbContext, string inputJson)
    {
        var carsAndParts = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson);

        var cars = new List<Car>();
        var parts = new List<PartCar>();

        foreach (var dto in carsAndParts)
        {
            var car = new Car()
            {
                Make = dto.Make,
                Model = dto.Model,
                TraveledDistance = dto.TraveledDistance
            };
            cars.Add(car);

            foreach (var part in dto.PartsId.Distinct())
            {
                PartCar partCar = new PartCar()
                {
                    Car = car,
                    PartId = part,
                };
                parts.Add(partCar);
            }
        }

        dbContext.Cars.AddRange(cars);

        dbContext.PartsCars.AddRange(parts);
        // dbContext.SaveChanges();

        return $"Successfully imported {cars.Count}.";
    }
    public static string ImportCustomers(CarDealerContext dbContext, string inputJson)
    {
        var customers = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

        dbContext.Customers.AddRange(customers);
        // dbContext.SaveChanges();

        return $"Successfully imported {customers.Count}.";
    }
    public static string ImportSales(CarDealerContext dbContext, string inputJson)
    {
        var sales = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

        dbContext.Sales.AddRange(sales);
        dbContext.SaveChanges();

        return $"Successfully imported {sales.Count}.";
    }
    public static string GetOrderedCustomers(CarDealerContext dbContext)
    {
        var customers = dbContext.Customers
            .OrderBy(c => c.BirthDate)
            .ThenBy(c => c.IsYoungDriver)
            .Select(c => new
            {
                Name = c.Name,
                BirthDate = c.BirthDate.ToString(@"dd/MM/yyyy", CultureInfo.InvariantCulture),
                IsYoungDriver = c.IsYoungDriver
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(customers, Formatting.Indented);
    }

    public static string GetCarsFromMakeToyota(CarDealerContext dbContext)
    {
        var toyotaCars = dbContext.Cars
            .Where(c => c.Make == "Toyota")
            .OrderBy(c => c.Model)
            .ThenByDescending(c => c.TraveledDistance)
            .Select(c => new
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(toyotaCars, Formatting.Indented);
    }

    public static string GetLocalSuppliers(CarDealerContext dbContext)
    {
        var suppliers = dbContext.Suppliers
            .Where(s => s.IsImporter == false)
            .Select(s => new
            {
                Id = s.Id,
                Name = s.Name,
                PartsCount = s.Parts.Count
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(suppliers, Formatting.Indented);
    }

    public static string GetCarsWithTheirListOfParts(CarDealerContext dbContext)
    {
        var carsAndParts = dbContext.Cars
            .Select(c => new
            {
                car = new
                {

                    Make = c.Make,
                    Model = c.Model,
                    TraveledDistance = c.TraveledDistance,
                },
                parts = c.PartsCars
                    .Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = $"{p.Part.Price:f2}"
                    })
                    .ToArray()
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(carsAndParts, Formatting.Indented);
    }
    public static string GetTotalSalesByCustomer(CarDealerContext dbContext)
    {
        var customers = dbContext.Customers
            .Where(c => c.Sales.Count >= 1)
            .Select(c => new
            {
                fullName = c.Name,
                boughtCars = c.Sales.Count(),
                spentMoney = c.Sales.SelectMany(x => x.Car.PartsCars.Select(c => c.Part.Price))
            })
            .AsNoTracking()
            .ToArray();

        var totalSalesByCustomer = customers.Select(t => new
        {
            t.fullName,
            t.boughtCars,
            spentMoney = t.spentMoney.Sum()
        })
            .OrderByDescending(t => t.spentMoney)
            .ThenByDescending(t => t.boughtCars)
            .ToArray();

        return JsonConvert.SerializeObject(totalSalesByCustomer, Formatting.Indented);
    }

    public static string GetSalesWithAppliedDiscount(CarDealerContext dbContext)
    {
        var sales = dbContext.Sales
            .Take(10)
            .Select(s => new
            {
                car = new
                {
                    s.Car.Make,
                    s.Car.Model,
                    s.Car.TraveledDistance
                },
                customerName = s.Customer.Name,
                discount = $"{s.Discount:f2}",
                price = $"{s.Car.PartsCars.Sum(p => p.Part.Price):f2}",
                priceWithDiscount = $"{s.Car.PartsCars.Sum(p => p.Part.Price) * (1 - s.Discount / 100):f2}"
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(sales, Formatting.Indented);
    }

    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CarDealerProfile>();
        }));
    }
}