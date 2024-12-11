using FitnessApp.Data;
using FitnessApp.Data.Models;
using FitnessApp.Data.Repository.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace FitnessApp.Services.Data
{
    public class FitnessClassStatusUpdateService : BackgroundService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly ILogger<FitnessClassStatusUpdateService> logger;
        private readonly TimeSpan checkInterval = TimeSpan.FromSeconds(30);

        public FitnessClassStatusUpdateService(IServiceScopeFactory _serviceScopeFactory, ILogger<FitnessClassStatusUpdateService> _logger)
        {
            this.serviceScopeFactory = _serviceScopeFactory;
            this.logger = _logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await UpdateFitnessClassStatuses();
                }
                catch (Exception ex)
                {

                    logger.LogError($"An error occurred while updating fitness class statuses: {ex.Message}");
                }

                await Task.Delay(checkInterval, stoppingToken);
            }
        }

        private async Task UpdateFitnessClassStatuses()
        {
            // Create a scope for resolving scoped services like IRepository
            using (var scope = serviceScopeFactory.CreateScope())
            {
                // Resolve the IRepository inside the scope
                var repository = scope.ServiceProvider.GetRequiredService<IRepository>();
                var currentDateTime = DateTime.Now;

                // Example of using IRepository to fetch and update fitness classes
                var fitnessClasses = await repository.All<FitnessClass>().ToListAsync();

                var classesToUpdate = fitnessClasses.Where(c => c.StartTime < currentDateTime && c.StatusId != 3).ToList();

                foreach (var fitnessClass in classesToUpdate)
                {
                    fitnessClass.StatusId = 3; // Set status to Finished

                    var bookings = await repository.AllReadOnly<Booking>()
                        .Where(b => b.FitnessClassId == fitnessClass.Id)
                        .ToListAsync();

                    await repository.SaveChangesAsync();

                    if (bookings != null)
                    {
                        foreach (var booking in bookings)
                        {
                            await repository.DeleteAsync<Booking>(booking);
                        }
                    }

                    await repository.SaveChangesAsync();
                }

                logger.LogInformation($"Updated {classesToUpdate.Count} fitness classes to 'Finished' status.");
            }
        }
    }
}
