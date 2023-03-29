namespace VaporStore.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Game
    {
        public Game()
        {
            this.GameTags = new HashSet<GameTag>();
            this.Purchases = new HashSet<Purchase>();
        }

        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }
        public DateTime ReleaseDate { get; set; }

        [ForeignKey(nameof(Developer))]
        public int DeveloperId { get; set; }
        public virtual Developer Developer { get; set; } = null!;

        [ForeignKey(nameof(Genre))]
        public int GenreId { get; set; }
        public virtual Genre Genre { get; set; } = null!;

        public virtual ICollection<Purchase> Purchases { get; set; } = null!;

        public virtual ICollection<GameTag> GameTags { get; set; } = null!;
    }
}
