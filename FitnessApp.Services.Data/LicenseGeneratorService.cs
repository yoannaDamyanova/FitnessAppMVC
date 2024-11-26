using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessApp.Services.Data
{
    using FitnessApp.Services.Data.Contracts;

    public class LicenseGeneratorService : ILicenseGeneratorService
    {
        public List<int> GenerateUniqueLicenseNumbers()
        {
            HashSet<int> numbers = new HashSet<int>();

            // Add the first number manually (123456)
            numbers.Add(123456);

            Random rand = new Random();

            // Generate unique numbers until we reach the desired count
            while (numbers.Count < 50)
            {   
                int randomNumber = rand.Next(100000, 1000000);
                numbers.Add(randomNumber);
            }

            // Return the numbers as a list
            return new List<int>(numbers);
        }
    }
}
