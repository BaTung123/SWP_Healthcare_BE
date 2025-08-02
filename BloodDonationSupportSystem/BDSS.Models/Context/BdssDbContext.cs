using BDSS.Models.Context.Configuration;
using BDSS.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BDSS.Models.Context;

public class BdssDbContext : DbContext
{
    #region Fields
    #endregion

    #region Constructors
    public BdssDbContext(DbContextOptions<BdssDbContext> options) : base(options)
    {
    }

    public BdssDbContext()
    {
    }
    #endregion

    #region Properties
    public DbSet<User> Users { get; set; }
    public DbSet<UserOtp> UserOtps { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<BloodImport> BloodImports { get; set; }
    public DbSet<BloodExport> BloodExports { get; set; }
    public DbSet<BloodBag> BloodBags { get; set; }
    public DbSet<Blog> Blogs { get; set; }
    public DbSet<UserEvents> UserEvents { get; set; }
    public DbSet<BloodDonationApplication> BloodDonationApplications { get; set; }
    public DbSet<BloodRequestApplication> BloodRequestApplications { get; set; }
    public DbSet<HealthCheck> HealthChecks { get; set; }
    #endregion

    #region Private Methods
    private static string? GetConnectionString()
    {
        IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"appsettings.json", false)
            .Build();

        return config.GetConnectionString("BDSS_DB");
    }

    private static void ConfigureModel(ModelBuilder modelBuilder)
    {
        EnumConfig.Configure(modelBuilder);
        RelationshipConfig.Configure(modelBuilder);
        DataConfig.Configure(modelBuilder);
    }
    #endregion

    #region Protected Methods
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(GetConnectionString());
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureModel(modelBuilder);
    }
    #endregion

    #region Public Methods
    #endregion
}
