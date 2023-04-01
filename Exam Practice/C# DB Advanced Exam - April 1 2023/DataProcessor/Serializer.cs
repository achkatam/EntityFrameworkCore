namespace Boardgames.DataProcessor;

using System.Text;

using Newtonsoft.Json;
using Data;
using Data.Utilities;
using ExportDto;

public class Serializer
{
    public static string ExportCreatorsWithTheirBoardgames(BoardgamesContext context)
    {
        XmlHelper xmlHelper = new XmlHelper();
        var sb = new StringBuilder();

        var creators = context.Creators
            .Where(c => c.Boardgames.Any())
            .ToArray()
            .Select(cc => new CreatorExportDto()
            {
                Count = cc.Boardgames.Count,
                CreatorName = $"{cc.FirstName} {cc.LastName}",
                BoardgameExportDtos = cc.Boardgames
                    .Select(bg => new BoardgameExportDto()
                    {
                        BoardgameName = bg.Name,
                        PublishedYear = bg.YearPublished
                    })
                    .OrderBy(bg => bg.BoardgameName)
                    .ToArray()
            })
            .OrderByDescending(c => c.Count)
            .ThenBy(c => c.CreatorName)
            .ToArray();

        return xmlHelper.Serialize<CreatorExportDto[]>(creators, "Creators");
    }

    public static string ExportSellersWithMostBoardgames(BoardgamesContext context, int year, double rating)
    {
        var sellers = context.Sellers
            .Where(s => s.BoardgamesSellers.Any(x => x.Boardgame.YearPublished >= year
                   && x.Boardgame.Rating <= rating))
            .ToArray()
            .Select(s => new
            {
                Name = s.Name,
                Website = s.Website,
                Boardgames = s.BoardgamesSellers
                    .Where(b => b.Boardgame.YearPublished >= year
                                && b.Boardgame.Rating <= rating)
                    .OrderByDescending(b => b.Boardgame.Rating)
                    .ThenBy(b => b.Boardgame.Name)
                    .Select(b => new
                    {
                        Name = b.Boardgame.Name,
                        Rating = b.Boardgame.Rating,
                        Mechanics = b.Boardgame.Mechanics,
                        Category = b.Boardgame.CategoryType.ToString()
                    })
                    .ToArray()
            })
            .OrderByDescending(s => s.Boardgames.Count())
            .ThenBy(s => s.Name)
            .Take(5)
            .ToArray();

        return JsonConvert.SerializeObject(sellers, Formatting.Indented);
    }
}