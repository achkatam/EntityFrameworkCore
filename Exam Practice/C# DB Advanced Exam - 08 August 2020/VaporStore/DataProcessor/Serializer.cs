namespace VaporStore.DataProcessor
{
    using System.Globalization;
    using Data;
    using Data.Models.Enums;
    using Data.Utilities;
    using ExportDto;
    using Newtonsoft.Json;

    public static class Serializer
    {

        public static string ExportGamesByGenres(VaporStoreDbContext context, string[] genreNames)
        {
            var genres = context.Genres
                .Where((g => genreNames.Contains(g.Name)
                             && g.Games.Any()))
                .Select(g => new GenreExport()
                {
                    Id = g.Id,
                    Genre = g.Name,
                    Games = g.Games
                        .Where(g => g.Purchases.Any())
                        .Select(x => new GameJsonExportDto()
                        {
                            Id = x.Id,
                            Title = x.Name,
                            Developer = x.Developer.Name,
                            Tags = string.Join(", ", x.GameTags.Select(gt => gt.Tag.Name)),
                            Players = x.Purchases.Count
                        })
                        .OrderByDescending(g => g.Players)
                        .ThenBy(x => x.Id)
                        .ToArray(),
                    TotalPlayers = g.Games.SelectMany(x => x.Purchases).Count()
                })
                .OrderByDescending(x => x.TotalPlayers)
                .ThenBy(x => x.Id)
                .ToList();

            return JsonConvert.SerializeObject(genres, Formatting.Indented);
        }
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
    }
}