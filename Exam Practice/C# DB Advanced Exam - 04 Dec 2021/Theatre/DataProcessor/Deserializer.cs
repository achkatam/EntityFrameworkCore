namespace Theatre.DataProcessor;

using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using Data.Utilities;
using Data;
using Data.Models;
using Data.Models.Enums;
using ImportDto;
using Newtonsoft.Json;

public class Deserializer
{

    private const string ErrorMessage = "Invalid data!";

    private const string SuccessfulImportPlay
        = "Successfully imported {0} with genre {1} and a rating of {2}!";

    private const string SuccessfulImportActor
        = "Successfully imported actor {0} as a {1} character!";

    private const string SuccessfulImportTheatre
        = "Successfully imported theatre {0} with #{1} tickets!";



    public static string ImportPlays(TheatreContext context, string xmlString)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var sb = new StringBuilder();

        var playDtos = xmlHelper.Deserializer<ImportPlayDto[]>(xmlString, "Plays");

        var plays = new HashSet<Play>();

        foreach (var playDto in playDtos)
        {
            var currTime = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture);

            bool genreCheck = Enum.TryParse(playDto.Genre, out Genre genre);

            if (!IsValid(playDto)
                || currTime < new TimeSpan(1, 0, 0)
                || !genreCheck
                || string.IsNullOrEmpty(playDto.Description))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Play play = new Play()
            {
                Title = playDto.Title,
                Duration = TimeSpan.ParseExact(playDto.Duration, "c", CultureInfo.InvariantCulture),
                Rating = playDto.Rating,
                Genre = genre,
                Description = playDto.Description,
                Screenwriter = playDto.Screenwriter
            };

            plays.Add(play);
            sb.AppendLine(string.Format(SuccessfulImportPlay, play.Title, genre, play.Rating));
        }

        context.Plays.AddRange(plays);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportCasts(TheatreContext context, string xmlString)
    {
        var sb = new StringBuilder();
        XmlHelper xmlHelper = new XmlHelper();

        var castDtos = xmlHelper.Deserializer<ImportCastDto[]>(xmlString, "Casts");

        var casts = new HashSet<Cast>();

        foreach (var castDto in castDtos)
        {
            if (!IsValid(castDto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            Cast cast = new Cast()
            {
                FullName = castDto.FullName,
                IsMainCharacter = castDto.IsMainCharacter,
                PhoneNumber = castDto.PhoneNumber,
                PlayId = castDto.PlayId
            };

            casts.Add(cast);

            sb.AppendLine(string.Format(SuccessfulImportActor, cast.FullName,
                cast.IsMainCharacter == true ? "main" : "lesser"));
        }

        context.Casts.AddRange(casts);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }

    public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
    {
        var sb = new StringBuilder();

        var theaterDtos = JsonConvert.DeserializeObject<ImportTheatreDto[]>(jsonString);

        var theaters = new HashSet<Theatre>();

        foreach (var dto in theaterDtos)
        {
            if (!IsValid(dto))
            {
                sb.AppendLine(ErrorMessage);
                continue;
            }

            var theater = new Theatre()
            {
                Name = dto.Name,
                NumberOfHalls = dto.NumberOfHalls,
                Director = dto.Director
            };

            foreach (var dtoTicket in dto.Tickets)
            {
                if (!IsValid(dtoTicket))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Ticket ticket = new Ticket()
                {
                    Price = dtoTicket.Price,
                    RowNumber = dtoTicket.RowNumber,
                    PlayId = dtoTicket.PlayId
                };

                theater.Tickets.Add(ticket);
            }

            theaters.Add(theater);
            sb.AppendLine(string.Format(SuccessfulImportTheatre, theater.Name, theater.Tickets.Count));
        }

        context.Theatres.AddRange(theaters);
        context.SaveChanges();

        return sb.ToString().TrimEnd();
    }


    private static bool IsValid(object obj)
    {
        var validator = new ValidationContext(obj);
        var validationRes = new List<ValidationResult>();

        var result = Validator.TryValidateObject(obj, validator, validationRes, true);
        return result;
    }
}