using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using FitnessApp.Services.Data.Contracts;
using FitnessApp.Web.ViewModels.Instructor;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FitnessApp.Services.Data
{
    public class InstructorService : BaseService, IInstructorService
    {
        private readonly IRepository repository;
        private readonly string projectRoot;


        private readonly string licenseFilePath;

        public InstructorService(IRepository _repository)
        {
            projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\"));
            licenseFilePath = Path.Combine(
            projectRoot,                          
                "FitnessApp.Services.Data",          
                "Licenses",                           
                "localLicenseNumbers.json"           
            );
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

        public async Task CreateAsync(BecomeInstructorFormModel model, string userId)
        {
            var instructor = new Instructor()
            {
                UserId = userId,
                Biography = model.Biography,
                LicenseNumber = model.LicenseNumber,
                Specializations = model.Specializations,
            };

            await repository.AddAsync(instructor);

            await repository.SaveChangesAsync();
        }

        public async Task<bool> ExistsByIdAsync(string userId)
        {
            return await repository.AllReadOnly<Instructor>().AnyAsync(i => i.UserId == userId);
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
            return LoadLicenseNumbers().Any(ln => ln == licenseNumber);
        }

        public async Task<double> GetRatingByIdAsync(string userId)
        {
            var instructor = await repository.AllReadOnly<Instructor>()
                .FirstOrDefaultAsync(i => i.UserId == userId);

            return instructor?.Rating ?? 0;
        }
    }
}
