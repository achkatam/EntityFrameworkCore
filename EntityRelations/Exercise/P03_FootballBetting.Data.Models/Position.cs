namespace P03_FootballBetting.Data.Models
{
    using System.Collections;
    using System.Collections.Generic;

    using Common;
    using System.ComponentModel.DataAnnotations;

    public class Position
    {
        public Position()
        {
            this.Players = new HashSet<Player>();
        }
        [Key]
        public int PositionId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.PositionMaxLength)]
        public string Name { get; set; }

        public ICollection<Player> Players { get; set; }
    }
}
