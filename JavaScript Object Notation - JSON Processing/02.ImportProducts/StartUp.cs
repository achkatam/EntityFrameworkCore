namespace ProductShop;

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
        string inputJson = File.ReadAllText(@"../../../Datasets/products.json");
        string result = ImportProducts(dbContext, inputJson);

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
        context.SaveChanges();

        return $"Successfully imported {products.Length}";
    }

    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}
