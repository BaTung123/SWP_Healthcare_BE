using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodExportRepository;

public interface IBloodExportRepository : IGenericRepository<BloodExport>
{
    Task<BloodExport?> GetByIdAsync(long bloodExportId);
    Task<BloodExport?> GetByBloodRequestApplicationIdAsync(long bloodRequestApplicationId);
}