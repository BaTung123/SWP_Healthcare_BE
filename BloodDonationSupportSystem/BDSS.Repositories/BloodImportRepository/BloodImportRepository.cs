using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BloodImportRepository;

public class BloodImportRepository : GenericRepository<BloodImport>, IBloodImportRepository
{
    public BloodImportRepository(BdssDbContext context) : base(context) { }

    public async Task<BloodImport?> GetByIdAsync(long bloodImportId)
    {
        return await Entities.FirstOrDefaultAsync(b => b.Id == bloodImportId && !b.IsDeleted);
    }
}