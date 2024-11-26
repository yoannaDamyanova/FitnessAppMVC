using FitnessApp.Services.Data.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;


namespace FitnessApp.Services.Data
{
    public class NumberGenerationBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly string _filePath;

        // Constructor to receive the configuration and service provider
        public NumberGenerationBackgroundService(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            // Manually set the file path
            var projectRoot = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\"));

            // Now combine this with the relative path to "FitnessApp.Services.Data\Licenses"
            _filePath = Path.Combine(
                projectRoot,                          // This points to the root of your project
                "FitnessApp.Services.Data",           // Your sub-directory
                "Licenses",                           // The "Licenses" folder
                "localLicenseNumbers.json"            // The target JSON file
            );
            Console.WriteLine($"File Path: {_filePath}");  // Debugging log to verify the file path
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Ensure the file path is not null
            if (string.IsNullOrEmpty(_filePath))
            {
                Console.WriteLine("File path is null or empty. Please check your configuration.");
                return;  // Exit early if the file path is not valid
            }

            // Resolve the scoped service ILicenseGeneratorService
            using (var scope = _serviceProvider.CreateScope())
            {
                var licenseGeneratorService = scope.ServiceProvider.GetRequiredService<ILicenseGeneratorService>();

                // Generate unique license numbers
                var numbers = licenseGeneratorService.GenerateUniqueLicenseNumbers();

                // Serialize the numbers to JSON
                string json = JsonConvert.SerializeObject(numbers, Formatting.Indented);

                // Debugging: Print the serialized JSON
                Console.WriteLine("Serialized JSON:");
                Console.WriteLine(json);

                // Ensure the directory exists before writing to the file
                var directory = Path.GetDirectoryName(_filePath);
                if (!Directory.Exists(directory))
                {
                    Console.WriteLine($"Directory does not exist. Creating directory: {directory}");
                    Directory.CreateDirectory(directory);  // Create the directory if it doesn't exist
                }

                // Try writing the numbers to the file
                try
                {
                    File.WriteAllText(_filePath, json);
                    Console.WriteLine("Number generation complete. Numbers saved to file.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error writing to file: {ex.Message}");
                }
            }

            await Task.CompletedTask;
        }
    }
}
