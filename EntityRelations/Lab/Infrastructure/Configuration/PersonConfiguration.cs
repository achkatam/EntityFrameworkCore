namespace Infrastructure.Configuration
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    internal class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.HasComment("This is person");
            builder.HasKey(e => e.Id);
            builder.Property(x => x.Name)
                .HasComment("Person's name");
        }
    }
}
