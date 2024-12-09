using FitnessApp.Data.Models;
using Microsoft.AspNetCore.Identity;
using static FitnessApp.Web.Infrastructure.Constants.CustomClaims;

namespace FitnessApp.Data.SeedDb
{
    internal class SeedData
    {
        public ApplicationUser InstructorUser { get; set; }
        public ApplicationUser AdminUser { get; set; }

        public IdentityUserClaim<string> InstructorUserClaim { get; set; }
        public IdentityUserClaim<string> AdminUserClaim { get; set; }

        public Instructor Instructor { get; set; }

        public Category YogaCategory { get; set; }
        public Category PilatesCategory { get; set; }
        public Category CyclingCategory { get; set; }
        public Category CardioCategory { get; set; }
        public Category MartialArtsCategory { get; set; }
        public Category HIITCategory { get; set; }
        public Category FamilyFitnessCategory { get; set; }

        public Status ActiveStatus { get; set; }
        public Status CanceledStatus { get; set; }
        public Status FinishedStatus { get; set; }
        public Status FullStatus { get; set; }

        public FitnessClass ActiveFitnessClass { get; set; }
        public FitnessClass CanceledFitnessClass { get; set; }
        public FitnessClass FinishedFitnessClass { get; set; }
        public FitnessClass FullFitnessClass { get; set; }

        public SeedData()
        {
            SeedUsers();
            SeedInstructor();
            SeedCategories();
            SeedStatuses();
            SeedFitnessClasses();
        }

        private void SeedUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();

            InstructorUser = new ApplicationUser()
            {
                Id = "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                UserName = "instructor@gmail.com",
                NormalizedUserName = "instructor@gmail.com",
                Email = "instructor@gmail.com",
                NormalizedEmail = "instructor@gmail.com",
                FirstName = "John",
                LastName = "Johnson"
            };

            InstructorUserClaim = new IdentityUserClaim<string>()
            {
                Id = 1,
                ClaimType = UserFullNameClaim,
                ClaimValue = "John Johnson",
                UserId = "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65"
            };

            InstructorUser.PasswordHash =
                hasher.HashPassword(InstructorUser, "john123");

            AdminUser = new ApplicationUser()
            {
                Id = "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                UserName = "admin@gmail.com",
                NormalizedUserName = "ADMIN@GMAIL.COM",
                Email = "admin@gmail.com",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                FirstName = "Great",
                LastName = "Admin"
            };

            AdminUserClaim = new IdentityUserClaim<string>()
            {
                Id = 2,
                ClaimType = UserFullNameClaim,
                UserId = "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                ClaimValue = "Great Admin"
            };

            AdminUser.PasswordHash =
                hasher.HashPassword(AdminUser, "admin123");
        }

        private void SeedInstructor()
        {
            Instructor = new Instructor()
            {
                Id = 1,
                UserId = InstructorUser.Id,
                Biography = "John Johnson is a dedicated fitness instructor with over a decade of experience in personal training and group fitness.Known for his motivating style and tailored workout plans, he specializes in strength training, HIIT, and functional fitness.John's mission is to help clients achieve their health goals while fostering a love for fitness. His approach combines expert guidance with an emphasis on form, endurance, and mental resilience, making him a trusted coach for all fitness levels.",
                Specializations = "Strength training, HIIT, functional fitness, and personalized programs for all fitness levels.",
                Rating = 4.8,
                LicenseNumber = 123456
            };
        }

        private void SeedCategories()
        {
            YogaCategory = new Category()
            {
                Id = 1,
                Name = "Yoga"
            };

            PilatesCategory = new Category()
            {
                Id = 2,
                Name = "Pilates"
            };

            CyclingCategory = new Category()
            {
                Id = 3,
                Name = "Cycling"
            };

            CardioCategory = new Category()
            {
                Id = 4,
                Name = "Cardio"
            };

            MartialArtsCategory = new Category()
            {
                Id = 5,
                Name = "MartialArts"
            };

            HIITCategory = new Category()
            {
                Id = 6,
                Name = "HIIT"
            };

            FamilyFitnessCategory = new Category()
            {
                Id = 7,
                Name = "FamilyFitness"
            };
        }

        private void SeedStatuses()
        {
            ActiveStatus = new Status()
            {
                Id = 1,
                Name = "Active"
            };

            CanceledStatus = new Status()
            {
                Id = 2,
                Name = "Canceled"
            };

            FinishedStatus = new Status()
            {
                Id = 3,
                Name = "Finished"
            };

            FullStatus = new Status()
            {
                Id = 4,
                Name = "Full"
            };
        }

        private void SeedFitnessClasses()
        {
            ActiveFitnessClass = new FitnessClass
            {
                Id = Guid.NewGuid(),
                Title = "Morning Yoga",
                Description = "Start your day with a relaxing yoga session to stretch and recharge.",
                CategoryId = 1,  // Yoga category
                StatusId = 1,    // Active
                InstructorId = 1,  // InstructorId = 1
                StartTime = new DateTime(2024, 12, 22, 7, 0, 0),  // 7:00 AM on Dec 10, 2024
                Duration = 60,  // 1 hour
                Capacity = 20,  // Maximum of 20 participants
                LeftCapacity = 20,  // All spots available
                IsApproved = true
            };

            CanceledFitnessClass = new FitnessClass
            {
                Id = Guid.NewGuid(),
                Title = "Pilates for Flexibility",
                Description = "Improve your flexibility with this focused Pilates class.",
                CategoryId = 2,  // Pilates category
                StatusId = 2,    // Canceled
                InstructorId = 1,  // InstructorId = 1
                StartTime = new DateTime(2024, 12, 12, 10, 0, 0),  // 10:00 AM on Dec 12, 2024
                Duration = 50,  // 50 minutes
                Capacity = 12,  // Maximum of 12 participants
                LeftCapacity = 7,  // Some spots have been taken
                IsApproved = true
            };

            FinishedFitnessClass = new FitnessClass
            {
                Id = Guid.NewGuid(),
                Title = "Zumba Dance Party",
                Description = "Join us for a fun and energetic Zumba dance workout.",
                CategoryId = 3,  // Cycling category 
                StatusId = 3,    // Finished
                InstructorId = 1,  // InstructorId = 1
                StartTime = new DateTime(2024, 12, 5, 18, 0, 0),  // 6:00 PM on Dec 5, 2024
                Duration = 60,  // 1 hour
                Capacity = 25,  // Maximum of 25 participants
                LeftCapacity = 0,  // Class is full, no spots left
                IsApproved = true
            };

            FullFitnessClass = new FitnessClass
            {
                Id = Guid.NewGuid(),
                Title = "HIIT Strength Training",
                Description = "Build strength and stamina with high-intensity interval training.",
                CategoryId = 6,  // HIIT category
                StatusId = 4,    // Full
                InstructorId = 1,  // InstructorId = 1
                StartTime = new DateTime(2024, 12, 11, 14, 30, 0),  // 2:30 PM on Dec 11, 2024
                Duration = 40,  // 40 minutes
                Capacity = 20,  // Maximum of 20 participants
                LeftCapacity = 0,  // Class is full
                IsApproved = true
            };
        }
    }
}
