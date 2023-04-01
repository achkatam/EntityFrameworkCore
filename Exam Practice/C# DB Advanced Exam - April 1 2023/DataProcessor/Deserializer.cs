namespace Boardgames.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Text;

using Data;
using Data.Models;
using Data.Models.Enums;
using Data.Utilities;
using ImportDto;
using Newtonsoft.Json;

public class Deserializer
{
    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfullyImportedCreator
        = "Successfully imported creator – {0} {1} with {2} boardgames.";

    private const string SuccessfullyImportedSeller
        = "Successfully imported seller - {0} with {1} boardgames.";

    public static string ImportCreators(BoardgamesContext context, string xmlString)
    {
        XmlHelper xmlHelper = new XmlHelper();
        var sb = new StringBuilder();

        var creatorDtos = xmlHelper.Deserializer<CreatorImportDto[]>(xmlString, "Creators");

        var creators = new HashSet<Creator>();

        foreach (var creatorDto in creatorDtos)
        {
            if (!IsValid(creatorDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Creator creator = new Creator()
            {
                FirstName = creatorDto.FirstName,
                LastName = creatorDto.LastName
            };

            foreach (var boardgameDto in creatorDto.Boardgames)
            {
                bool isValidCategory = Enum.TryParse(boardgameDto.CategoryType, out CategoryType categoryType);

                if (!IsValid(boardgameDto)
                    || string.IsNullOrWhiteSpace(boardgameDto.Name)
                    || !isValidCategory)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Boardgame boardgame = new Boardgame()
                {
                    Name = boardgameDto.Name,
                    Rating = boardgameDto.Rating,
                    YearPublished = boardgameDto.YearPublished,
                    CategoryType = categoryType,
                    Mechanics = boardgameDto.Mechanics
                };

                creator.Boardgames.Add(boardgame);
            }

            creators.Add(creator);
            sb.AppendLine(string.Format(SuccessfullyImportedCreator, creator.FirstName, creator.LastName, creator.Boardgames.Count));
        }
        context.Creators.AddRange(creators);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportSellers(BoardgamesContext context, string jsonString)
    {
        var sb = new StringBuilder();

        var sellerDtos = JsonConvert.DeserializeObject<SellerImportDto[]>(jsonString);

        var sellers = new HashSet<Seller>();

        foreach (var sellerDto in sellerDtos)
        {

            if (!IsValid(sellerDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Seller seller = new Seller()
            {
                Name = sellerDto.Name,
                Address = sellerDto.Address,
                Country = sellerDto.Country,
                Website = sellerDto.Website
            };

            foreach (var boardgameSellerId in sellerDto.Boardgames.Distinct())
            {

                var boardgame = context.Boardgames.FirstOrDefault(x => x.Id == boardgameSellerId);

                if (boardgame is null)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                BoardgameSeller bgSeller = new BoardgameSeller()
                {
                    Seller = seller,
                    BoardgameId = boardgameSellerId
                };

                seller.BoardgamesSellers.Add(bgSeller);
            }

            sellers.Add(seller);
            sb.AppendLine(string.Format(SuccessfullyImportedSeller, seller.Name, seller.BoardgamesSellers.Count));
        }

        context.Sellers.AddRange(sellers);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    private static bool IsValid(object dto)
    {
        var validationContext = new ValidationContext(dto);
        var validationResult = new List<ValidationResult>();

        return Validator.TryValidateObject(dto, validationContext, validationResult, true);
    }
}