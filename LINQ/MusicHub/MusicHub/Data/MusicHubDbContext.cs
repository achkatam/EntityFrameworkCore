using Microsoft.EntityFrameworkCore.Internal;

namespace MusicHub.Data;

using Models;
using Microsoft.EntityFrameworkCore;

public class MusicHubDbContext : DbContext
{
    public MusicHubDbContext()
    {
    }

    public MusicHubDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer(Configuration.ConnectionString);
        }
    }

    public DbSet<Song> Songs { get; set; }
    public DbSet<Album> Albums { get; set; }

    public DbSet<Producer> Producers { get; set; }

    public DbSet<Performer> Performers { get; set; }

    public DbSet<Writer> Writers { get; set; }

    public DbSet<SongPerformer> SongsPerformers { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Song>(e =>
        {
            e
                .Property(s => s.CreatedOn)
                .HasColumnType("date");
        });

        builder.Entity<Album>(a =>
        {
            a
                .Property(a => a.ReleaseDate)
                .HasColumnType("date");
        });

        builder.Entity<SongPerformer>(sp =>
        {
            sp.HasKey(k => new { k.SongId, k.PerformerId });
        });
    }
}

