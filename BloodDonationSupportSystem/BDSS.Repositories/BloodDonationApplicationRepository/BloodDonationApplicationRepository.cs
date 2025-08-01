using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodDonationApplicationRepository;

public class BloodDonationApplicationRepository : GenericRepository<BloodDonationApplication>, IBloodDonationApplicationRepository
{
    public BloodDonationApplicationRepository(BdssDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BloodDonationApplication>> GetByUserIdAsync(long userId)
    {
        return await Entities.Where(bda => bda.UserId == userId && !bda.IsDeleted).ToListAsync();
    }

    public async Task<BloodDonationApplication?> GetLatestByUserIdAsync(long userId)
    {
        return await Entities
            .Where(bda => bda.UserId == userId && !bda.IsDeleted)
            .OrderByDescending(bda => bda.CreatedAt)
            .FirstOrDefaultAsync();
    }
}