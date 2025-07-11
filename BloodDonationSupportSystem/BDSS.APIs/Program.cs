using BDSS.APIs.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

ServiceConfig.Configure(builder.Services);
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

app.Run();
