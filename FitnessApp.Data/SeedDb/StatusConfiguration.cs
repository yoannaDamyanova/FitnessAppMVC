using FitnessApp.Data.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data.SeedDb
{
    internal class StatusConfiguration : IEntityTypeConfiguration<Status>
    {
        public void Configure(EntityTypeBuilder<Status> builder)
        {
            var data = new SeedData();

            builder.HasData(new Status[]
            {
                data.ActiveStatus,
                data.CancledStatus
            });
        }
    }
}
