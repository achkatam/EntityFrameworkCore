namespace P03_FootballBetting.Data.Models
{
    using System;
    using Enums;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class Bet
    {
        [Key]
        public int BetId { get; set; }

        [Required]
        public Prediction Prediction { get; set; }

        public decimal Amount { get; set; }

        // Required by default
        public DateTime DateTime { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }
        public virtual User User { get; set; }


        [ForeignKey(nameof(Game))]
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
