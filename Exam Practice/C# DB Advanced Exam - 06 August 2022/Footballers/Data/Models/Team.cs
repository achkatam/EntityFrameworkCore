namespace Footballers.Data.Models
{
    using Utilities;

    using System.ComponentModel.DataAnnotations;

    public class Team
    {
        public Team()
        {
            this.TeamsFootballers = new HashSet<TeamFootballer>();
        }

        [Key]
        public int Id { get; set; }

        [MinLength(ValidationConstants.TEAM_NAME_MIN)]
        [MaxLength(ValidationConstants.TEAM_NAME_MAX)]
        [RegularExpression(ValidationConstants.REGEX_PATTERN)]
        public string Name { get; set; } = null!;

        [MinLength(ValidationConstants.NATIONALITY_NAME_MIN)]
        [MaxLength(ValidationConstants.NATIONALITY_NAME_MAX)]
        public string Nationality { get; set; } = null!;

        public int Trophies { get; set; }

        public virtual ICollection<TeamFootballer> TeamsFootballers { get; set; } = null!;
    }
}
