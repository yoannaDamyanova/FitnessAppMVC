using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data
{
    public class InstructorService : BaseService, IInstructorService
    {
        private readonly IRepository repository;
        private readonly string licenseFilePath = "FitnessApp.Web/wwwroot/data/license_numbers.json"; // Path to the JSON file

        public InstructorService(IRepository _repository)
        {
            repository = _repository;
        }
        private List<int> LoadLicenseNumbers()
        {
            if (!File.Exists(licenseFilePath))
            {
                throw new FileNotFoundException("License numbers file not found.");
            }

            var licenseNumbers = JsonSerializer.Deserialize<List<int>>(File.ReadAllText(licenseFilePath));
            return new List<int>(licenseNumbers);
        }

        public async Task CreateAsync(string userId)
        {
            await repository.AddAsync(new Instructor { UserId = userId });
        }

        public async Task<bool> ExistsByIdAsync(string userId)
        {
            return await repository.AllReadOnly<Instructor>()
                .AnyAsync(a => a.UserId == userId); 
        }

        public async Task<int?> GetInstructorByIdAsync(string userId)
        {
            return (await repository.AllReadOnly<Instructor>()
                .FirstOrDefaultAsync(a => a.UserId == userId))?.Id;
        }

        public async Task<bool> IsLicenseNumberValidAsync(int licenseNumber)
        {
            var licenseNumbers = LoadLicenseNumbers();

            bool isInGeneratedList = licenseNumbers.Contains(licenseNumber);

            bool isRegistered = await repository.AllReadOnly<Instructor>()
                .AnyAsync(i => i.LicenseNumber == licenseNumber);

            return isInGeneratedList && !isRegistered;
        }

        public async Task<bool> UserWithLicenseNumberExistsInDbAsync(int licenseNumber)
        {
            return await repository.AllReadOnly<Instructor>()
                .AnyAsync(i => i.LicenseNumber == licenseNumber);
        }

        public bool UserWithLicenseNumberExistsGlobally(int licenseNumber)
        {
            return  LoadLicenseNumbers().Any(ln => ln == licenseNumber);
        }
    }
}
