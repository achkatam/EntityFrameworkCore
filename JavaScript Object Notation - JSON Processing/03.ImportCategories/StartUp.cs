﻿using ProductShop.DTOs.Import.Category;

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
        //string inputJson = File.ReadAllText(@"../../../Datasets/products.json");
        //string result = ImportProducts(dbContext, inputJson);

        // 03. Import Categories
        string inputJson = File.ReadAllText(@"../../../Datasets/categories.json");
        string result = ImportCategories(dbContext, inputJson);
        
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

    private static IMapper CreateMapper()
    {
        return new Mapper(new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<ProductShopProfile>();
        }));
    }
}
