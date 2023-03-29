namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using Data.Models;

    public class User
    {
        public User()
        {
            this.Cards = new HashSet<Card>();
        }

        [Key]
        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int Age { get; set; }

        public virtual ICollection<Card> Cards { get; set; } = null!;
    }
}
