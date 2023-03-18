namespace ProductShop;

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

        string inputXml = File.ReadAllText("../../../Datasets/users.xml");
        string result = ImportUsers(dbContext, inputXml);

        Console.WriteLine(result);
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

        dbContext.AddRange(users);
        // dbContext.SaveChanges();

        return $"Successfully imported {users.Count}";
    }

    private static IMapper CreateMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {

            cfg.AddProfile<ProductShopProfile>();
        }));
}