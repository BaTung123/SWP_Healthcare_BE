using BDSS.DTOs.UserEvents;
using BDSS.DTOs;
using BDSS.Models.Entities;
using BDSS.Repositories.UserEventsRepository;
using BDSS.Repositories.EventRepository;
using System.Threading.Tasks;

namespace BDSS.Services.UserEvents;

public class UserEventsService : IUserEventsService
{
    private readonly IUserEventsRepository _userEventsRepository;
    private readonly IEventRepository _eventRepository;

    public UserEventsService(IUserEventsRepository userEventsRepository, IEventRepository eventRepository)
    {
        _userEventsRepository = userEventsRepository;
        _eventRepository = eventRepository;
    }

    public async Task<BaseResponseModel<RegisterUserToEventResponse>> RegisterUserToEventAsync(RegisterUserToEventRequest request)
    {
        var existing = await _userEventsRepository.FindByUserAndEventAsync(request.UserId, request.EventId);
        if (existing != null)
        {
            return new BaseResponseModel<RegisterUserToEventResponse>
            {
                Code = 400,
                Message = "User already registered for this event.",
                Data = new RegisterUserToEventResponse { Success = false, Message = "User already registered." }
            };
        }

        var eventEntity = await _eventRepository.FindAsync(request.EventId);
        if (eventEntity == null)
        {
            return new BaseResponseModel<RegisterUserToEventResponse>
            {
                Code = 404,
                Message = "Event not found.",
                Data = new RegisterUserToEventResponse { Success = false, Message = "Event not found." }
            };
        }

        // Add the user to the event
        var userEvent = new BDSS.Models.Entities.UserEvents
        {
            UserId = request.UserId,
            EventId = request.EventId,
            UserEventsStatus = BDSS.Common.Enums.UserEventsStatus.Registered
        };
        await _userEventsRepository.AddAsync(userEvent);

        // Count users and update event status if needed
        var userCount = await _userEventsRepository.CountByEventIdAsync(request.EventId);
        if (userCount >= eventEntity.TargetParticipant && eventEntity.Status != BDSS.Common.Enums.EventStatus.Full)
        {
            eventEntity.Status = BDSS.Common.Enums.EventStatus.Full;
            await _eventRepository.UpdateAsync(eventEntity);
        }

        return new BaseResponseModel<RegisterUserToEventResponse>
        {
            Code = 201,
            Data = new RegisterUserToEventResponse { Success = true, Message = "User registered successfully." }
        };
    }
}