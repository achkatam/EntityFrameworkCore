﻿namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

using Common;
public class Town
{
    public Town()
    {
        this.Teams = new HashSet<Team>();
    }
    [Key]
    public int TownId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.TownNameMaxLength)]
    public string Name { get; set; } = null!;

    [ForeignKey(nameof(Country))]
    public int CountryId { get; set; }
    public virtual Country Country { get; set; } = null!;
     
    public virtual ICollection<Team> Teams { get; set; }
}