using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data
{
    using FitnessApp.Services.Data.Contracts;
    using System.Diagnostics;

    public class LicenseGeneratorService : ILicenseGeneratorService
    {
        public void GenerateLicenseNumbers()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = "python",
                Arguments = "FitnessApp.Web/wwwroot/Scripts/generate_license_numbers.py", 
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"Error generating license numbers: {error}");
                }
            }
        }
    }
}
