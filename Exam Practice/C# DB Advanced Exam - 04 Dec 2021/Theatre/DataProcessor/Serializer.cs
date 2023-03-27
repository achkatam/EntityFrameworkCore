namespace Theatre.DataProcessor;

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Data;
using Data.Models.Enums;
using Data.Utilities;
using ExportDto;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class Serializer
{
    public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
    {
        var theaters = context.Theatres
            .Where(t => t.NumberOfHalls >= numbersOfHalls
                        && t.Tickets.Count >= 20)
            .Select(t => new ExportTheaterDto()
            {
                Name = t.Name,
                NumberOfHalls = t.NumberOfHalls,
                TotalIncome = t.Tickets.Where(x => x.RowNumber <= 5).Sum(x => x.Price),
                Tickets = t.Tickets.Where(t => t.RowNumber <= 5)
                    .Select(x => new ExportTicketDto()
                    {
                        Price = x.Price,
                        RowNumber = x.RowNumber
                    })
                    .OrderByDescending(t => t.Price)
                    .ToArray()
            })
            .OrderByDescending(t => t.NumberOfHalls)
            .ThenBy(t => t.Name)
            .ToArray();

        return JsonConvert.SerializeObject(theaters, Formatting.Indented);
    }


    public static string ExportPlays(TheatreContext context, double raiting)
    {
        XmlHelper xmlHelper = new XmlHelper();

        var actors = context.Plays
            .Where(p => p.Rating <= raiting)
            .OrderBy(p => p.Title)
            .ThenByDescending(t => t.Genre)
            .Select(p => new ExportPlayDto()
            {
                Title = p.Title,
                Duration = p.Duration.ToString("c"),
                Rating = p.Rating == 0 ? "Premier" : p.Rating.ToString(),
                Genre = p.Genre.ToString(),
                Actors = p.Casts
                    .Where(p => p.IsMainCharacter)
                    .Select(c => new ExportCastDto()
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{p.Title}'."
                    })
                    .OrderByDescending(c => c.FullName)
                    .ToArray()
            })
            .ToArray();

        return xmlHelper.Serialize<ExportPlayDto[]>(actors, "Actors");
    }
}