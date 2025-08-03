using BDSS.Models.Entities;
using BDSS.Services.Authentication;
using BDSS.Services.Authentication.Hash;
using BDSS.Services.Authentication.Token;
using BDSS.Services.Blog;
using BDSS.Services.BloodDonationApplication;
using BDSS.Services.BloodExport;
using BDSS.Services.BloodImport;
using BDSS.Services.BloodRequestApplication;
using BDSS.Services.BloodBag;
using BDSS.Services.Event;
using BDSS.Services.HealthCheck;
using BDSS.Services.UserEvents;
using BDSS.Services.BackgroundServices;
using Microsoft.AspNetCore.Identity;


namespace BDSS.APIs.Configuration;

internal static class ServiceConfig
{
    public static void Configure(IServiceCollection services, IConfiguration configuration)
    {
        RegisterServices(services, configuration);
    }

    private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Authentication services
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IPasswordHashingService, PasswordHashingService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITokenService, JwtService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IBloodExportService, BloodExportService>();
        services.AddScoped<IBloodImportService, BloodImportService>();
        services.AddScoped<IEventService, EventService>();
        services.AddScoped<IBloodBagService, BloodBagService>();
        services.AddScoped<IBloodDonationApplicationService, BloodDonationApplicationService>();
        services.AddScoped<IBloodRequestApplicationService, BloodRequestApplicationService>();
        services.AddScoped<IUserEventsService, UserEventsService>();
        services.AddScoped<IHealthCheckService, HealthCheckService>();

        // Background services
        services.AddHostedService<ExpiredBloodBagService>();
        services.AddLogging(logging => logging.AddConsole());

    }
}
