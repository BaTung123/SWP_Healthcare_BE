using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodDonationApplicationRepository;

public interface IBloodDonationApplicationRepository : IGenericRepository<BloodDonationApplication>
{
    Task<BloodDonationApplication?> GetByIdAsync(long id);
    Task<IEnumerable<BloodDonationApplication>> GetByUserIdAsync(long userId);
}