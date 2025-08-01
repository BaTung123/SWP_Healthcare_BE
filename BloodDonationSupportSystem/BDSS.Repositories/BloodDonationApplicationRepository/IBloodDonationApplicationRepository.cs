using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodDonationApplicationRepository;

public interface IBloodDonationApplicationRepository : IGenericRepository<BloodDonationApplication>
{
    Task<IEnumerable<BloodDonationApplication>> GetByUserIdAsync(long userId);
    Task<BloodDonationApplication?> GetLatestByUserIdAsync(long userId);
}