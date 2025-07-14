using BDSS.Common.Enums;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodStorageRepository;

public interface IBloodStorageRepository : IGenericRepository<BloodStorage>
{
    Task<BloodStorage?> GetByIdAsync(long id);
    Task<BloodStorage?> GetByBloodTypeAsync(BloodType bloodType);
}