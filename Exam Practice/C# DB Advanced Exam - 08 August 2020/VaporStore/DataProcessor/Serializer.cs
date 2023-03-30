namespace VaporStore.DataProcessor
{
    using System.Globalization;
    using Data;
    using Data.Models.Enums;
    using Data.Utilities;
    using ExportDto;

    public static class Serializer
    {
        public static string ExportUserPurchasesByType(VaporStoreDbContext context, string purchaseType)
        {
            var type = Enum.Parse<PurchaseType>(purchaseType);

            XmlHelper xmlHelper = new XmlHelper();
            
            var users = context.Users
                .ToArray()
                .Where(u => u.Cards.Any(c => c.Purchases.Any(p => p.Type == type)))
                .Select(u => new UserExportDto()
                {
                    Username = u.Username,
                    Purchases = u.Cards.SelectMany(c => c.Purchases)
                        .Where(p => p.Type == type)
                        .OrderBy(p => p.Date)
                        .Select(p => new PurchaseExportDto()
                        {
                            CardNumber = p.Card.Number,
                            Cvc = p.Card.Cvc,
                            Date = p.Date.ToString("yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture),
                            Games = new GameExportDto
                            {
                                GameTitle = p.Game.Name,
                                Genre = p.Game.Genre.Name,
                                Price = p.Game.Price
                            }
                        })
                        .ToArray(),
                    TotalSpent = u.Cards
                        .Sum(c => c.Purchases
                            .Where(p => p.Type == type)
                            .Sum(p => p.Game.Price))
                })
                .OrderByDescending(u => u.TotalSpent)
                .ThenBy(u => u.Username)
                .ToArray();


            return xmlHelper.Serialize<UserExportDto[]>(users, "Users");
        }

        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            throw new NotImplementedException();
        }
    }
}