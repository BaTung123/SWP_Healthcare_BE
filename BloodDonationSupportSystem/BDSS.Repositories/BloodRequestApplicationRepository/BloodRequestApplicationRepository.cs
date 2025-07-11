using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodRequestApplicationRepository;

public class BloodRequestApplicationRepository : GenericRepository<BloodRequestApplication>, IBloodRequestApplicationRepository
{
    public BloodRequestApplicationRepository(BdssDbContext context) : base(context) { }

    public async Task<BloodRequestApplication?> GetByIdAsync(long id)
    {
        return await Entities.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

}