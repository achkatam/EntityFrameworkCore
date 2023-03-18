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

        string inputXml = File.ReadAllText("../../../Datasets/categories-products.xml");
        string result = ImportCategoryProducts(dbContext, inputXml);

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

        foreach (var dto in categoryProductDtos.Where(x=> x.CategoryId != 0 && x.ProductId != 0))
        {
            CategoryProduct categoryProduct = mapper.Map<CategoryProduct>(dto);

            categoryProducts.Add(categoryProduct);
        }

        //var categoryProducts = categoryProductDtos.Where(x => x.CategoryId != 0 &&
        //                                                      x.ProductId != 0)
        //    .Select(cp => new CategoryProduct()
        //    {
        //        CategoryId = cp.CategoryId,
        //        ProductId = cp.ProductId
        //    })
        //    .ToArray();

        context.CategoryProducts.AddRange(categoryProducts);
        context.SaveChanges();

        return $"Successfully imported {categoryProducts.Count()}";
    }

    private static IMapper CreateMapper()
        => new Mapper(new MapperConfiguration(cfg =>
        {

            cfg.AddProfile<ProductShopProfile>();
        }));
}