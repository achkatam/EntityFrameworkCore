namespace P02_FootballBetting.Data.Models;

using System.ComponentModel.DataAnnotations.Schema;

using Common;
public class PlayerStatistic
{

    [ForeignKey(nameof(Game))]
    public int GameId { get; set; }
    public virtual Game Game { get; set; } = null!;

    [ForeignKey(nameof(Player))]
    public int PlayerId { get; set; }
    public virtual Player Player { get; set; } = null!;

    public int ScoredGoals { get; set; }

    public int MinutesPlayed { get; set; }

    public int Assists { get; set; }
}

