using FitnessApp.Data.Models;
using FitnessApp.Data.SeedDb;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Data
{
    public class FitnessAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public FitnessAppDbContext()
        {

        }

        public FitnessAppDbContext(DbContextOptions options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new InstructorConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimsConfiguration());
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<FitnessClass> Classes { get; set; } = null!;

        public virtual DbSet<Booking> Bookings { get; set; } = null!;

        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Review> Reviews { get; set; } = null!;

        public virtual DbSet<Status> Statuses { get; set; } = null!;

        public virtual DbSet<Instructor> Instructors { get; set; } 
    }
}
