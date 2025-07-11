using Microsoft.Extensions.DependencyInjection;
using BDSS.Models.Context;
using BDSS.Repositories.BlogRepository;
using BDSS.Repositories.BloodExportRepository;
using BDSS.Repositories.BloodImportRepository;
using BDSS.Repositories.EventRepository;
using BDSS.Repositories.UserOtpRepository;
using BDSS.Repositories.UserRepository;
using BDSS.Repositories.BloodStorageRepository;
using BDSS.Repositories.BloodDonationApplicationRepository;
using BDSS.Repositories.BloodRequestApplicationRepository;

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
        services.AddScoped<IBloodExportRepository, BloodExportRepository>();
        services.AddScoped<IBloodImportRepository, BloodImportRepository>();
        services.AddScoped<IEventRepository, EventRepository>();
        services.AddScoped<IBloodStorageRepository, BloodStorageRepository>();
        services.AddScoped<IBloodDonationApplicationRepository, BloodDonationApplicationRepository>();
        services.AddScoped<IBloodRequestApplicationRepository, BloodRequestApplicationRepository>();
    }
}
