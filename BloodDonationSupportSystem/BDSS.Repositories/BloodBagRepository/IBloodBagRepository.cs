using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodBagRepository;

public interface IBloodBagRepository : IGenericRepository<BloodBag>
{
    Task<IEnumerable<BloodBag>> GetAvailableBloodBagsAsync();
    Task<IEnumerable<BloodBag>> GetBloodBagsByBloodTypeAsync(BDSS.Common.Enums.BloodType bloodType);
    Task<IEnumerable<BloodBag>> GetExpiredBloodBagsAsync();
}