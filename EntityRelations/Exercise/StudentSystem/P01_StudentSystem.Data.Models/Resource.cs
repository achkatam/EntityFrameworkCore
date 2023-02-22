
namespace P01_StudentSystem.Data.Models
{

    using System.ComponentModel.DataAnnotations;
    using Enums;
    using P01_StudentSystem_.Data.Common;
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Resource
    {
        //TODO: Define the FK
        [Key]
        public int ResourceId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.ResourceNameMaxLenght)]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "varchar(2048)")]
        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }


        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
