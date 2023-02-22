namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using P01_StudentSystem_.Data.Common;
    using System.ComponentModel.DataAnnotations.Schema;
    using System;
    using System.Collections.Generic;

    public class Student
    {
        public Student()
        {
            this.CourseEnrollments = new HashSet<StudentCourse>();
            this.HomeworkSubmissions = new HashSet<Homework>();
        }
        [Key]
        public int StudentId { get; set; }

        [Required]
        [MaxLength(GlobalConstants.StudentNameMaxLength)]
        public string Name { get; set; }

        [Column(TypeName = "char(10)")]
        public string PhoneNumber { get; set; }

        public DateTime RegisteredOn { get; set; }

        public DateTime? Birthday { get; set; }

        public virtual ICollection<StudentCourse> CourseEnrollments { get; set; }

        public virtual ICollection<Homework> HomeworkSubmissions { get; set; }
    }
}
