﻿namespace Boardgames.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class BoardgameSeller
{
    [Required]
    [ForeignKey(nameof(Boardgame))]
    public int BoardgameId { get; set; }
    public virtual Boardgame Boardgame { get; set; } = null!;


    [Required]
    [ForeignKey(nameof(Seller))]
    public int SellerId { get; set; }

    public virtual Seller Seller { get; set; } = null!;
}