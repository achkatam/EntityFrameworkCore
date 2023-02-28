﻿namespace P01_StudentSystem.Data.Models;
 

using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

using Common;

public class Resource
{
    [Key]
    public int ResourceId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.ResourceNameMaxLength)]
    public string Name { get; set; } = null!;

    [Required]
    [Column(TypeName = "varchar(2048)")]
    public string Url { get; set; } = null!;

    public ResourceType ResourceType { get; set; }

    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    public virtual Course Course { get; set; } = null!;
}

