
namespace P03_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;


    public class Player
    {
        public Player()
        {
            this.PlayerStatistics = new HashSet<PlayerStatistic>();
        }
        [Key]
        public int PlayerId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PlayerNameMaxLength)]
        public string Name { get; set; }

        public byte SquadNumber { get; set; }

        [ForeignKey(nameof(Team))]
        public int TeamId { get; set; }
        public Team Team { get; set; }

        [ForeignKey(nameof(Position))]
        public int PositionId { get; set; }
        public virtual Position Position { get; set; }

        public virtual ICollection<PlayerStatistic> PlayerStatistics { get; set; }
          
        public bool IsInjured { get; set; }
    }
}
