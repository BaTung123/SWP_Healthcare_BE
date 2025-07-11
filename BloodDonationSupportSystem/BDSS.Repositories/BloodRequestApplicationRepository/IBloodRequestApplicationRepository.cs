using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodRequestApplicationRepository;

public interface IBloodRequestApplicationRepository : IGenericRepository<BloodRequestApplication>
{
    Task<BloodRequestApplication?> GetByIdAsync(long id);
}