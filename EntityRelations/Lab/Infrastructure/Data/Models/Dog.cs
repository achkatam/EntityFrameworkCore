namespace Infrastructure.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;


    public class Dog
    {
        [Key]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int PersonId { get; set; }

        [ForeignKey(nameof(PersonId))]
        public Person Owner { get; set; }
    }
}
