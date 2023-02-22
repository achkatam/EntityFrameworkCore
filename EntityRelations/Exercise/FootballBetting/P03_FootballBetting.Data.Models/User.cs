namespace P03_FootballBetting.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Common;
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }
        [Key]
        public int UserId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.UsernameMaxLength)]
        public string Username { get; set; }

        //Hashed string
        [Required]
        [MaxLength(GlobalConstants.PasswordMaxLength)]
        public string Password { get; set; }

        [Required]
        [MaxLength(GlobalConstants.EmailMaxLength)]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}
