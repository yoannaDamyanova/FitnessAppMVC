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

        public FitnessAppDbContext(DbContextOptions<FitnessAppDbContext> options)
        : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new InstructorConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserClaimsConfiguration());
            modelBuilder.ApplyConfiguration(new StatusConfiguration());

            modelBuilder.Entity<Booking>()
                .HasIndex(b => new { b.UserId, b.FitnessClassId })
                .IsUnique();

            modelBuilder.Entity<FitnessClass>()
                .HasOne(c => c.Status)
                .WithMany()
                .HasForeignKey(c=>c.StatusId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FitnessClass>()
                .Property(c => c.StatusId)
                .HasDefaultValue(1);

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<FitnessClass> Classes { get; set; } = null!;

        public virtual DbSet<Booking> Bookings { get; set; } = null!;

        public virtual DbSet<Category> Categories { get; set; } = null!;

        public virtual DbSet<Review> Reviews { get; set; } = null!;
        public virtual DbSet<Instructor> Instructors { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null !;
    }
}
