namespace Theatre.Data.Models;

using System.ComponentModel.DataAnnotations;
using Enums;

public class Play
{

    public Play()
    {
        this.Casts = new HashSet<Cast>();
        this.Tickets = new HashSet<Ticket>();
    }

    [Key]
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public TimeSpan Duration { get; set; }
    
    public float Rating { get; set; }

    public Genre Genre { get; set; }

    public string Description { get; set; } = null!;

    public string Screenwriter { get; set; } = null!;

    public virtual ICollection<Cast> Casts { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = null!;
}