using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data;
using Moq;
using Xunit;
using FitnessApp.Data.Models;
using FitnessApp.Web.ViewModels.FitnessClass;

namespace FitnessApp.Tests
{
    using NUnit.Framework;
    using Moq;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Globalization;
    using FitnessApp.Web.Infrastructure.Enumerations;
    using Microsoft.Extensions.Logging;
    using FitnessApp.Services.Data.Contracts;

    [TestFixture]
    public class FitnessClassServiceTests
    {
        private Mock<IRepository> repositoryMock;
        private IFitnessClassService service;
        private ILogger<FitnessClassService> logger;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository>();
            service = new FitnessClassService(repositoryMock.Object);
        }

        [Test]
        public async Task AddClassAsync_Should_Add_Class_With_Correct_Data()
        {
            // Arrange
            var model = new FitnessClassFormModel
            {
                Title = "Yoga Basics",
                CategoryId = 1,
                Description = "A beginner-friendly yoga class.",
                StartTime = "15/12/2024 10:00",
                Duration = 60,
                Capacity = 20
            };
            int instructorId = 1;
            var expectedId = Guid.NewGuid();

            // Mock the AddAsync behavior to simulate setting an ID
            repositoryMock.Setup(r => r.AddAsync(It.IsAny<FitnessClass>()))
                .Callback<FitnessClass>(fc =>
                {
                    fc.Id = expectedId;
                    fc.LeftCapacity = fc.Capacity; // Simulating setting LeftCapacity
                });

            repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1); // Simulate SaveChangesAsync success

            // Act
            var result = await service.AddClassAsync(model, instructorId);

            // Assert
            Assert.AreEqual(expectedId, result);
            repositoryMock.Verify(r => r.AddAsync(It.Is<FitnessClass>(fc =>
                fc.Title == "Yoga Basics" &&
                fc.CategoryId == 1 &&
                fc.Description == "A beginner-friendly yoga class." &&
                fc.StartTime == DateTime.ParseExact("15/12/2024 10:00", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) &&
                fc.Duration == 60 &&
                fc.Capacity == 20 &&
                fc.LeftCapacity == 20 &&
                fc.InstructorId == instructorId &&
                fc.IsApproved == false
            )), Times.Once);
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AllAsync_ShouldFilterByCategory()
        {
            // Arrange
            var category = "Yoga";
            var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Morning Yoga",
                    Description = "A refreshing yoga session to start your day.",
                    CategoryId = 1,
                    StatusId = 1, // Active
                    InstructorId = 1,
                    StartTime = DateTime.Now.AddMinutes(45),
                    Duration = 60, // 1 hour
                    Capacity = 20,
                    LeftCapacity = 15,
                    IsApproved = true,
                    // Navigation properties
                    Category = new Category { Id = 1, Name = "Yoga" },
                    Status = new Status { Id = 1, Name = "Active" },
                    Instructor = new Instructor
                    {
                        Id = 1,
                        User = new ApplicationUser { FirstName = "John", LastName = "Doe" }
                    }
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Evening Pilates",
                    Description = "A relaxing pilates session to unwind your day.",
                    CategoryId = 2,
                    StatusId = 2, // Canceled
                    InstructorId = 2,
                    StartTime = DateTime.Now.AddDays(2),
                    Duration = 90, // 1.5 hours
                    Capacity = 25,
                    LeftCapacity = 0,
                    IsApproved = true,
                    // Navigation properties
                    Category = new Category { Id = 2, Name = "Pilates" },
                    Status = new Status { Id = 2, Name = "Canceled" },
                    Instructor = new Instructor
                    {
                        Id = 2,
                        User = new ApplicationUser { FirstName = "Jane", LastName = "Smith" }
                    }
                }
            }.AsQueryable();

            repositoryMock
                .Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            // Act
            var result = await service.AllAsync(category: category);

            // Assert
            Assert.AreEqual(1, result.TotalClassesCount);
            Assert.AreEqual("Morning Yoga", result.FitnessClasses.First().Title);
        }

        [Test]
        public async Task AllBookedByUserId_ValidUser_ReturnsBookedClasses()
        {
            // Arrange
            var userId = Guid.NewGuid();

            var users = new List<ApplicationUser>
            {
                new ApplicationUser
                {
                    Id = userId.ToString(),
                    FirstName = "Jane",
                    LastName = "Doe"
                }
            };

                    var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Morning Yoga",
                    StartTime = new DateTime(2023, 01, 01, 08, 00, 00),
                    Duration = 60,
                    Capacity = 20,
                    LeftCapacity = 15,
                    Status = new Status { Id = 1, Name = "Active" },
                    StatusId = 1,
                    Instructor = new Instructor
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "John",
                            LastName = "Smith"
                        }
                    }
                }
            };

            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    User = users.First(),
                    UserId = userId.ToString(),
                    FitnessClassId = fitnessClasses.First().Id,
                    FitnessClass = fitnessClasses.First(),
                    BookingDate = DateTime.Now
                }
            };

            repositoryMock
                .Setup(r => r.GetByIdAsync<ApplicationUser>(userId.ToString()))
                .ReturnsAsync(new ApplicationUser
                {
                    Id = userId.ToString(),
                    FirstName = "Jane",
                    LastName = "Doe"
                });

            repositoryMock
                .Setup(r => r.AllReadOnly<Booking>())
                .Returns(bookings.AsQueryable());

            // Act
            var result = await service.AllBookedByUserId(userId.ToString());

            // Assert
            Assert.AreEqual(1, result.Count());
            var bookedClass = result.First();
            Assert.AreEqual(fitnessClasses.First().Title, bookedClass.Title);
            Assert.AreEqual(fitnessClasses.First().Status.Name, bookedClass.Status);
            Assert.AreEqual("John Smith", bookedClass.InstructorFullName);
            Assert.AreEqual("01/01/2023 08:00", bookedClass.StartTime);
        }

    }
}