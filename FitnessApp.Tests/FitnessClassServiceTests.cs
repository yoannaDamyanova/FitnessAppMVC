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
    }
}