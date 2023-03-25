namespace Footballers.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Utilities;


    public class Coach
    {
        public Coach()
        {
            this.Footballers = new HashSet<Footballer>();
        }

        [Key]
        public int Id { get; set; }

        [MinLength(ValidationConstants.COACH_NAME_MIN)]
        [MaxLength(ValidationConstants.COACH_NAME_MAX)]
        public string Name { get; set; } = null!;

        public string Nationality { get; set; } = null!;

        public virtual ICollection<Footballer> Footballers { get; set; } = null!;
    }
}
