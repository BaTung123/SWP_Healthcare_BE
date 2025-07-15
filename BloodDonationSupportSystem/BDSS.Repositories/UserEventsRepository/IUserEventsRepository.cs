using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.UserEventsRepository;

public interface IUserEventsRepository : IGenericRepository<UserEvents>
{
    Task<UserEvents?> FindByUserAndEventAsync(long userId, long eventId);
    Task<int> CountByEventIdAsync(long eventId);
}