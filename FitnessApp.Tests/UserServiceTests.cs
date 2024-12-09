using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Tests
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<IRepository> mockRepository;
        private UserService userService;

        [SetUp]
        public void SetUp()
        {
            mockRepository = new Mock<IRepository>();
            userService = new UserService(mockRepository.Object);
        }

        [Test]
        public async Task AllAsync_ShouldReturnListOfUsersWithCorrectProperties()
        {
            // Arrange
            var users = new List<ApplicationUser>
        {
            new ApplicationUser
            {
                Id = "1",
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                Instructor = new Instructor() // Assumes Instructor exists
            },
            new ApplicationUser
            {
                Id = "2",
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@example.com",
                Instructor = null // Assumes no Instructor for this user
            }
        }.AsQueryable();

            var mockDbSet = new Mock<DbSet<ApplicationUser>>();
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockRepository.Setup(r => r.AllReadOnly<ApplicationUser>()).Returns(mockDbSet.Object);

            // Act
            var result = await userService.AllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());

            // Check if the first user has the correct full name and is an instructor
            var firstUser = result.First();
            Assert.AreEqual("John Doe", firstUser.FullName);
            Assert.IsTrue(firstUser.IsInstructor);

            // Check if the second user has the correct full name and is not an instructor
            var secondUser = result.Skip(1).First();
            Assert.AreEqual("Jane Smith", secondUser.FullName);
            Assert.IsFalse(secondUser.IsInstructor);
        }

        [Test]
        public async Task AllAsync_ShouldReturnEmptyList_WhenNoUsersExist()
        {
            // Arrange
            var users = new List<ApplicationUser>().AsQueryable(); // Empty list

            var mockDbSet = new Mock<DbSet<ApplicationUser>>();
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(users.Provider);
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(users.Expression);
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(users.ElementType);
            mockDbSet.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(users.GetEnumerator());

            mockRepository.Setup(r => r.AllReadOnly<ApplicationUser>()).Returns(mockDbSet.Object);

            // Act
            var result = await userService.AllAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
