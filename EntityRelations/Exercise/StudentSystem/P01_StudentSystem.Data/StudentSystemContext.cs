namespace P01_StudentSystem.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class StudentSystemContext : DbContext
    {
        public StudentSystemContext()
        {

        }

        public StudentSystemContext(DbContextOptions options)
        : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }

        public DbSet<Course> Courses { get; set; }

        public DbSet<Homework> HomeworkSubmissions { get; set; }

        public DbSet<StudentCourse> StudentCourses { get; set; }

        public DbSet<Resource> Resources { get; set; }

        // Connect to SQL Server
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Config.ConnectionString);
            }
        }

        // Define the rules, including the composite key
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>(sc =>
            {
                sc.HasKey(s => new { s.StudentId, s.CourseId });
            });
        }
    }
}
