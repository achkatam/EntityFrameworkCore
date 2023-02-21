namespace Infrastructure.Data
{
    using Configuration;
    using Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());

            //modelBuilder.Entity<Person>()
            //    .HasKey(e => e.Id);

            //modelBuilder.Entity<Person>()
            //    .HasComment("This is person");
        }

        public DbSet<Person> People { get; set; }

        public DbSet<Dog> Dogs { get; set; }
    }
}
