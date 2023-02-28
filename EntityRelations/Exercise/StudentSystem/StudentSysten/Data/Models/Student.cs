namespace P01_StudentSystem.Data.Models;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;
using Common;

public class Student
{
    public Student()
    {
        this.StudentsCourses = new HashSet<StudentCourse>();
        this.Homeworks = new HashSet<Homework>();
    }
    // Student
    [Key]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.StudentNameMaxLength)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "char(10)")]
    public string PhoneNumber { get; set; } = null!;

    public DateTime RegisteredOn { get; set; }

    //not required, so marked as nullable
    public DateTime? Birthday { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }
}
