using BDSS.Models.Context;
using BDSS.Repositories.BlogRepository;
using BDSS.Repositories.BloodDonationApplicationRepository;
using BDSS.Repositories.BloodExportRepository;
using BDSS.Repositories.BloodImportRepository;
using BDSS.Repositories.BloodRequestApplicationRepository;
using BDSS.Repositories.BloodBagRepository;
using BDSS.Repositories.EventRepository;
using BDSS.Repositories.HealthCheckRepository;
using BDSS.Repositories.UserEventsRepository;
using BDSS.Repositories.UserOtpRepository;
using BDSS.Repositories.UserRepository;

namespace BDSS.APIs.Configuration;

internal static class RepositoryConfig
{
    public static void Configure(IServiceCollection services)
    {
        RegisterRepositories(services);
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        // Register DbContext
        services.AddDbContext<BdssDbContext>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserOtpRepository, UserOtpRepository>();
        services.AddScoped<IBlogRepository, BlogRepository>();
        services.AddScoped<IUserEventsRepository, UserEventsRepository>();
        services.AddScoped<IBloodExportRepository, BloodExportRepository>();
        services.AddScoped<IBloodImportRepository, BloodImportRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IBloodBagRepository, BloodBagRepository>();
        services.AddScoped<IBloodDonationApplicationRepository, BloodDonationApplicationRepository>();
        services.AddScoped<IBloodRequestApplicationRepository, BloodRequestApplicationRepository>();
        services.AddScoped<IHealthCheckRepository, HealthCheckRepository>();
    }
}
