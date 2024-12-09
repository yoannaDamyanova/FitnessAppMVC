using FitnessApp.Services.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Tests
{
    [TestFixture]
    public class LicenseGeneratorServiceTests
    {
        private LicenseGeneratorService licenseGeneratorService;

        [SetUp]
        public void SetUp()
        {
            licenseGeneratorService = new LicenseGeneratorService();
        }

        [Test]
        public void GenerateUniqueLicenseNumbers_ShouldReturnListOf50UniqueNumbers()
        {
            // Act
            var licenseNumbers = licenseGeneratorService.GenerateUniqueLicenseNumbers();

            // Assert
            Assert.IsNotNull(licenseNumbers);
            Assert.AreEqual(50, licenseNumbers.Count);
            Assert.AreEqual(licenseNumbers.Distinct().Count(), licenseNumbers.Count, "The list contains duplicate numbers.");
        }

        [Test]
        public void GenerateUniqueLicenseNumbers_ShouldContain123456()
        {
            // Act
            var licenseNumbers = licenseGeneratorService.GenerateUniqueLicenseNumbers();

            // Assert
            Assert.Contains(123456, licenseNumbers, "The list does not contain the manually added license number 123456.");
        }

        [Test]
        public void GenerateUniqueLicenseNumbers_ShouldContainOnlySixDigitNumbers()
        {
            // Act
            var licenseNumbers = licenseGeneratorService.GenerateUniqueLicenseNumbers();

            // Assert
            Assert.IsTrue(licenseNumbers.All(num => num >= 100000 && num < 1000000),
                "The list contains numbers that are not six digits.");
        }
    }
}
