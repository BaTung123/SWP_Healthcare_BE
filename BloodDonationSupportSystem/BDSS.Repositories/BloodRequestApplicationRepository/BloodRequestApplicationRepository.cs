using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BloodRequestApplicationRepository;

public class BloodRequestApplicationRepository : GenericRepository<BloodRequestApplication>, IBloodRequestApplicationRepository
{
    public BloodRequestApplicationRepository(BdssDbContext context) : base(context)
    {
    }
}