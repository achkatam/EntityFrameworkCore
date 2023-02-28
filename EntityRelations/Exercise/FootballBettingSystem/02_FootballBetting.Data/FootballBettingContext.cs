using P02_FootballBetting.Data.Models;

namespace P02_FootballBetting.Data;

using Microsoft.EntityFrameworkCore;

public class FootballBettingContext : DbContext
{
    public FootballBettingContext()
    {

    }

    public FootballBettingContext(DbContextOptions options)
    : base() { }

    public DbSet<Team> Teams { get; set; }

    public DbSet<Color> Colors { get; set; }

    public DbSet<Town> Towns { get; set; }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Player> Players { get; set; }

    public DbSet<Position> Positions { get; set; }

    public DbSet<PlayerStatistic> PlayersStatistics { get; set; }

    public DbSet<Game> Games { get; set; }

    public DbSet<Bet> Bets { get; set; }

    public DbSet<User> Users { get; set; }
    // Connect to the SQL Server
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(Config.ConnectionString);
        }

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO: Fluent API
        modelBuilder
            .Entity<PlayerStatistic>(e =>
            {
                // The Composite Key
                e.HasKey(ps => new { ps.PlayerId, ps.GameId });
            });

        modelBuilder.Entity<Team>(t =>
        {
            t
                .HasOne(e => e.PrimaryKitColor)
                .WithMany(c => c.PrimaryKitTeams)
                .HasForeignKey(e => e.PrimaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);

            t
                .HasOne(st => st.SecondaryKitColor)
                .WithMany(c => c.SecondaryKitTeams)
                .HasForeignKey(st=> st.SecondaryKitColorId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity
                .HasOne(g => g.HomeTeam)
                .WithMany(t => t.HomeGames)
                .HasForeignKey(g => g.HomeTeamId)
                .OnDelete(DeleteBehavior.NoAction);

            entity
                .HasOne(g => g.AwayTeam)
                .WithMany(e => e.AwayGames)
                .HasForeignKey(g => g.AwayTeamId)
                .OnDelete(DeleteBehavior.NoAction);
        });

    }
}

