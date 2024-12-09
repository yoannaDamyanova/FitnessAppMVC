using FitnessApp.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Tests
{
    [TestFixture]
    public class BaseServiceTests
    {
        private BaseService baseService;

        [SetUp]
        public void SetUp()
        {
            baseService = new BaseService();
        }

        [Test]
        public void IsGuidValid_ShouldReturnTrue_IfValidGuidIsProvided()
        {
            // Arrange
            string validGuidString = "d9b8a1c0-efb6-48bc-b6cd-5f3f3a6b4746";
            Guid parsedGuid = Guid.Empty;

            // Act
            var result = baseService.IsGuidValid(validGuidString, ref parsedGuid);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(new Guid(validGuidString), parsedGuid);
        }

        [Test]
        public void IsGuidValid_ShouldReturnFalse_IfInvalidGuidIsProvided()
        {
            // Arrange
            string invalidGuidString = "invalid-guid";
            Guid parsedGuid = Guid.Empty;

            // Act
            var result = baseService.IsGuidValid(invalidGuidString, ref parsedGuid);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(Guid.Empty, parsedGuid);  // The parsedGuid should remain empty if invalid.
        }

        [Test]
        public void IsGuidValid_ShouldReturnFalse_IfGuidIsNull()
        {
            // Arrange
            string? nullGuidString = null;
            Guid parsedGuid = Guid.Empty;

            // Act
            var result = baseService.IsGuidValid(nullGuidString, ref parsedGuid);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(Guid.Empty, parsedGuid);  // The parsedGuid should remain empty.
        }

        [Test]
        public void IsGuidValid_ShouldReturnFalse_IfGuidIsEmpty()
        {
            // Arrange
            string emptyGuidString = string.Empty;
            Guid parsedGuid = Guid.Empty;

            // Act
            var result = baseService.IsGuidValid(emptyGuidString, ref parsedGuid);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(Guid.Empty, parsedGuid);  // The parsedGuid should remain empty.
        }

        [Test]
        public void IsGuidValid_ShouldReturnFalse_IfGuidIsWhiteSpace()
        {
            // Arrange
            string whiteSpaceGuidString = "   ";
            Guid parsedGuid = Guid.Empty;

            // Act
            var result = baseService.IsGuidValid(whiteSpaceGuidString, ref parsedGuid);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(Guid.Empty, parsedGuid);  // The parsedGuid should remain empty.
        }
    }
}
