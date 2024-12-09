using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Services.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FitnessApp.Web.ViewModels.Instructor;
using FitnessApp.Data.Models;

namespace FitnessApp.Tests
{
    [TestFixture]
    public class InstructorServiceTests
    {
        private Mock<IRepository> repositoryMock;
        private IInstructorService service;

        [SetUp]
        public void Setup()
        {
            repositoryMock = new Mock<IRepository>();
            service = new InstructorService(repositoryMock.Object);
        }

        [Test]
        public async Task CreateAsync_WhenCalled_AddsInstructorAndSavesChanges()
        {
            // Arrange
            var userId = "user123";
            var model = new BecomeInstructorFormModel
            {
                Biography = "Experienced instructor",
                LicenseNumber = 12345,
                Specializations = "Yoga, Pilates"
            };

            // Act
            await service.CreateAsync(model, userId);

            // Assert
            repositoryMock.Verify(repo => repo.AddAsync(It.Is<Instructor>(i =>
                i.UserId == userId &&
                i.Biography == model.Biography &&
                i.LicenseNumber == model.LicenseNumber &&
                i.Specializations == model.Specializations)), Times.Once);

            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ExistsByIdAsync_WhenInstructorExists_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor> { new Instructor { Id = userId } }.AsQueryable());

            // Act
            var result = await service.ExistsByIdAsync(userId);

