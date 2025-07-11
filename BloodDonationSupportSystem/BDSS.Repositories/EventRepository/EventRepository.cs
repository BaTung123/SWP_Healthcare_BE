using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.EventRepository;

public class EventRepository : GenericRepository<Event>, IEventRepository
{
    public EventRepository(BdssDbContext context) : base(context) { }

    public async Task<Event?> GetByIdAsync(long eventId)
    {
        return await Entities.FirstOrDefaultAsync(e => e.Id == eventId && !e.IsDeleted);
    }
}