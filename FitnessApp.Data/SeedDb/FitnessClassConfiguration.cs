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
    internal class FitnessClassConfiguration : IEntityTypeConfiguration<FitnessClass>
    {
        public void Configure(EntityTypeBuilder<FitnessClass> builder)
        {
            var data = new SeedData();

            builder.HasData(new FitnessClass[]
            {
                data.ActiveFitnessClass,
                data.CanceledFitnessClass,
                data.FinishedFitnessClass,
                data.FullFitnessClass,
            });
        }
    }
}
