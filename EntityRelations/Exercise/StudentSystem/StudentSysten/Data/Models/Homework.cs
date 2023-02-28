namespace P01_StudentSystem.Data.Models;


using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

public class Homework
{
    [Key]
    public int HomeworkId { get; set; }

    [Required]
    [Column(TypeName = "varchar(255)")]
    public string Content { get; set; } = null!;

    public virtual ContentType ContentType { get; set; }

    public DateTime SubmissionTime { get; set; }

    [ForeignKey(nameof(Student))]
    public int StudentId { get; set; }
    public virtual Student Student { get; set; } = null!;

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
}