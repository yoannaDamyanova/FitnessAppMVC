using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data.Models;

namespace FitnessApp.Data.SeedDb
{
    internal class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
    {
        public void Configure(EntityTypeBuilder<Instructor> builder)
        {
            var data = new SeedData();

            builder.HasData(data.Instructor);
        }
    }
}
