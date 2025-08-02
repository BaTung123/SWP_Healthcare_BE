using BDSS.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Models.Context.Configuration;

internal static class RelationshipConfig
{
    public static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserEvents>()
            .HasKey(o => new { o.UserId, o.EventId });

        // Many-to-many: User <-> Event via UserEvents
        modelBuilder.Entity<UserEvents>()
            .HasOne(ue => ue.User)
            .WithMany(u => u.UserEvents)
            .HasForeignKey(ue => ue.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserEvents>()
            .HasOne(ue => ue.Event)
            .WithMany(e => e.UserEvents)
            .HasForeignKey(ue => ue.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Remove invalid BloodImport -> Event relationship (no EventId property)

        // One-to-many: BloodBag -> BloodImport
        modelBuilder.Entity<BloodImport>()
            .HasOne(bi => bi.BloodBag)
            .WithMany(bb => bb.BloodImports)
            .HasForeignKey(bi => bi.BloodBagId)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: BloodBag -> BloodExport
        modelBuilder.Entity<BloodExport>()
            .HasOne(be => be.BloodBag)
            .WithMany(bb => bb.BloodExports)
            .HasForeignKey(be => be.BloodBagId)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: BloodDonationApplication -> BloodImport (optional)
        modelBuilder.Entity<BloodImport>()
            .HasOne(bi => bi.BloodDonationApplication)
            .WithMany()
            .HasForeignKey(bi => bi.BloodDonationApplicationId)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: User -> HealthCheck (optional)
        modelBuilder.Entity<HealthCheck>()
            .HasOne(hc => hc.User)
            .WithMany()
            .HasForeignKey(hc => hc.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // One-to-many: BloodDonationApplication -> HealthCheck (optional)
        modelBuilder.Entity<HealthCheck>()
            .HasOne(hc => hc.BloodDonationApplication)
            .WithMany()
            .HasForeignKey(hc => hc.BloodDonationApplicationId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
