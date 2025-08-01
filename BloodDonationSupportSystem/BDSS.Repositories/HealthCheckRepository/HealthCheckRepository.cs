using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.HealthCheckRepository;

public class HealthCheckRepository : GenericRepository<HealthCheck>, IHealthCheckRepository
{
    public HealthCheckRepository(BdssDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<HealthCheck>> GetByUserIdAsync(long userId)
    {
        return await _context.Set<HealthCheck>()
            .Where(hc => hc.UserId == userId && !hc.IsDeleted)
            .ToListAsync();
    }

    public async Task<IEnumerable<HealthCheck>> GetByBloodDonationApplicationIdAsync(long bloodDonationApplicationId)
    {
        return await _context.Set<HealthCheck>()
            .Where(hc => hc.BloodDonationApplicationId == bloodDonationApplicationId && !hc.IsDeleted)
            .ToListAsync();
    }
} 