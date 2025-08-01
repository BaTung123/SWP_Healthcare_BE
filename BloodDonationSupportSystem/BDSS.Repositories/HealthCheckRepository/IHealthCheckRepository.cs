using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.HealthCheckRepository;

public interface IHealthCheckRepository : IGenericRepository<HealthCheck>
{
    Task<IEnumerable<HealthCheck>> GetByUserIdAsync(long userId);
    Task<IEnumerable<HealthCheck>> GetByBloodDonationApplicationIdAsync(long bloodDonationApplicationId);
} 