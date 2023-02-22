
namespace P03_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;

    using System.ComponentModel.DataAnnotations.Schema;
    using System.Collections.Generic;

    public class Town
    {
        public Town()
        {
            this.Teams = new HashSet<Team>();
        }


        [Key]
        public int TownId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TownNameMaxLength)]
        public string Name { get; set; }

        
        [ForeignKey(nameof(Country))]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}