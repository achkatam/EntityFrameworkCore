namespace Footballers.DataProcessor
{
    using System.Globalization;
    using Newtonsoft.Json;

    using Data;
    using Data.Utilities;
    using ExportDto;

    public class Serializer
    {
        public static string ExportTeamsWithMostFootballers(FootballersContext context, DateTime date)
        {
            
            var teams = context.Teams
                .Where(t => t.TeamsFootballers.Any(t => t.Footballer.ContractStartDate >= date))
                .Select(t => new ExportTeamDto()
                {
                    Name = t.Name,
                    Footballers = t.TeamsFootballers
                        .Where(f => f.Footballer.ContractStartDate >= date)
                        .OrderByDescending(c => c.Footballer.ContractEndDate)
                        .ThenBy(f => f.Footballer.Name)
                        .Select(f => new ExportFootballerDto()
                        {
                            FootballerName = f.Footballer.Name,
                            ContractStartDate =
                                f.Footballer.ContractStartDate.ToString("d", CultureInfo.InvariantCulture),
                            ContractEndDate = f.Footballer.ContractEndDate.ToString("d", CultureInfo.InvariantCulture),
                            BestSkillType = f.Footballer.BestSkillType.ToString(),
                            PositionType = f.Footballer.PositionType.ToString()
                        })
                        .ToArray()
                })
                .OrderByDescending(t => t.Footballers.Count())
                .ThenBy(t => t.Name)
                .Take(5);

            return JsonConvert.SerializeObject(teams, Formatting.Indented);
        }
        public static string ExportCoachesWithTheirFootballers(FootballersContext context)
        {
            XmlHelper xmlHelper = new XmlHelper();

            var coaches = context.Coaches
                .Where(c => c.Footballers.Any())
                .Select(c => new ExportCoachDto()
                {
                    CoachName = c.Name,
                    Count = c.Footballers.Count,
                    Footballers = c.Footballers
                        .Select(f => new ExportFootballerXmlDto()
                        {
                            Name = f.Name,
                            Position = f.PositionType.ToString()
                        })
                        .OrderBy(f => f.Name)
                        .ToArray()
                })
                .OrderByDescending(c => c.Count)
                .ThenBy(c => c.CoachName)
                .ToArray();

            return xmlHelper.Serialize<ExportCoachDto[]>(coaches, "Coaches");
        }
    }
}