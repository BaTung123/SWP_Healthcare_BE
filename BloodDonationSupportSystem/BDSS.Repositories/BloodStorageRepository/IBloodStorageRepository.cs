using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using BDSS.Common.Enums;

namespace BDSS.Repositories.BloodStorageRepository;

public interface IBloodStorageRepository : IGenericRepository<BloodStorage>
{
    Task<BloodStorage?> GetByIdAsync(long id);
    Task<BloodStorage?> GetByBloodTypeAsync(BloodType bloodType);
}