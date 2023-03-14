using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using ProductShop.DTOs.Export;

namespace ProductShop;

using DTOs.Import.CategoryProduct;
using DTOs.Import.Category;
using DTOs.Import.Product;
using AutoMapper;
using Models;
using Newtonsoft.Json;
using DTOs.Import.User;
using Data;

public class StartUp
{

    public static void Main()
    {
        ProductShopContext dbContext = new ProductShopContext();

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        // 01. Import Users
        //string inputJson = File.ReadAllText(@"../../../Datasets/users.json");
        //string result = ImportUsers(dbContext, inputJson);

        // 02. Import Products
        //string inputJson = File.ReadAllText(@"../../../Datasets/products.json");
        //string result = ImportProducts(dbContext, inputJson);

        // 03. Import Categories
        // string inputJson = File.ReadAllText(@"../../../Datasets/categories.json");
        // string result = ImportCategories(dbContext, inputJson);

        // 04.ImportCategoriesProducts
        // string inputJson = File.ReadAllText(@"../../../Datasets/categories-products.json");
        //string result = ImportCategoryProducts(dbContext, inputJson);

        // 05. ExportProducts in Range
        // string result = GetProductsInRange(dbContext);

        // 06.Export Sold Products
        // string result = GetSoldProducts(dbContext);

        // 07. Export Categories by Product Count
       // string result = GetCategoriesByProductsCount(dbContext);

       // 08. Export Users and Products
       string result = GetUsersWithProducts(dbContext);

        Console.WriteLine(result);
    }

    public static string ImportUsers(ProductShopContext context, string inputJson)
    {
        IMapper mapper = new Mapper(new MapperConfiguration(cfg =>
         {
             cfg.AddProfile<ProductShopProfile>();
         }));

        var userDtos = JsonConvert.DeserializeObject<ImportUserDto[]>(inputJson);

        ICollection<User> users = new HashSet<User>();

        foreach (var userDto in userDtos)
        {
            User user = mapper.Map<User>(userDto);

            users.Add(user);
        }
        context.Users.AddRange(users);
        // context.SaveChanges();

        return $"Successfully imported {users.Count}";
    }

    public static string ImportProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var productsDtos = JsonConvert.DeserializeObject<ImportProductDto[]>(inputJson);

        Product[] products = mapper.Map<Product[]>(productsDtos);

        context.Products.AddRange(products);
        //  context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    public static string ImportCategories(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var categoryDrtos =
             JsonConvert.DeserializeObject<ImportCategoryDto[]>(inputJson);

        ICollection<Category> categories = new HashSet<Category>();

        foreach (var categoryDto in categoryDrtos)
        {
            if (String.IsNullOrEmpty(categoryDto.Name))
            {
                continue;
            }

            Category category = mapper.Map<Category>(categoryDto);

            categories.Add(category);
        }

        context.Categories.AddRange(categories);
        // context.SaveChanges();

        return $"Successfully imported {categories.Count}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
    {
        IMapper mapper = CreateMapper();

        var cprDtos = JsonConvert.DeserializeObject<ImportCategoryProduct[]>(inputJson);

        ICollection<CategoryProduct> cps = new HashSet<CategoryProduct>();

        foreach (var categoryProduct in cprDtos)
        {
            var catProd = mapper.Map<CategoryProduct>(categoryProduct);
            cps.Add(catProd);
        }

        context.CategoriesProducts.AddRange(cps);
        context.SaveChanges();

        return $"Successfully imported {cps.Count}";
    }

    public static string GetProductsInRange(ProductShopContext dbContext)
    {
        // using Anonymous objects
        //var products = dbContext.Products
        //    .Where(p => p.Price >= 500 && p.Price <= 1000)
        //    .OrderBy(p => p.Price)
        //    .Select(p => new
        //    {
        //        name = p.Name,
        //        price = p.Price,
        //        seller = p.Seller.FirstName + " " + p.Seller.LastName
        //    })
        //    .AsNoTracking()
        //    .ToArray();

        // return JsonConvert.SerializeObject(products, Formatting.Indented);

        // using DTO + AutoMapper

        IMapper mapper = CreateMapper();

        ExportProductInRangeDto[] productDtos = dbContext.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .AsNoTracking()
            .ProjectTo<ExportProductInRangeDto>(mapper.ConfigurationProvider)
            .ToArray();

        return JsonConvert.SerializeObject(productDtos, Formatting.Indented);
    }
    public static string GetSoldProducts(ProductShopContext context)
    {
        IContractResolver contractResolver = ConfigureCamelCaseNaming();

        var usersWithSoldProducts = context.Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Select(u => new
            {
                u.FirstName,
                u.LastName,
                soldProducts = u.ProductsSold
                    .Where(p => p.Buyer != null)
                    .Select(p => new
                    {
                        p.Name,
                        p.Price,
                        BuyerFirstName = p.Buyer.FirstName,
                        BuyerLastName = p.Buyer.LastName
                    })
                    .ToArray()
            })
            .AsNoTracking()
            .ToArray();


        return JsonConvert.SerializeObject(usersWithSoldProducts, Formatting.Indented, new JsonSerializerSettings()
        {
            ContractResolver = contractResolver
        });
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        IContractResolver contractResolver = ConfigureCamelCaseNaming();


        var categoriesByProductsCount = context.Categories
            .OrderByDescending(c => c.CategoriesProducts.Count)
            .Select(c => new
            {
                Category = c.Name,
                ProductsCount = c.CategoriesProducts.Count,
                AveragePrice = Math.Round((double)c.CategoriesProducts.Average(p => p.Product.Price), 2),
                TotalRevenue = Math.Round((double)c.CategoriesProducts.Sum(p => p.Product.Price), 2)
            })
            .AsNoTracking()
            .ToArray();

        return JsonConvert.SerializeObject(categoriesByProductsCount, Formatting.None, new JsonSerializerSettings()
        {
            ContractResolver = contractResolver
        });
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        var usersWithProducts = context
            .Users
            .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
            .OrderByDescending(u => u.ProductsSold
                .Where(p => p.Buyer != null)
                .Count())
            .Select(u => new
            {
                firstName = u.FirstName,
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    count = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Count(),
                    products = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price
                        })
                }
            })
            .ToArray();

        var usersInfo = new
        {
            usersCount = usersWithProducts.Count(),
            users = usersWithProducts
        };

        string output = JsonConvert.SerializeObject(usersInfo, new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
        });

        return output;
    }




    private static IContractResolver ConfigureCamelCaseNaming()
    {
        return new DefaultContractResolver()
        {
            NamingStrategy = new CamelCaseNamingStrategy(false, true)
        };
    }
    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}