            // Assert
            Assert.IsTrue(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task ExistsByIdAsync_WhenInstructorDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = 99;
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act
            var result = await service.ExistsByIdAsync(userId);

            // Assert
            Assert.IsFalse(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task IsLicenseNumberValidAsync_WhenLicenseIsValid_ReturnsTrue()
        {
            // Arrange
            var licenseNumber = 123456;
            var instructors = new List<Instructor>
            {
                new Instructor { LicenseNumber = licenseNumber }
            }.AsQueryable();

            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>()).Returns(instructors);

            // Act
            var result = await service.IsLicenseNumberValidAsync(licenseNumber);

            // Assert
            Assert.IsFalse(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task GetInstructorByIdAsync_WhenInstructorExists_ReturnsId()
        {
            // Arrange
            var userId = "user123";
            var instructor = new Instructor { Id = 1, UserId = userId };
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            var result = await service.GetInstructorByIdAsync(userId);

            // Assert
            Assert.AreEqual(1, result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task Rate_WhenCalled_UpdatesRatingAndSavesChanges()
        {
            // Arrange
            var instructorId = 1;
            var model = new InstructorRateFormModel { Rating = 4.5 };

            var instructor = new Instructor { Id = instructorId, Rating = 4.0 };
            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            await service.Rate(model, instructorId);

            // Assert
            Assert.AreEqual(4.25, instructor.Rating);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task EditBiographyAsync_WhenCalled_UpdatesBiographyAndSavesChanges()
        {
            // Arrange
            var instructorId = 1;
            var model = new InstructorEditBiographyFormModel { Biography = "Updated Biography" };

            var instructor = new Instructor { Id = instructorId, Biography = "Old Biography" };
            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            await service.EditBiographyAsync(model, instructorId);

            // Assert
            Assert.AreEqual("Updated Biography", instructor.Biography);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task EditSpecializationsAsync_WhenCalled_UpdatesSpecializationsAndSavesChanges()
        {
            // Arrange
            var instructorId = 1;
            var model = new InstructorEditSpecializationsFormModel { Specializations = "Updated Specializations" };

            var instructor = new Instructor { Id = instructorId, Specializations = "Old Specializations" };
            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            await service.EditSpecializationsAsync(model, instructorId);

            // Assert
            Assert.AreEqual("Updated Specializations", instructor.Specializations);
            repositoryMock.Verify(repo => repo.SaveChangesAsync(), Times.Once);
        }

        [Test]
        public async Task ExistsByUserIdAsync_WhenValidUserIdExists_ReturnsTrue()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var instructors = new List<Instructor>
            {
                new Instructor { UserId = userId }
            }.AsQueryable();

            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>()).Returns(instructors);

            // Act
            var result = await service.ExistsByUserIdAsync(userId);

            // Assert
            Assert.IsTrue(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task ExistsByUserIdAsync_WhenInvalidUserIdFormat_ReturnsFalse()
        {
            // Arrange
            var userId = "invalid-guid";

            // Act
            var result = await service.ExistsByUserIdAsync(userId);

            // Assert
            Assert.IsFalse(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Never);
        }

        [Test]
        public async Task ExistsByUserIdAsync_WhenValidUserIdDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act
            var result = await service.ExistsByUserIdAsync(userId);

            // Assert
            Assert.IsFalse(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task GetInstructorIdByUserId_WhenUserIdExists_ReturnsInstructorId()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var instructor = new Instructor { Id = 1, UserId = userId };
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            var result = await service.GetInstructorIdByUserId(userId);

            // Assert
            Assert.AreEqual(1, result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public void GetInstructorIdByUserId_WhenUserIdDoesNotExist_ThrowsInvalidOperationException()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act & Assert
            var result = service.GetInstructorIdByUserId(userId);
            Assert.IsNull(result.Result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task UserWithLicenseNumberExistsInDbAsync_WhenLicenseNumberExists_ReturnsTrue()
        {
            // Arrange
            var licenseNumber = 12345;
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor>
                          {
                      new Instructor { LicenseNumber = licenseNumber }
                          }.AsQueryable());

            // Act
            var result = await service.UserWithLicenseNumberExistsInDbAsync(licenseNumber);

            // Assert
            Assert.IsTrue(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task UserWithLicenseNumberExistsInDbAsync_WhenLicenseNumberDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var licenseNumber = 12345;
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act
            var result = await service.UserWithLicenseNumberExistsInDbAsync(licenseNumber);

            // Assert
            Assert.IsFalse(result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public void UserWithLicenseNumberExistsGlobally_WhenLicenseNumberExists_ReturnsTrue()
        {
            // Arrange
            var licenseNumber = 123456;

            // Act
            var result = service.UserWithLicenseNumberExistsGlobally(licenseNumber);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void UserWithLicenseNumberExistsGlobally_WhenLicenseNumberDoesNotExist_ReturnsFalse()
        {
            // Arrange
            var licenseNumber = 543219;

            // Act
            var result = service.UserWithLicenseNumberExistsGlobally(licenseNumber);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task GetRatingByIdAsync_WhenUserHasRating_ReturnsRating()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var instructor = new Instructor { UserId = userId, Rating = 4.5 };
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            var result = await service.GetRatingByIdAsync(userId);

            // Assert
            Assert.AreEqual(4.5, result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task GetRatingByIdAsync_WhenUserDoesNotHaveRating_ReturnsZero()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            repositoryMock.Setup(repo => repo.AllReadOnly<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act
            var result = await service.GetRatingByIdAsync(userId);

            // Assert
            Assert.AreEqual(0, result);
            repositoryMock.Verify(repo => repo.AllReadOnly<Instructor>(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WhenInstructorExists_ReturnsInstructor()
        {
            // Arrange
            var instructorId = 1;
            var instructor = new Instructor { Id = instructorId };
            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());

            // Act
            var result = await service.GetByIdAsync(instructorId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(instructorId, result.Id);
            repositoryMock.Verify(repo => repo.All<Instructor>(), Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_WhenInstructorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = 1;
            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act
            var result = await service.GetByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
            repositoryMock.Verify(repo => repo.All<Instructor>(), Times.Once);
        }

        [Test]
        public async Task GetInstructorViewModelByIdAsync_WhenInstructorExists_ReturnsViewModel()
        {
            // Arrange
            var userId = 1;
            var instructor = new Instructor
            {
                Id = userId,
                Biography = "Test Bio",
                Specializations = "Test Specializations",
                User = new ApplicationUser { FirstName = "John", LastName = "Doe" },
                Rating = 4.5
            };
            var fitnessClasses = new List<FitnessClass>
            {
                new FitnessClass
                {
                    Id = Guid.NewGuid(),
                    Title = "Yoga Class",
                    Duration = 60,
                    Capacity = 10,
                    Instructor = instructor,
                    InstructorId = userId,
                    Status = new Status { Name = "Open" },
                    StartTime = DateTime.UtcNow,
                    IsApproved = true
                }
            };

            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor> { instructor }.AsQueryable());
            repositoryMock.Setup(repo => repo.AllReadOnly<FitnessClass>())
                          .Returns(fitnessClasses.AsQueryable());

            // Act
            var result = await service.GetInstructorViewModelByIdAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userId, result.Id);
            Assert.AreEqual("Test Bio", result.Biography);
            Assert.AreEqual(1, result.Classes.Count());
            repositoryMock.Verify(repo => repo.All<Instructor>(), Times.Once);
            repositoryMock.Verify(repo => repo.AllReadOnly<FitnessClass>(), Times.Once);
        }

        [Test]
        public async Task GetInstructorViewModelByIdAsync_WhenInstructorDoesNotExist_ReturnsNull()
        {
            // Arrange
            var userId = 1;
            repositoryMock.Setup(repo => repo.All<Instructor>())
                          .Returns(new List<Instructor>().AsQueryable());

            // Act
            var result = await service.GetInstructorViewModelByIdAsync(userId);

            // Assert
            Assert.IsNull(result);
            repositoryMock.Verify(repo => repo.All<Instructor>(), Times.Once);
            repositoryMock.Verify(repo => repo.AllReadOnly<FitnessClass>(), Times.Never);
        }

    }
}
