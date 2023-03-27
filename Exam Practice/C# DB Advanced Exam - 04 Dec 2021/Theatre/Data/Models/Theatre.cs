namespace Theatre.Data.Models;

using System.ComponentModel.DataAnnotations;

public class Theatre
{
    public Theatre()
    {
        this.Tickets = new HashSet<Ticket>();
    }

    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; } = null!;

    public sbyte NumberOfHalls { get; set; }

    public string Director { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = null!;
}