namespace ProductShop;

using ProductShop.DTOs.Export;
using DTOs.Import;
using Models;
using Utilities;
using AutoMapper;
using Data;

public class StartUp
{
    public static void Main()
    {
        ProductShopContext dbContext = new ProductShopContext();


        // Import Data 
        //string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
        //string result = ImportCategoryProducts(dbContext, inputXml);


        // Export data
        string xmlResult = GetUsersWithProducts(dbContext);
        File.WriteAllText(@"../../../Results/users-and-products.xml", xmlResult);

        Console.WriteLine(xmlResult);
    }

    public static string ImportUsers(ProductShopContext dbContext, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        ImportUserDto[] userDtos = xmlHelper.Deserializer<ImportUserDto[]>(inputXml, "Users");

        ICollection<User> users = new HashSet<User>();

        foreach (var userDto in userDtos)
        {
            User user = mapper.Map<User>(userDto);

            users.Add(user);
        }

        dbContext.Users.AddRange(users);
        dbContext.SaveChanges();

        return $"Successfully imported {users.Count}";
    }
    public static string ImportProducts(ProductShopContext dbContext, string inputXml)
    {
        //IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var productDtos = xmlHelper.Deserializer<ImportProductDto[]>(inputXml, "Products");

        Product[] products = productDtos
            .Select(p => new Product()
            {
                Name = p.Name,
                Price = p.Price,
                BuyerId = p.BuyerId == 0 ? null : p.BuyerId,
                SellerId = p.SellerId
            })
            .ToArray();

        dbContext.Products.AddRange(products);
        dbContext.SaveChanges();

        return $"Successfully imported {products.Count()}";
    }

    public static string ImportCategories(ProductShopContext dbContext, string inputXml)
    {
        XmlHelper xmlHelper = new XmlHelper();
        IMapper mapper = CreateMapper();

        var categoryDtos = xmlHelper.Deserializer<ImportCategoryDto[]>(inputXml, "Categories");

        ICollection<Category> categories = new HashSet<Category>();

        foreach (var categoryDto in categoryDtos.Where(x => x.Name is not null))
        {
            Category category = mapper.Map<Category>(categoryDto);

            categories.Add(category);
        }

        dbContext.Categories.AddRange(categories);
        // dbContext.SaveChanges();

        return $"Successfully imported {categories.Count}";
    }

    public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
    {
        IMapper mapper = CreateMapper();
        XmlHelper xmlHelper = new XmlHelper();

        var categoryProductDtos = xmlHelper.Deserializer<ImportCategoryProductDto[]>(inputXml, "CategoryProducts");

        ICollection<CategoryProduct> categoryProducts = new HashSet<CategoryProduct>();

        foreach (var dto in categoryProductDtos.Where(x => x.CategoryId != 0 && x.ProductId != 0))
        {
            CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(dto);

            categoryProducts.Add(categoryProduct);
        }

        context.CategoryProducts.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count()}";
    }

    public static string GetProductsInRange(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var products = context.Products
            .Where(p => p.Price >= 500 && p.Price <= 1000)
            .OrderBy(p => p.Price)
            .Take(10)
            .Select(p => new ExportProductDto()
            {
                Name = p.Name,
                Price = p.Price,
                Buyer = $"{p.Buyer.FirstName} {p.Buyer.LastName}"
            })
            .ToArray();

        return xmlHelper.Serialize<ExportProductDto[]>(products, "Products");
    }

    public static string GetSoldProducts(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var users = context.Users
            .Where(u => u.ProductsSold.Count >= 1)
            .OrderBy(u => u.LastName)
            .ThenBy(u => u.FirstName)
            .Take(5)
            .Select(u => new ExportSoldProductDto()
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                SoldProducts = u.ProductsSold
                    .Select(p => new ProductDto()
                    {
                        Name = p.Name,
                        Price = p.Price
                    })
                    .ToArray()
            })
            .ToArray();

        return xmlHelper.Serialize<ExportSoldProductDto[]>(users, "Users");
    }

    public static string GetCategoriesByProductsCount(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var categories = context.Categories
            .Select(c => new ExportCategoryByProduct()
            {
                Name = c.Name,
                Count = c.CategoryProducts.Count,
                AveragePrice = c.CategoryProducts.Average(p => p.Product.Price),
                TotalRevenue = c.CategoryProducts.Sum(p => p.Product.Price)
            })
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.TotalRevenue)
            .ToArray();

        return xmlHelper.Serialize<ExportCategoryByProduct[]>(categories, "Categories");
    }

    public static string GetUsersWithProducts(ProductShopContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var allUsers = context.Users
            .Where(x => x.ProductsSold.Any())
            .OrderByDescending(x => x.ProductsSold.Count)
            .Take(10)
            .Select(x => new UserDto()
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Age = x.Age,
                SoldProducts = new SoldProducts()
                {
                    Count = x.ProductsSold.Count,
                    Products = x.ProductsSold
                        .Select(x => new ProductDto()
                        {
                            Name = x.Name,
                            Price = x.Price
                        })
                        .OrderByDescending(x => x.Price)
                        .ToArray()
                }
            })
            .ToArray();

        var exportUsers = new ExportUserAndProductsDto()
        {
            Count = context.Users.Count(x => x.ProductsSold.Any()),
            Users = allUsers
        };

        return xmlHelper.Serialize<ExportUserAndProductsDto>(exportUsers, "Users");
    }

    private static IMapper CreateMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {

            cfg.AddProfile<ProductShopProfile>();
        }));
}