namespace ProductShop
{
    using Newtonsoft.Json;
    using Models;

    using Data;

    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext dbContext = new ProductShopContext();

            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            string inputJson = File.ReadAllText(@"../../../Datasets/users.json");
            string result = ImportUsers(dbContext, inputJson);

            Console.WriteLine(result);
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            ICollection<User> users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
    }
}