namespace Footballers.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;

    using Data;
    using Data.Models;
    using Data.Models.Enums;
    using Data.Utilities;
    using ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedCoach
            = "Successfully imported coach - {0} with {1} footballers.";

        private const string SuccessfullyImportedTeam
            = "Successfully imported team - {0} with {1} footballers.";

        public static string ImportCoaches(FootballersContext context, string xmlString)
        {
            XmlHelper xmlHelper = new XmlHelper();
            var sb = new StringBuilder();

            var coachDtos = xmlHelper.Deserializer<ImportCoachXmlDto[]>(xmlString, "Coaches");

            var coaches = new HashSet<Coach>();
            var footballers = new HashSet<Footballer>();

            foreach (var coachDto in coachDtos)
            {
                if (!IsValid(coachDto) || string.IsNullOrEmpty(coachDto.Nationality))
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Coach coach = new Coach()
                {
                    Name = coachDto.Name,
                    Nationality = coachDto.Nationality,
                };

                foreach (var footballerDto in coachDto.Footballers)
                {
                    bool startDate = DateTime.TryParseExact(footballerDto.ContractStartDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startContractDate);

                    bool endDate = DateTime.TryParseExact(footballerDto.ContractEndDate, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endContractDate);

                    if (!IsValid(footballerDto) || !startDate || !endDate || startContractDate > endContractDate || string.IsNullOrEmpty(footballerDto.Name))
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    Footballer footballer = new Footballer()
                    {
                        Name = footballerDto.Name,
                        ContractStartDate = startContractDate,
                        ContractEndDate = endContractDate,
                        BestSkillType = (BestSkillType)footballerDto.BestSkillType,
                        PositionType = (PositionType)footballerDto.PositionType
                    };

                    coach.Footballers.Add(footballer);
                }

                coaches.Add(coach);

                sb.AppendLine(string.Format(SuccessfullyImportedCoach, coach.Name, coach.Footballers.Count));
            }

            context.Coaches.AddRange(coaches);
             context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTeams(FootballersContext context, string jsonString)
        {
            var sb = new StringBuilder();

            var teamDtos = JsonConvert.DeserializeObject<ImportTeamJsonDto[]>(jsonString);

            var teams = new HashSet<Team>();

            foreach (var teamDto in teamDtos)
            {
                if (!IsValid(teamDto) || teamDto.Trophies == 0)
                {
                    sb.AppendLine(ErrorMessage);

                    continue;
                }

                Team team = new Team()
                {
                    Name = teamDto.Name,
                    Nationality = teamDto.Nationality,
                    Trophies = teamDto.Trophies
                };

                foreach (var currId in teamDto.FootballersIds.Distinct())
                {
                    var footballer = context.Footballers.FirstOrDefault(x => x.Id == currId);

                    if (footballer == null)
                    {
                        sb.AppendLine(ErrorMessage);

                        continue;
                    }

                    TeamFootballer teamFootballer = new TeamFootballer()
                    {
                        Team = team,
                        Footballer = footballer
                    };

                    team.TeamsFootballers.Add(teamFootballer);
                }

                teams.Add(team);
                sb.AppendLine(string.Format(SuccessfullyImportedTeam, team.Name, team.TeamsFootballers.Count));
            }


            context.Teams.AddRange(teams);
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
}