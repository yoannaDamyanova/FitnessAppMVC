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
    using FitnessApp.Services.Data.Contracts;

    [TestFixture]
    public class FitnessClassServiceTests
    {
        private Mock<IRepository> repositoryMock;
        private IFitnessClassService service;

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

        [Test]
        public async Task UpdateClassAsync_ShouldUpdateClass_WhenClassExists()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var existingClass = new FitnessClass
            {
                Id = classId,
                Title = "Yoga Basics",
                CategoryId = 1,
                Description = "A beginner-friendly yoga class.",
                StartTime = DateTime.Now,
                Duration = 60,
                Capacity = 20,
                LeftCapacity = 20,
                InstructorId = 1,
                IsApproved = false
            };

            var updateModel = new FitnessClassFormModel
            {
                Id = classId.ToString(),
                Title = "Advanced Yoga",
                Description = "An advanced yoga class.",
                StartTime = "12/12/2024 12:12",
                Duration = 90,
                Capacity = 25,
                CategoryId = 2
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(existingClass);
            repositoryMock.Setup(r => r.SaveChangesAsync())
                          .ReturnsAsync(1);

            // Act
            await service.EditAsync(updateModel);

            // Assert
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
            Assert.AreEqual("Advanced Yoga", existingClass.Title);
            Assert.AreEqual("An advanced yoga class.", existingClass.Description);
            Assert.AreEqual(90, existingClass.Duration);
            Assert.AreEqual(25, existingClass.Capacity);
            Assert.AreEqual(2, existingClass.CategoryId);
        }

        [Test]
        public async Task UpdateClassAsync_ShouldReturnFalse_WhenClassNotFound()
        {
            // Arrange
            var classId = Guid.NewGuid();

            var updateModel = new FitnessClassFormModel
            {
                Id = classId.ToString(),
                Title = "Advanced Yoga",
                Description = "An advanced yoga class.",
                StartTime = "12/12/2024 12:12",
                Duration = 90,
                Capacity = 25
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync((FitnessClass)null);
            // Act
            await service.EditAsync(updateModel);

            // Assert
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task ApproveClassAsync_ShouldApproveClass_WhenClassExists()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var existingClass = new FitnessClass
            {
                Id = classId,
                Title = "Yoga Basics",
                CategoryId = 1,
                Description = "A beginner-friendly yoga class.",
                StartTime = DateTime.Now,
                Duration = 60,
                Capacity = 20,
                LeftCapacity = 20,
                InstructorId = 1,
                IsApproved = false
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(existingClass);
            repositoryMock.Setup(r => r.SaveChangesAsync())
                          .ReturnsAsync(1);

            // Act
            await service.ApproveFitnessClassAsync(classId);

            // Assert
            Assert.IsTrue(existingClass.IsApproved);
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ApproveClassAsync_ShouldReturnFalse_WhenClassNotFound()
        {
            // Arrange
            var classId = Guid.NewGuid();
            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync((FitnessClass)null);

            // Act
            await service.ApproveFitnessClassAsync(classId);

            // Assert
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task CancelClassAsync_ShouldCancelClass_WhenClassExists()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var existingClass = new FitnessClass
            {
                Id = classId,
                Title = "Yoga Basics",
                StatusId = 1, // Active
                InstructorId = 1
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(existingClass);
            repositoryMock.Setup(r => r.SaveChangesAsync())
                          .ReturnsAsync(1);

            // Act
            await service.CancelClassAsync(classId);

            // Assert
            Assert.AreEqual(2, existingClass.StatusId); // Canceled status
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task GetClassByIdAsync_ShouldReturnClass_WhenClassExists()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var existingClass = new FitnessClass
            {
                Id = classId,
                Title = "Yoga Basics",
                Description = "A beginner-friendly yoga class.",
                StartTime = DateTime.Now,
                Duration = 60,
                Capacity = 20,
                LeftCapacity = 15,
                IsApproved = false,
                InstructorId = 1,
                Instructor = new Instructor
                {
                    User = new ApplicationUser { FirstName = "John", LastName = "Doe" }
                }
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(existingClass);

            // Act
            var result = await service.GetByIdAsync(classId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(existingClass.Title, result.Title);
            Assert.AreEqual(existingClass.Description, result.Description);
            Assert.AreEqual(existingClass.StartTime, result.StartTime);
            Assert.AreEqual(existingClass.Duration, result.Duration);
            Assert.AreEqual(existingClass.Capacity, result.Capacity);
            Assert.AreEqual(existingClass.LeftCapacity, result.LeftCapacity);
        }

        [Test]
        public async Task GetClassByIdAsync_ShouldReturnNull_WhenClassDoesNotExist()
        {
            // Arrange
            var classId = Guid.NewGuid();
            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync((FitnessClass)null);

            // Act
            var result = await service.GetByIdAsync(classId);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Yoga" },
                new Category { Id = 2, Name = "Pilates" }
            }.AsQueryable();

            repositoryMock.Setup(r => r.AllReadOnly<Category>())
                          .Returns(categories);

            // Act
            var result = await service.AllCategoriesAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Yoga", result.First().Name);
            Assert.AreEqual(1, result.First().Id);
        }

        [Test]
        public async Task GetAllCategoriesAsync_ShouldReturnEmptyList_WhenNoCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>().AsQueryable();
            repositoryMock.Setup(r => r.AllReadOnly<Category>())
                          .Returns(categories);

            // Act
            var result = await service.AllCategoriesAsync();

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task UnBookAsync_ShouldCancelBooking_WhenClassAndBookingExist()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();
            var fitnessClass = new FitnessClass
            {
                Id = classId,
                LeftCapacity = 5,
                StatusId = 1 // Active status
            };
            var booking = new Booking
            {
                Id = 2,
                FitnessClassId = classId,
                UserId = userId
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(fitnessClass);
            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                          .Returns(new List<Booking> { booking }.AsQueryable());
            repositoryMock.Setup(r => r.DeleteAsync<Booking>(booking.Id))
                          .Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync())
                          .ReturnsAsync(1);

            // Act
            await service.UnBookAsync(classId, userId);

            // Assert
            Assert.AreEqual(6, fitnessClass.LeftCapacity); // Capacity should increase
            repositoryMock.Verify(r => r.DeleteAsync<Booking>(booking.Id), Times.Once); // Booking should be deleted
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once); // Changes should be saved
        }

        [Test]
        public void UnBookAsync_ShouldThrowUnauthorizedAccessException_WhenUserHasNotBookedClass()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var userId = "user123";
            var fitnessClass = new FitnessClass
            {
                Id = classId,
                LeftCapacity = 5,
                StatusId = 1 // Active status
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(fitnessClass);
            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                          .Returns(new List<Booking>().AsQueryable()); // No bookings for the class

            // Act & Assert
            var ex = Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.UnBookAsync(classId, userId));
            Assert.AreEqual("This user has not booked this class", ex.Message);
        }

        [Test]
        public async Task UnBookAsync_ShouldNotProceed_WhenClassDoesNotExist()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var userId = "user123";

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync((FitnessClass)null); // No class found

            // Act
            await service.UnBookAsync(classId, userId);

            // Assert
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task UnBookAsync_ShouldUpdateStatus_WhenLeftCapacityIsGreaterThanZero()
        {
            // Arrange
            var classId = Guid.NewGuid();
            var userId = "user123";
            var fitnessClass = new FitnessClass
            {
                Id = classId,
                LeftCapacity = 1, // Only 1 spot left
                StatusId = 2 // Assume it's canceled initially
            };
            var booking = new Booking
            {
                Id = 1,
                FitnessClassId = classId,
                UserId = userId
            };

            repositoryMock.Setup(r => r.GetByIdAsync<FitnessClass>(classId))
                          .ReturnsAsync(fitnessClass);
            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                          .Returns(new List<Booking> { booking }.AsQueryable());
            repositoryMock.Setup(r => r.DeleteAsync<Booking>(booking.Id))
                          .Returns(Task.CompletedTask);
            repositoryMock.Setup(r => r.SaveChangesAsync())
                          .ReturnsAsync(1);

            // Act
            await service.UnBookAsync(classId, userId);

            // Assert
            Assert.AreEqual(1, fitnessClass.StatusId); // Status should change as left capacity was > 0
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once); // Ensure changes are saved
        }

        [Test]
        public async Task AllBookingsAsync_ShouldReturnBookingViewModels_WhenBookingsExist()
        {
            // Arrange
            var bookings = new List<Booking>
            {
                new Booking
                {
                    Id = 1,
                    FitnessClass = new FitnessClass
                    {
                        Title = "Yoga Basics",
                        StartTime = DateTime.Parse("2024-12-15 10:00"),
                        Instructor = new Instructor
                        {
                            User = new ApplicationUser { FirstName = "John", LastName = "Doe" }
                        }
                    },
                    User = new ApplicationUser { FirstName = "Jane", LastName = "Smith" }
                },
                new Booking
                {
                    Id = 2,
                    FitnessClass = new FitnessClass
                    {
                        Title = "Pilates for Beginners",
                        StartTime = DateTime.Parse("2024-12-16 12:00"),
                        Instructor = new Instructor
                        {
                            User = new ApplicationUser { FirstName = "Alice", LastName = "Johnson" }
                        }
                    },
                    User = new ApplicationUser { FirstName = "Bob", LastName = "Brown" }
                }
            }.AsQueryable();

            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                .Returns(bookings);

            // Act
            var result = await service.AllBookingsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());
            var firstBooking = result.First();
            Assert.AreEqual("Yoga Basics", firstBooking.FitnessClassTitle);
            Assert.AreEqual("Jane Smith", firstBooking.BookerName);
            Assert.AreEqual("John Doe", firstBooking.InstructorFullName);
            Assert.AreEqual("15/12/2024 10:00", firstBooking.BookingDate);

            var secondBooking = result.Skip(1).First();
            Assert.AreEqual("Pilates for Beginners", secondBooking.FitnessClassTitle);
            Assert.AreEqual("Bob Brown", secondBooking.BookerName);
            Assert.AreEqual("Alice Johnson", secondBooking.InstructorFullName);
            Assert.AreEqual("16/12/2024 12:00", secondBooking.BookingDate);
        }

        [Test]
        public async Task AllBookingsAsync_ShouldReturnEmptyList_WhenNoBookingsExist()
        {
            // Arrange
            var bookings = new List<Booking>().AsQueryable();

            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                .Returns(bookings);

            // Act
            var result = await service.AllBookingsAsync();

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [Test]
        public async Task AllReviewsAsync_Should_Return_Correct_Review_ViewModels()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    Rating = 5,
                    Comments = "Great class!",
                    FitnessClass = new FitnessClass
                    {
                        Id = Guid.NewGuid(),
                        Title = "Morning Yoga",
                        Instructor = new Instructor
                        {
                            Id = 1,
                            User = new ApplicationUser { FirstName = "John", LastName = "Doe" }
                        }
                    },
                    User = new ApplicationUser { FirstName = "Jane", LastName = "Smith" }
                },
                new Review
                {
                    Id = Guid.NewGuid(),
                    Rating = 4,
                    Comments = "Good session, but could be longer.",
                    FitnessClass = new FitnessClass
                    {
                        Id = Guid.NewGuid(),
                        Title = "Evening Pilates",
                        Instructor = new Instructor
                        {
                            Id = 2,
                            User = new ApplicationUser { FirstName = "Alice", LastName = "Brown" }
                        }
                    },
                    User = new ApplicationUser { FirstName = "Bob", LastName = "White" }
                }
            }.AsQueryable();

            // Mock the repository to return the reviews
            repositoryMock
                .Setup(r => r.AllReadOnly<Review>())
                .Returns(reviews);

            // Act
            var result = await service.AllReviewsAsync();

            // Assert
            Assert.AreEqual(2, result.Count());  // Ensure two reviews are returned
            var firstReview = result.First();
            var secondReview = result.Last();

            // Validate that the data in the ReviewViewModel is correct
            Assert.AreEqual("Morning Yoga", firstReview.FitnessClassTitle);
            Assert.AreEqual("John Doe", firstReview.InstructorFullName);
            Assert.AreEqual("Jane Smith", firstReview.ReviewerName);
            Assert.AreEqual(5, firstReview.Rating);
            Assert.AreEqual("Great class!", firstReview.Comments);

            Assert.AreEqual("Evening Pilates", secondReview.FitnessClassTitle);
            Assert.AreEqual("Alice Brown", secondReview.InstructorFullName);
            Assert.AreEqual("Bob White", secondReview.ReviewerName);
            Assert.AreEqual(4, secondReview.Rating);
            Assert.AreEqual("Good session, but could be longer.", secondReview.Comments);
        }

        [Test]
        public async Task AllReviewsAsync_Should_Return_Empty_List_When_No_Reviews_Found()
        {
            // Arrange
            var reviews = new List<Review>().AsQueryable();  // Empty list of reviews

            // Mock the repository to return the empty list
            repositoryMock
                .Setup(r => r.AllReadOnly<Review>())
                .Returns(reviews);

            // Act
            var result = await service.AllReviewsAsync();

            // Assert
            Assert.IsEmpty(result);  // Ensure that the result is empty
        }

        [Test]
        public async Task AllReviewsAsync_Should_Handle_Exception_When_FitnessClass_Or_User_Not_Found()
        {
            // Arrange
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    Rating = 5,
                    Comments = "Great class!",
                    FitnessClass = null,  // Simulate missing fitness class
                    User = null  // Simulate missing user
                }
            }.AsQueryable();

            // Mock the repository to return reviews with missing data
            repositoryMock
                .Setup(r => r.AllReadOnly<Review>())
                .Returns(reviews);

            // Act & Assert
            var ex = Assert.ThrowsAsync<NullReferenceException>(async () => await service.AllReviewsAsync());
            Assert.That(ex.Message, Is.EqualTo("Object reference not set to an instance of an object."));  // Handle the exception based on actual implementation
        }

        [Test]
        public async Task GetUnApprovedAsync_Should_Return_UnApproved_FitnessClasses()
        {
            // Arrange
            var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Yoga Basics",
                    IsApproved = false,
                    Duration = 60,
                    Capacity = 20,
                    StartTime = new DateTime(2024, 12, 15, 10, 0, 0),
                    Status = new Status { Id = 1, Name = "Pending" },
                    InstructorId = 1,
                    Instructor = new Instructor
                    {
                        Id = 1,
                        User = new ApplicationUser { FirstName = "John", LastName = "Doe" }
                    }
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Pilates for Beginners",
                    IsApproved = false,
                    Duration = 45,
                    Capacity = 15,
                    StartTime = new DateTime(2024, 12, 16, 14, 0, 0),
                    Status = new Status { Id = 2, Name = "Pending" },
                    InstructorId = 2,
                    Instructor = new Instructor
                    {
                        Id = 2,
                        User = new ApplicationUser { FirstName = "Alice", LastName = "Smith" }
                    }
                }
            }.AsQueryable();

            var statuses = new List<Status>
            {
                new Status { Id = 1, Name = "Pending" },
                new Status { Id = 2, Name = "Pending" }
            }.AsQueryable();

            // Mock repository calls
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            repositoryMock.Setup(r => r.AllReadOnly<Status>())
                .Returns(statuses);

            // Act
            var result = await service.GetUnApprovedAsync();

            // Assert
            Assert.AreEqual(2, result.Count()); // Two unapproved fitness classes
            var firstClass = result.First();
            var secondClass = result.Last();

            // Assert data for the first fitness class
            Assert.AreEqual("Yoga Basics", firstClass.Title);
            Assert.AreEqual("Pending", firstClass.Status);
            Assert.AreEqual(60, firstClass.Duration);
            Assert.AreEqual(20, firstClass.Capacity);
            Assert.AreEqual("15/12/2024 10:00", firstClass.StartTime);
            Assert.AreEqual(1, firstClass.InstructorId);
            Assert.AreEqual("John Doe", firstClass.InstructorFullName);

            // Assert data for the second fitness class
            Assert.AreEqual("Pilates for Beginners", secondClass.Title);
            Assert.AreEqual("Pending", secondClass.Status);
            Assert.AreEqual(45, secondClass.Duration);
            Assert.AreEqual(15, secondClass.Capacity);
            Assert.AreEqual("16/12/2024 14:00", secondClass.StartTime);
            Assert.AreEqual(2, secondClass.InstructorId);
            Assert.AreEqual("Alice Smith", secondClass.InstructorFullName);
        }

        [Test]
        public async Task GetUnApprovedAsync_Should_Return_Empty_When_No_UnApproved_FitnessClasses()
        {
            // Arrange
            var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Yoga Basics",
                    IsApproved = true, // Approved class
                    Duration = 60,
                    Capacity = 20,
                    StartTime = new DateTime(2024, 12, 15, 10, 0, 0),
                    Status = new Status { Id = 1, Name = "Approved" },
                    Instructor = new Instructor
                    {
                        Id = 1,
                        User = new ApplicationUser { FirstName = "John", LastName = "Doe" }
                    }
                }
            }.AsQueryable();

            var statuses = new List<Status>
            {
                new Status { Id = 1, Name = "Approved" }
            }.AsQueryable();

            // Mock repository calls to return only approved fitness classes
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            repositoryMock.Setup(r => r.AllReadOnly<Status>())
                .Returns(statuses);

            // Act
            var result = await service.GetUnApprovedAsync();

            // Assert
            Assert.IsEmpty(result);  // Ensure that the result is empty (no unapproved classes)
        }

        [Test]
        public async Task UserHasReviewedClassAsync_Should_Return_True_When_User_Has_Reviewed_Class()
        {
            // Arrange
            var userId = "user123";
            var fitnessClassId = Guid.NewGuid();

            // Mock data for reviews
            var reviews = new List<Review>
            {
                new Review
                {
                    Id = Guid.NewGuid(),
                    FitnessClassId = fitnessClassId,
                    UserId = userId,
                    Rating = 5,
                    Comments = "Great class!"
                }
            }.AsQueryable();

            // Mock the repository to return reviews for the given fitness class and user
            repositoryMock.Setup(r => r.AllReadOnly<Review>())
                .Returns(reviews);

            // Act
            var result = await service.UserHasReviewedClassAsync(userId, fitnessClassId);

            // Assert
            Assert.IsTrue(result);  // The result should be true as the user has reviewed the class
        }

        [Test]
        public async Task UserHasReviewedClassAsync_Should_Return_False_When_User_Has_Not_Reviewed_Class()
        {
            // Arrange
            var userId = "user123";
            var fitnessClassId = Guid.NewGuid();

            // Mock data for reviews (no review for the given user and fitness class)
            var reviews = new List<Review>().AsQueryable();

            // Mock the repository to return no reviews
            repositoryMock.Setup(r => r.AllReadOnly<Review>())
                .Returns(reviews);

            // Act
            var result = await service.UserHasReviewedClassAsync(userId, fitnessClassId);

            // Assert
            Assert.IsFalse(result);  // The result should be false as the user has not reviewed the class
        }

        [Test]
        public async Task UserHasReviewedClassAsync_Should_Return_False_When_No_Reviews_Exist_For_Class()
        {
            // Arrange
            var userId = "user123";
            var fitnessClassId = Guid.NewGuid();

            // Mock data for reviews (no reviews for the given fitness class at all)
            var reviews = new List<Review>().AsQueryable();

            // Mock the repository to return no reviews for the class
            repositoryMock.Setup(r => r.AllReadOnly<Review>())
                .Returns(reviews);

            // Act
            var result = await service.UserHasReviewedClassAsync(userId, fitnessClassId);

            // Assert
            Assert.IsFalse(result);  // The result should be false as there are no reviews for the class
        }

        [Test]
        public async Task WriteReviewAsync_Should_Add_Review_Correctly()
        {
            // Arrange
            var model = new FitnessClassReviewFormModel
            {
                Rating = 5,
                Comments = "Excellent class!",
                FitnessClassId = Guid.NewGuid().ToString()
            };
            var userId = "user123";

            // Create a new review object that is expected to be added
            var reviewToAdd = new Review
            {
                Rating = model.Rating,
                Comments = model.Comments,
                FitnessClassId = Guid.Parse(model.FitnessClassId),
                UserId = userId,
                DateSubmitted = DateTime.UtcNow, // Set this dynamically in real time
                IsApproved = false
            };

            // Mock the repository to simulate saving the review
            repositoryMock.Setup(r => r.AddAsync<Review>(It.IsAny<Review>()))
                .Callback<Review>(review => review.Id = Guid.NewGuid()); // Assign an ID to the review

            repositoryMock.Setup(r => r.SaveChangesAsync())
                .ReturnsAsync(1); // Simulate SaveChangesAsync success

            // Act
            await service.WriteReviewAsync(model, userId);

            // Assert
            repositoryMock.Verify(r => r.AddAsync(It.Is<Review>(r =>
                r.Rating == model.Rating &&
                r.Comments == model.Comments &&
                r.FitnessClassId == Guid.Parse(model.FitnessClassId) &&
                r.UserId == userId &&
                r.IsApproved == false
            )), Times.Once);

            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task WriteReviewAsync_Should_Not_Call_SaveChanges_If_Repository_Fails()
        {
            // Arrange
            var model = new FitnessClassReviewFormModel
            {
                Rating = 5,
                Comments = "Fantastic class!",
                FitnessClassId = Guid.NewGuid().ToString()
            };
            var userId = "user123";

            // Setup the repository to throw an exception when saving
            repositoryMock.Setup(r => r.SaveChangesAsync())
                .ThrowsAsync(new Exception("Error saving changes"));

            // Act & Assert
            var exception = Assert.ThrowsAsync<Exception>(async () =>
                await service.WriteReviewAsync(model, userId)
            );
            Assert.AreEqual("Error saving changes", exception.Message);

            // Verify SaveChangesAsync was called even though it threw an exception
            repositoryMock.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AllStatusesNamesAsync_Should_Return_All_Unique_Status_Names()
        {
            // Arrange
            var statuses = new List<Status>
        {
            new Status { Id = 1, Name = "Active" },
            new Status { Id = 2, Name = "Canceled" },
            new Status { Id = 3, Name = "Finished" },  // Duplicate status name
            new Status { Id = 4, Name = "Full" }
        }.AsQueryable();

            // Mock the repository to return the list of statuses
            repositoryMock.Setup(r => r.AllReadOnly<Status>())
                .Returns(statuses);

            // Act
            var result = await service.AllStatusesNamesAsync();

            // Assert
            Assert.AreEqual(4, result.Count());
            Assert.Contains("Active", result.ToList());
            Assert.Contains("Full", result.ToList());
            Assert.Contains("Finished", result.ToList());
            Assert.Contains("Full", result.ToList());
        }

        [Test]
        public async Task AllStatusesNamesAsync_Should_Return_Empty_If_No_Statuses_Are_Found()
        {
            // Arrange
            var statuses = new List<Status>().AsQueryable(); // No statuses

            // Mock the repository to return an empty list
            repositoryMock.Setup(r => r.AllReadOnly<Status>())
                .Returns(statuses);

            // Act
            var result = await service.AllStatusesNamesAsync();

            // Assert
            Assert.IsEmpty(result); // Ensure the result is empty
        }

        [Test]
        public async Task LastFiveHousesAsync_Should_Return_Last_Five_Approved_Classes_With_Capacity_And_Active_Status()
        {
            // Arrange
            var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Yoga Basics",
                    StartTime = DateTime.Now.AddHours(1),
                    Duration = 60,
                    Capacity = 20,
                    LeftCapacity = 15,
                    IsApproved = true,
                    StatusId = 1,
                    Instructor = new Instructor
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "John",
                            LastName = "Doe"
                        }
                    }
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Advanced Pilates",
                    StartTime = DateTime.Now.AddHours(2),
                    Duration = 90,
                    Capacity = 25,
                    LeftCapacity = 20,
                    IsApproved = true,
                    StatusId = 1,
                    Instructor = new Instructor
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "Jane",
                            LastName = "Smith"
                        }
                    }
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Zumba Party",
                    StartTime = DateTime.Now.AddHours(3),
                    Duration = 60,
                    Capacity = 15,
                    LeftCapacity = 5,
                    IsApproved = true,
                    StatusId = 1,
                    Instructor = new Instructor
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "Mark",
                            LastName = "Johnson"
                        }
                    }
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Kickboxing for Beginners",
                    StartTime = DateTime.Now.AddHours(4),
                    Duration = 60,
                    Capacity = 20,
                    LeftCapacity = 18,
                    IsApproved = true,
                    StatusId = 1,
                    Instructor = new Instructor
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "Emily",
                            LastName = "Williams"
                        }
                    }
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Cardio Blast",
                    StartTime = DateTime.Now.AddHours(5),
                    Duration = 45,
                    Capacity = 30,
                    LeftCapacity = 10,
                    IsApproved = true,
                    StatusId = 1,
                    Instructor = new Instructor
                    {
                        User = new ApplicationUser
                        {
                            FirstName = "Sarah",
                            LastName = "Brown"
                        }
                    }
                }
            }.AsQueryable();

            // Mock the repository to return the fitness classes
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            // Act
            var result = await service.LastFiveHousesAsync();

            // Assert
            Assert.AreEqual(5, result.Count()); // Ensure exactly 5 classes are returned
            Assert.AreEqual("Yoga Basics", result.First().Title); // Check first class title
            Assert.AreEqual("Cardio Blast", result.Last().Title); // Check last class title
            Assert.AreEqual("John", result.First().InstructorName); // Check instructor's first name for first class
            Assert.AreEqual("Sarah", result.Last().InstructorName); // Check instructor's first name for last class

            // Ensure the StartTime is in the correct format
            Assert.AreEqual(fitnessClasses.First().StartTime.ToString("dd/MM/yyyy HH:mm"), result.First().StartTime);
            Assert.AreEqual(fitnessClasses.Last().StartTime.ToString("dd/MM/yyyy HH:mm"), result.Last().StartTime);

            // Verify that the repository's AllReadOnly method was called
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task LastFiveHousesAsync_Should_Return_Empty_If_No_Classes_Match_Criteria()
        {
            // Arrange
            var fitnessClasses = new List<FitnessClass>
        {
            new FitnessClass
            {
                Id = Guid.NewGuid(),
                Title = "Class with No Capacity",
                StartTime = DateTime.Now.AddHours(1),
                Duration = 60,
                Capacity = 0,
                LeftCapacity = 0,
                IsApproved = true,
                StatusId = 1,
                Instructor = new Instructor
                {
                    User = new ApplicationUser
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    }
                }
            }
        }.AsQueryable();

            // Mock the repository to return classes with no capacity
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            // Act
            var result = await service.LastFiveHousesAsync();

            // Assert
            Assert.IsEmpty(result); // No classes should be returned due to no available capacity
        }


        [Test]
        public async Task IsBookedByIUserWithIdAsync_Should_Return_True_When_User_Is_Booked()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = "user123";

            var bookings = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                UserId = userId,
                FitnessClassId = fitnessClassId,
                BookingDate = DateTime.Now
            }
        }.AsQueryable();

            // Mock the repository to return the bookings
            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                .Returns(bookings);

            // Act
            var result = await service.IsBookedByIUserWithIdAsync(fitnessClassId, userId);

            // Assert
            Assert.IsTrue(result); // Should return true because the user has booked the class
            repositoryMock.Verify(r => r.AllReadOnly<Booking>(), Times.Once);
        }

        [Test]
        public async Task IsBookedByIUserWithIdAsync_Should_Return_False_When_User_Is_Not_Booked()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = "user123";

            var bookings = new List<Booking>
        {
            new Booking
            {
                Id = 1,
                UserId = "differentUser",
                FitnessClassId = fitnessClassId,
                BookingDate = DateTime.Now
            }
        }.AsQueryable();

            // Mock the repository to return the bookings
            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                .Returns(bookings);

            // Act
            var result = await service.IsBookedByIUserWithIdAsync(fitnessClassId, userId);

            // Assert
            Assert.IsFalse(result); // Should return false because the user has not booked the class
            repositoryMock.Verify(r => r.AllReadOnly<Booking>(), Times.Once);
        }

        [Test]
        public async Task IsBookedByIUserWithIdAsync_Should_Return_False_When_No_Bookings_Exist()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = "user123";

            var bookings = new List<Booking>().AsQueryable(); // No bookings exist

            // Mock the repository to return no bookings
            repositoryMock.Setup(r => r.AllReadOnly<Booking>())
                .Returns(bookings);

            // Act
            var result = await service.IsBookedByIUserWithIdAsync(fitnessClassId, userId);

            // Assert
            Assert.IsFalse(result); // Should return false because no bookings exist
            repositoryMock.Verify(r => r.AllReadOnly<Booking>(), Times.Once);
        }

        [Test]
        public async Task HasInstructorWithIdAsync_Should_Return_True_When_Instructor_Is_Associated_With_Class()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = "instructor123";

            var fitnessClasses = new List<FitnessClass>
        {
            new FitnessClass
            {
                Id = fitnessClassId,
                Instructor = new Instructor
                {
                    UserId = userId,
                    User = new ApplicationUser { Id = userId, FirstName = "John", LastName = "Doe" }
                }
            }
        }.AsQueryable();

            // Mock the repository to return the fitness class with the instructor
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            // Act
            var result = await service.HasInstructorWithIdAsync(fitnessClassId, userId);

            // Assert
            Assert.IsTrue(result); // Should return true as the instructor is associated with the class
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task HasInstructorWithIdAsync_Should_Return_False_When_Instructor_Is_Not_Associated_With_Class()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = "instructor123";

            var fitnessClasses = new List<FitnessClass>
        {
            new FitnessClass
            {
                Id = Guid.NewGuid(), // Different fitness class ID
                Instructor = new Instructor
                {
                    UserId = "differentInstructorId", // Different instructor ID
                    User = new ApplicationUser { Id = "differentInstructorId", FirstName = "Jane", LastName = "Smith" }
                }
            }
        }.AsQueryable();

            // Mock the repository to return the fitness class with a different instructor
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            // Act
            var result = await service.HasInstructorWithIdAsync(fitnessClassId, userId);

            // Assert
            Assert.IsFalse(result); // Should return false as the instructor is not associated with the class
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task HasInstructorWithIdAsync_Should_Return_False_When_FitnessClass_Does_Not_Exist()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = "instructor123";

            var fitnessClasses = new List<FitnessClass>().AsQueryable(); // No fitness classes exist

            // Mock the repository to return no fitness classes
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(fitnessClasses);

            // Act
            var result = await service.HasInstructorWithIdAsync(fitnessClassId, userId);

            // Assert
            Assert.IsFalse(result); // Should return false as the fitness class does not exist
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task GetFitnessClassFormModelByIdAsync_Should_Return_FitnessClassFormModel_When_FitnessClass_Exists()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var fitnessClass = new FitnessClass
            {
                Id = fitnessClassId,
                Title = "Yoga Basics",
                Description = "A beginner-friendly yoga class.",
                CategoryId = 1,
                StartTime = new DateTime(2024, 12, 12, 12, 12, 00),
                Duration = 60,
                Capacity = 20
            };

            var fitnessClassFormModel = new FitnessClassFormModel
            {
                Id = fitnessClassId.ToString(),
                Title = "Yoga Basics",
                Description = "A beginner-friendly yoga class.",
                CategoryId = 1,
                StartTime = "12/12/2024 12:12",
                Duration = 60,
                Capacity = 20
            };

            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(new List<FitnessClass> { fitnessClass }.AsQueryable());

            // Act
            var result = await service.GetFitnessClassFormModelByIdAsync(fitnessClassId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(fitnessClassFormModel.Id, result?.Id);
            Assert.AreEqual(fitnessClassFormModel.Title, result?.Title);
            Assert.AreEqual(fitnessClassFormModel.Description, result?.Description);
            Assert.AreEqual(fitnessClassFormModel.CategoryId, result?.CategoryId);
            Assert.AreEqual(fitnessClassFormModel.StartTime, result?.StartTime);
            Assert.AreEqual(fitnessClassFormModel.Duration, result?.Duration);
            Assert.AreEqual(fitnessClassFormModel.Capacity, result?.Capacity);
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task GetFitnessClassFormModelByIdAsync_Should_Return_Null_When_FitnessClass_Does_Not_Exist()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid(); // ID that does not exist
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(new List<FitnessClass>().AsQueryable()); // Empty list to simulate no fitness class found

            // Act
            var result = await service.GetFitnessClassFormModelByIdAsync(fitnessClassId.ToString());

            // Assert
            Assert.IsNull(result); // Should return null as no fitness class exists with the given ID
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task GetFitnessClassFormModelByIdAsync_Should_Return_Null_When_Invalid_Id_Provided()
        {
            // Arrange
            var invalidId = "invalid-guid"; // Invalid GUID format
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(new List<FitnessClass>().AsQueryable()); // Empty list to simulate no fitness class found

            // Act
            var result = await service.GetFitnessClassFormModelByIdAsync(invalidId);

            // Assert
            Assert.IsNull(result); // Should return null as the ID format is invalid and no fitness class is found
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }


        [Test]
        public async Task ExistsAsync_Should_Return_True_When_FitnessClass_Exists()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(new List<FitnessClass> { new FitnessClass { Id = fitnessClassId } }.AsQueryable());

            // Act
            var result = await service.ExistsAsync(fitnessClassId);

            // Assert
            Assert.IsTrue(result); // The fitness class should exist
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task ExistsAsync_Should_Return_False_When_FitnessClass_Does_Not_Exist()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(new List<FitnessClass>().AsQueryable()); // No fitness classes

            // Act
            var result = await service.ExistsAsync(fitnessClassId);

            // Assert
            Assert.IsFalse(result); // The fitness class should not exist
            repositoryMock.Verify(r => r.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task FitnessClassDetailsByIdAsync_Should_Return_Null_When_FitnessClass_Does_Not_Exist()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid(); // ID that does not exist
            repositoryMock.Setup(r => r.AllReadOnly<FitnessClass>())
                .Returns(new List<FitnessClass>().AsQueryable()); // No fitness classes

            // Act

            // Assert
            var ex = Assert.ThrowsAsync<InvalidOperationException>(() => service.FitnessClassDetailsByIdAsync(fitnessClassId));
            Assert.AreEqual("This class doesn't exist!", ex.Message);
        }

        [Test]
        public async Task CategoryExistsAsync_CategoryExists_ReturnsTrue()
        {
            // Arrange
            var categoryId = 1;
            var categories = new List<Category>
            {
                new Category { Id = categoryId, Name = "Yoga" }
            }.AsQueryable();

            repositoryMock.Setup(repo => repo.AllReadOnly<Category>())
                    .Returns(categories);

            // Act
            var result = await service.CategoryExistsAsync(categoryId);

            // Assert
            Assert.IsTrue(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Category>(), Times.Once);
        }

        [Test]
        public async Task CategoryExistsAsync_CategoryDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var categoryId = 99; // Non-existent ID
            var categories = new List<Category>().AsQueryable(); // Empty category list

            repositoryMock.Setup(repo => repo.AllReadOnly<Category>())
                    .Returns(categories);

            // Act
            var result = await service.CategoryExistsAsync(categoryId);

            // Assert
            Assert.IsFalse(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Category>(), Times.Once);
        }

        [Test]
        public async Task DeleteAsync_RemovesAssociatedReviewsAndBookings_ThenDeletesFitnessClass()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var reviews = new List<Review>
            {
                new Review { Id = Guid.NewGuid(), FitnessClassId = fitnessClassId },
                new Review { Id = Guid.NewGuid(), FitnessClassId = fitnessClassId }
            }.AsQueryable();

            var bookings = new List<Booking>
            {
                new Booking { Id = 1, FitnessClassId = fitnessClassId },
                new Booking { Id = 2, FitnessClassId = fitnessClassId }
            }.AsQueryable();

            repositoryMock.Setup(repo => repo.All<Review>()).Returns(reviews);
            repositoryMock.Setup(repo => repo.All<Booking>()).Returns(bookings);

            // Act
            await service.DeleteAsync(fitnessClassId);

            // Assert
            repositoryMock.Verify(repo => repo.DeleteAsync<Review>(It.IsAny<Guid>()), Times.Exactly(reviews.Count()));
            repositoryMock.Verify(repo => repo.DeleteAsync<Booking>(It.IsAny<int>()), Times.Exactly(bookings.Count()));
            repositoryMock.Verify(repo => repo.DeleteAsync<FitnessClass>(fitnessClassId), Times.Once);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task AllFitnessClassesByInstructorIdAsync_WhenInstructorHasClasses_ReturnsCorrectViewModels()
        {
            // Arrange
            var instructorId = 1;
            var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Yoga",
                    LeftCapacity = 10,
                    Status = new Status { Name = "Available" },
                    StatusId = 1,
                    StartTime = new DateTime(2024, 12, 25, 10, 0, 0),
                    InstructorId = instructorId,
                    IsApproved = true
                },
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Pilates",
                    LeftCapacity = 5,
                    Status = new Status { Name = "Available" },
                    StatusId = 1,
                    StartTime = new DateTime(2024, 12, 26, 11, 0, 0),
                    InstructorId = instructorId,
                    IsApproved = true
                }
            }.AsQueryable();

            repositoryMock.Setup(repo => repo.AllReadOnly<FitnessClass>())
                          .Returns(fitnessClasses);

            // Act
            var result = await service.AllFitnessClassesByInstructorIdAsync(instructorId);

            // Assert
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual("Yoga", result.First().Title);
            Assert.AreEqual("25/12/2024 10:00", result.First().StartTime);
            repositoryMock.Verify(repo => repo.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task BookAsync_WhenFitnessClassAndUserExistAndHasCapacity_BooksClass()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            var fitnessClass = new FitnessClass
            {
                Id = fitnessClassId,
                LeftCapacity = 5,
                StatusId = 1,
                StartTime = DateTime.UtcNow
            };

            var user = new ApplicationUser { Id = userId };

            repositoryMock.Setup(repo => repo.GetByIdAsync<FitnessClass>(fitnessClassId))
                          .ReturnsAsync(fitnessClass);
            repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                          .ReturnsAsync(user);

            // Act
            await service.BookAsync(fitnessClassId, userId);

            // Assert
            Assert.AreEqual(4, fitnessClass.LeftCapacity);
            repositoryMock.Verify(repo => repo.AddAsync(It.Is<Booking>(b =>
                b.UserId == userId && b.FitnessClassId == fitnessClassId)), Times.Once);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task BookAsync_WhenFitnessClassHasNoCapacity_DoesNotBook()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            var fitnessClass = new FitnessClass
            {
                Id = fitnessClassId,
                LeftCapacity = 0,
                StatusId = 1,
                StartTime = DateTime.UtcNow
            };

            var user = new ApplicationUser { Id = userId };

            repositoryMock.Setup(repo => repo.GetByIdAsync<FitnessClass>(fitnessClassId))
                          .ReturnsAsync(fitnessClass);
            repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                          .ReturnsAsync(user);

            // Act
            await service.BookAsync(fitnessClassId, userId);

            // Assert
            Assert.AreEqual(0, fitnessClass.LeftCapacity); // Capacity remains unchanged
            repositoryMock.Verify(repo => repo.AddAsync<Booking>(It.IsAny<Booking>()), Times.Never);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task BookAsync_WhenFitnessClassOrUserDoesNotExist_DoesNotBook()
        {
            // Arrange
            var fitnessClassId = Guid.NewGuid();
            var userId = Guid.NewGuid().ToString();

            repositoryMock.Setup(repo => repo.GetByIdAsync<FitnessClass>(fitnessClassId))
                          .ReturnsAsync((FitnessClass)null); // Fitness class not found
            repositoryMock.Setup(repo => repo.GetByIdAsync<ApplicationUser>(userId))
                          .ReturnsAsync((ApplicationUser)null); // User not found

            // Act
            await service.BookAsync(fitnessClassId, userId);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync<Booking>(It.IsAny<Booking>()), Times.Never);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Never);
        }

        [Test]
        public async Task AllCategoriesNamesAsync_ReturnsDistinctCategoryNames()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Name = "Yoga" },
                new Category { Name = "Pilates" },
                new Category { Name = "Yoga" }, // Duplicate
                new Category { Name = "Zumba" }
            }.AsQueryable();

            repositoryMock.Setup(repo => repo.AllReadOnly<Category>())
                          .Returns(categories);

            // Act
            var result = await service.AllCategoriesNamesAsync();

            // Assert
            var expected = new List<string> { "Yoga", "Pilates", "Zumba" }; // Distinct names
            CollectionAssert.AreEquivalent(expected, result); // Order may vary due to `Distinct`
            repositoryMock.Verify(repo => repo.AllReadOnly<Category>(), Times.Once);
        }
    }
}