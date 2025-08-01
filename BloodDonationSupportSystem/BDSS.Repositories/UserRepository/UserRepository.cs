using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.UserRepository;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(BdssDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await Entities.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }
}
