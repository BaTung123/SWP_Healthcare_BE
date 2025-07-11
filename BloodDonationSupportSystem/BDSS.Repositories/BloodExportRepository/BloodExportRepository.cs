using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodExportRepository;

public class BloodExportRepository : GenericRepository<BloodExport>, IBloodExportRepository
{
    public BloodExportRepository(BdssDbContext context) : base(context) { }

    public async Task<BloodExport?> GetByIdAsync(long bloodExportId)
    {
        return await Entities.FirstOrDefaultAsync(b => b.Id == bloodExportId && !b.IsDeleted);
    }

    public async Task<BloodExport?> GetByBloodRequestApplicationIdAsync(long bloodRequestApplicationId)
    {
        return await Entities.FirstOrDefaultAsync(b => b.BloodRequestApplicationId == bloodRequestApplicationId && !b.IsDeleted);
    }
}