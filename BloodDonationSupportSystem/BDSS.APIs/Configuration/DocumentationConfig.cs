using Microsoft.OpenApi.Models;

namespace BDSS.APIs.Configuration;

internal static class DocumentationConfig
{
    #region Public methods
    public static void Configure(IServiceCollection services)
    {
        GenerateSwagger(services);
    }
    #endregion

    #region Private methods
    private static void GenerateSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var securityDefinition = new OpenApiSecurityScheme()
            {
                Name = "Bearer",
                BearerFormat = "JWT",
                Scheme = "bearer",
                Description = "Specify the authorization token.",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
            };
            c.AddSecurityDefinition("jwt_auth", securityDefinition);

            var securityScheme = new OpenApiSecurityScheme()
            {
                Reference = new OpenApiReference()
                {
                    Id = "jwt_auth",
                    Type = ReferenceType.SecurityScheme
                }
            };
            var securityRequirements = new OpenApiSecurityRequirement()
            {
                {securityScheme, Array.Empty<string>()},
            };
            c.AddSecurityRequirement(securityRequirements);
        });
    }
    #endregion
}
