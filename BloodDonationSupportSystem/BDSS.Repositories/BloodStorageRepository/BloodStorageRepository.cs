using BDSS.Common.Enums;
using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodStorageRepository;

public class BloodStorageRepository : GenericRepository<BloodStorage>, IBloodStorageRepository
{
    public BloodStorageRepository(BdssDbContext context) : base(context)
    {
    }

    public async Task<BloodStorage?> GetByBloodTypeAsync(BDSS.Common.Enums.BloodType bloodType)
    {
        return await Entities.FirstOrDefaultAsync(bs => bs.BloodType == bloodType && !bs.IsDeleted);
    }
}