using BDSS.Models.Context;
using BDSS.Models.Entities;
using Microsoft.EntityFrameworkCore;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.UserEventsRepository;

public class UserEventsRepository : GenericRepository<UserEvents>, IUserEventsRepository
{
    public UserEventsRepository(BdssDbContext context) : base(context)
    {
    }

    public async Task<UserEvents?> FindByUserAndEventAsync(long userId, long eventId)
    {
        return await _context.Set<UserEvents>().FirstOrDefaultAsync(ue => ue.UserId == userId && ue.EventId == eventId);
    }
}