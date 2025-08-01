using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.EventRepository;

public class EventRepository : GenericRepository<Event>, IEventRepository
{
    public EventRepository(BdssDbContext context) : base(context)
    {
    }
}