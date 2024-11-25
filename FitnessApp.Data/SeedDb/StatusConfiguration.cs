using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitnessApp.Data.Models;

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
                data.CanceledStatus,
                data.FinishedStatus,
                data.FullStatus,
            });
        }
    }
}
