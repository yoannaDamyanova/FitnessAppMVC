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
        public Status CancledStatus { get; set; }

        public SeedData()
        {
            SeedUsers();
            SeedInstructor();
            SeedCategories();
            SeedStatuses();
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

            CancledStatus = new Status()
            {
                Id = 2,
                Name = "Cancled"
            };
        }
    }
}
