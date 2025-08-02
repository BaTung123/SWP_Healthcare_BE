using BDSS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Models.Context.Configuration;

internal static class EnumConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BloodImport>()
            .Property(o => o.Status)
            .HasConversion<int>();

        modelBuilder.Entity<BloodExport>()
            .Property(o => o.Status)
            .HasConversion<int>();

        modelBuilder.Entity<BloodBag>()
            .Property(o => o.Status)
            .HasConversion<int>();

        modelBuilder.Entity<Event>()
            .Property(o => o.Status)
            .HasConversion<int>();

        modelBuilder.Entity<User>()
            .Property(o => o.Role)
            .HasConversion<int>();
    }
}
