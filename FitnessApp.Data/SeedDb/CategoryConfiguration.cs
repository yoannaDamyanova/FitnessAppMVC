using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using FitnessApp.Data.Models;

namespace FitnessApp.Data.SeedDb
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            var data = new SeedData();

            builder.HasData(new Category[]
            {
                data.CyclingCategory,
                data.YogaCategory,
                data.HIITCategory,
                data.CardioCategory,
                data.PilatesCategory,
                data.FamilyFitnessCategory,
                data.MartialArtsCategory
            });
        }
    }
}
