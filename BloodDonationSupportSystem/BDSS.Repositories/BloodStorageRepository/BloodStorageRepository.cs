using BDSS.Common.Enums;
using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodStorageRepository;

public class BloodStorageRepository : GenericRepository<BloodStorage>, IBloodStorageRepository
{
    public BloodStorageRepository(BdssDbContext context) : base(context) { }

    public async Task<BloodStorage?> GetByIdAsync(long id)
    {
        return await Entities.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
    }

    public async Task<BloodStorage?> GetByBloodTypeAsync(BloodType bloodType)
    {
        return await Entities.FirstOrDefaultAsync(x => x.BloodType == bloodType && !x.IsDeleted);
    }
}