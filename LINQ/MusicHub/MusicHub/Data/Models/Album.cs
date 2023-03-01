namespace MusicHub.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
public class Album
{ 
    public Album()
    {
        this.Songs = new HashSet<Song>();
    }
    [Key]
    public int Id { get; set; }

    [MaxLength(ValidationConstants.AlbumNameMaxLength)]
    public string Name { get; set; } = null!;

    [NotMapped] // excludes it in DB
    public decimal Price
        => this.Songs.Sum(s => s.Price);

    public DateTime ReleaseDate { get; set; }

    [ForeignKey(nameof(Producer))]
    public int? ProducerId { get; set; }
    public virtual Producer? Producer { get; set; }

    public virtual ICollection<Song> Songs { get; set; }
}