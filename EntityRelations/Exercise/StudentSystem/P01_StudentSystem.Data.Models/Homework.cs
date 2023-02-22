﻿namespace P01_StudentSystem.Data.Models
{
    using System.ComponentModel.DataAnnotations;
    using System;
    using P01_StudentSystem.Data.Models.Enums;
    using System.ComponentModel.DataAnnotations.Schema;

    //TODO: Define FK
    public class Homework
    {
        [Key]
        public int HomeworkId { get; set; }

        [Required]
        [Column(TypeName = "varchar(255)")]
        public string Content { get; set; }

        [Required]
        public ContentType ContentType { get; set; }

        public DateTime SubmissionTime { get; set; }

        [ForeignKey(nameof(Student))]
        public int StudentId { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
    }
}
