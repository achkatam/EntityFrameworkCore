namespace Trucks.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Enums;

    public class Truck
    {
        public Truck()
        {
            this.ClientsTrucks = new HashSet<ClientTruck>();
        }

        [Key]
        public int Id { get; set; }

        [RegularExpression(@"^[A-Z]{2}[0-9]{4}[A-Z]{2}$")]
        public string? RegistrationNumber { get; set; }

        [StringLength(17)]
        public string VinNumber { get; set; } = null!;

        [Range(950, 1420)]
        public int TankCapacity { get; set; }

        [Range(5000, 29_000)]
        public int CargoCapacity { get; set; }

        public CategoryType CategoryType { get; set; }

        public MakeType MakeType { get; set; }

        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }

        public virtual Despatcher Despatcher { get; set; }

        public virtual ICollection<ClientTruck> ClientsTrucks { get; set; }
    }
}
