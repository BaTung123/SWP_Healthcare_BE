using BDSS.Common;
using BDSS.Common.Configurations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BDSS.APIs.Configuration;

internal static class AuthConfig
{
    #region Public methods
    public static void Configure(IServiceCollection services, IConfiguration config)
    {
        ConfigureJwtBearer(services, config);
    }
    #endregion

    #region Private methods
    private static void ConfigureJwtBearer(IServiceCollection services, IConfiguration config)
    {
        JwtConfigurations? jwtConfigs =
            config.GetSection(Constants.JwtConfig).Get<JwtConfigurations>();

        if (jwtConfigs == null)
        {
            return;
        }

        services.AddSingleton(jwtConfigs);
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtConfigs.Key)
            );

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtConfigs.Issuer,
                ValidAudience = jwtConfigs.Audience,
                IssuerSigningKey = key,
            };
        });

        services.AddAuthorization();
    }
    #endregion
}
