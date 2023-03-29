namespace VaporStore.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text; 
    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using ImportDto;
    using Newtonsoft.Json;

    public static class Deserializer
    {
        public const string ErrorMessage = "Invalid Data";

        public const string SuccessfullyImportedGame = "Added {0} ({1}) with {2} tags";

        public const string SuccessfullyImportedUser = "Imported {0} with {1} cards";

        public const string SuccessfullyImportedPurchase = "Imported {0} for {1}";

        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var gameDtos = JsonConvert.DeserializeObject<GameImportDto[]>(jsonString);

            var games = new HashSet<Game>();
            var developers = new HashSet<Developer>();
            var genres = new HashSet<Genre>();
            var tags = new HashSet<Tag>();

            foreach (var dto in gameDtos)
            {
                bool releaseDateCheck = DateTime.TryParseExact(dto.ReleaseDate, "yyyy-MM-dd",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime releaseDate);

                if (!IsValid(dto)
                    || dto.Price < 0
                    || string.IsNullOrWhiteSpace(dto.Name)
                    || !releaseDateCheck
                    || string.IsNullOrWhiteSpace(dto.Genre)
                    || string.IsNullOrWhiteSpace(dto.Developer)
                    || !dto.Tags.Any())
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Game game = new Game()
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    ReleaseDate = releaseDate
                };

                Developer dev = developers.FirstOrDefault(d => d.Name == dto.Developer);
                if (dev is null)
                {
                    dev = new Developer() { Name = dto.Developer };
                    developers.Add(dev);
                }
                game.Developer = dev;

                Genre genre = genres.FirstOrDefault(x => x.Name == dto.Genre);
                if (genre is null)
                {
                    genre = new Genre()
                    {
                        Name = dto.Genre
                    };
                    genres.Add(genre);
                }
                game.Genre = genre;

                foreach (string tagDto in dto.Tags)
                {
                    Tag tag = tags.FirstOrDefault(x => x.Name == tagDto);
                    if (tag is null)
                    {
                        tag = new Tag() { Name = tagDto };
                        tags.Add(tag);
                    }

                    GameTag gameTag = new GameTag()
                    {
                        Game = game,
                        Tag = tag
                    };

                    game.GameTags.Add(gameTag);
                }
                games.Add(game);
                sb.AppendLine(string.Format(SuccessfullyImportedGame, game.Name, genre.Name, game.GameTags.Count));
            }
            context.Games.AddRange(games);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var sb = new StringBuilder();
            var userDtos = JsonConvert.DeserializeObject<UserImportDto[]>(jsonString);

            var users = new HashSet<User>();

            foreach (var userDto in userDtos)
            {
                if (!IsValid(userDto)
                    || string.IsNullOrEmpty(userDto.Email)
                    || !userDto.Cards.Any()
                    || string.IsNullOrWhiteSpace(userDto.FullName))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                User user = new User()
                {
                    FullName = userDto.FullName,
                    Username = userDto.Username,
                    Email = userDto.Email,
                    Age = userDto.Age
                };

                foreach (var cardDto in userDto.Cards)
                {
                    bool cardTypeCheck = Enum.TryParse<CardType>(cardDto.Type, out CardType validCardType);

                    if (!IsValid(cardDto)
                        || !cardTypeCheck)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Card card = new Card()
                    {
                        Number = cardDto.Number,
                        Cvc = cardDto.Cvc,
                        Type = validCardType
                    };

                    user.Cards.Add(card);
                }

                users.Add(user);
                sb.AppendLine(string.Format(SuccessfullyImportedUser, user.Username, user.Cards.Count));
            }
            context.Users.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}