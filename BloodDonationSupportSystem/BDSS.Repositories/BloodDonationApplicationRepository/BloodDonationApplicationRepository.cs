using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodDonationApplicationRepository;

public class BloodDonationApplicationRepository : GenericRepository<BloodDonationApplication>, IBloodDonationApplicationRepository
{
    public BloodDonationApplicationRepository(BdssDbContext context) : base(context) { }

    public async Task<BloodDonationApplication?> GetByIdAsync(long id)
    {
        return await Entities.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<IEnumerable<BloodDonationApplication>> GetByUserIdAsync(long userId)
    {
        return await Entities.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
    }

    public async Task<BloodDonationApplication?> GetLatestByUserIdAsync(long userId)
    {
        return await Entities
            .Where(x => x.UserId == userId && !x.IsDeleted)
            .OrderByDescending(x => x.DonationEndDate)
            .FirstOrDefaultAsync();
    }
}