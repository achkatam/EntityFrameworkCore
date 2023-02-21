namespace Infrastructure.Data.Models
{
    using Microsoft.EntityFrameworkCore;
    using System.ComponentModel.DataAnnotations;

    [Comment("TestComment for the class/table")]
    public class Person
    {
        public Person()
        {
            Pets = new List<Dog>();
        }

        [Key]
        [Comment("Better be shorter than 50 chars")]
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public List<Dog> Pets { get; set; }
    }
}
