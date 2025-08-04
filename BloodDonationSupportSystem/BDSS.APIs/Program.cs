using BDSS.APIs.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ServiceConfig.Configure(builder.Services, builder.Configuration);
RepositoryConfig.Configure(builder.Services);
AuthConfig.Configure(builder.Services, builder.Configuration);
DocumentationConfig.Configure(builder.Services);
CorsConfig.Configure(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Redirect to Swagger UI when accessing the root URL
app.Use(async (context, next) =>
{
    if (context.Request.Path.Value == "/")
    {
        context.Response.Redirect("/swagger");
        return;
    }
    await next();
});

app.UseHttpsRedirection();

// Enable CORS
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Verify background services are running
var logger = app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation("🚀 Application starting - background services will start automatically");

app.Run();
