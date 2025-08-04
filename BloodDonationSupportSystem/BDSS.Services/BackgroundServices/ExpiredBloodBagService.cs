using BDSS.Common.Enums;
using BDSS.Common.Utils;
using BDSS.Models.Entities;
using BDSS.Repositories.BloodBagRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BDSS.Services.BackgroundServices;

public class ExpiredBloodBagService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ExpiredBloodBagService> _logger;
    private readonly TimeSpan _checkInterval;

    public ExpiredBloodBagService(
        IServiceProvider serviceProvider,
        ILogger<ExpiredBloodBagService> logger,
        IConfiguration configuration)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        var intervalSeconds = configuration.GetValue<int>("ExpiredBloodBag:CheckIntervalSeconds", 1); // Default 1 second
        _checkInterval = TimeSpan.FromSeconds(intervalSeconds);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("ExpiredBloodBagService started.");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Checking for expired blood bags at {Time}", DateTimeUtils.GetCurrentGmtPlus7());

                using var scope = _serviceProvider.CreateScope();
                var bloodBagRepository = scope.ServiceProvider.GetRequiredService<IBloodBagRepository>();

                // Get all blood bags that are available but have expired
                var expiredBloodBags = await bloodBagRepository.GetExpiredBloodBagsAsync();
                var availableExpiredBags = expiredBloodBags.Where(bb => bb.Status == BloodBagStatus.Available).ToList();

                if (availableExpiredBags.Any())
                {
                    _logger.LogInformation("Found {Count} expired blood bags that need status update", availableExpiredBags.Count);

                    foreach (var bloodBag in availableExpiredBags)
                    {
                        bloodBag.Status = BloodBagStatus.Expired;
                        await bloodBagRepository.UpdateAsync(bloodBag);
                        _logger.LogInformation("Updated blood bag {BagNumber} status to Expired", bloodBag.BagNumber);
                    }

                    _logger.LogInformation("Successfully updated {Count} expired blood bags", availableExpiredBags.Count);
                }
                else
                {
                    _logger.LogDebug("No expired blood bags found that need status update");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking for expired blood bags at {Time}", DateTimeUtils.GetCurrentGmtPlus7());
            }

            await Task.Delay(_checkInterval, stoppingToken);
        }

        _logger.LogInformation("ExpiredBloodBagService stopped.");
    }
}