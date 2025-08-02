using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using BDSS.Common.Enums;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodBagRepository;

public class BloodBagRepository : GenericRepository<BloodBag>, IBloodBagRepository
{
    public BloodBagRepository(BdssDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BloodBag>> GetAvailableBloodBagsAsync()
    {
        return await _context.BloodBags
            .Where(bb => bb.Status == BloodBagStatus.Available && bb.ExpiryDate > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task<IEnumerable<BloodBag>> GetBloodBagsByBloodTypeAsync(BloodType bloodType)
    {
        return await _context.BloodBags
            .Where(bb => bb.BloodType == bloodType)
            .ToListAsync();
    }

    public async Task<IEnumerable<BloodBag>> GetExpiredBloodBagsAsync()
    {
        return await _context.BloodBags
            .Where(bb => bb.ExpiryDate <= DateTime.UtcNow)
            .ToListAsync();
    }
} 