using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodImportRepository;

public class BloodImportRepository : GenericRepository<BloodImport>, IBloodImportRepository
{
    public BloodImportRepository(BdssDbContext context) : base(context)
    {
    }
}