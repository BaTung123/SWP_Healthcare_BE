using BDSS.DTOs;
using BDSS.DTOs.Event;
using BDSS.Repositories.EventRepository;
using BDSS.Common.Utils;

namespace BDSS.Services.Event;

public class EventService : IEventService
{
    private readonly IEventRepository _eventRepository;
    public EventService(IEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<BaseResponseModel<GetAllEventsResponse>> GetAllEventsAsync()
    {
        try
        {
            var events = await _eventRepository.GetAllAsync();
            var response = new GetAllEventsResponse
            {
                Events = events.Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Type = e.Type,
                    LocationName = e.LocationName,
                    LocationAddress = e.LocationAddress,
                    TargetParticipant = e.TargetParticipant,
                    EventStartTime = e.EventStartTime,
                    EventEndTime = e.EventEndTime,
                    Status = e.Status,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                })
            };
            return new BaseResponseModel<GetAllEventsResponse> { Code = 200, Data = response };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllEventsResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<EventDto>> GetEventByIdAsync(long id)
    {
        try
        {
            var ev = await _eventRepository.FindAsync(id);
            if (ev == null)
                return new BaseResponseModel<EventDto> { Code = 404, Message = "Event not found" };
            var dto = new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Type = ev.Type,
                LocationName = ev.LocationName,
                LocationAddress = ev.LocationAddress,
                TargetParticipant = ev.TargetParticipant,
                EventStartTime = ev.EventStartTime,
                EventEndTime = ev.EventEndTime,
                Status = ev.Status,
                CreatedAt = ev.CreatedAt,
                UpdatedAt = ev.UpdatedAt
            };
            return new BaseResponseModel<EventDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<EventDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<EventDto>> CreateEventAsync(CreateEventRequest request)
    {
        try
        {
            var now = DateTimeUtils.GetCurrentGmtPlus7();
            if (request.EventStartTime < now || request.EventEndTime < now)
            {
                return new BaseResponseModel<EventDto> { Code = 400, Message = "Event start and end time must be present or future (not in the past)." };
            }
            if (request.EventEndTime <= request.EventStartTime)
            {
                return new BaseResponseModel<EventDto> { Code = 400, Message = "Event end time must be after event start time." };
            }
            var ev = new Models.Entities.Event
            {
                Name = request.Name,
                Type = request.Type,
                LocationName = request.LocationName,
                LocationAddress = request.LocationAddress,
                TargetParticipant = request.TargetParticipant,
                EventStartTime = request.EventStartTime,
                EventEndTime = request.EventEndTime,
                Status = request.Status
            };
            var created = await _eventRepository.AddAsync(ev);
            var dto = new EventDto
            {
                Id = created.Id,
                Name = created.Name,
                Type = created.Type,
                LocationName = created.LocationName,
                LocationAddress = created.LocationAddress,
                TargetParticipant = created.TargetParticipant,
                EventStartTime = created.EventStartTime,
                EventEndTime = created.EventEndTime,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
            return new BaseResponseModel<EventDto> { Code = 201, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<EventDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<EventDto>> UpdateEventAsync(UpdateEventRequest request)
    {
        try
        {
            var now = DateTimeUtils.GetCurrentGmtPlus7();
            if (request.EventStartTime < now || request.EventEndTime < now)
            {
                return new BaseResponseModel<EventDto> { Code = 400, Message = "Event start and end time must be present or future (not in the past)." };
            }
            if (request.EventEndTime <= request.EventStartTime)
            {
                return new BaseResponseModel<EventDto> { Code = 400, Message = "Event end time must be after event start time." };
            }
            var ev = await _eventRepository.FindAsync(request.Id);
            if (ev == null)
                return new BaseResponseModel<EventDto> { Code = 404, Message = "Event not found" };
            if (!IsValidStatusTransition(ev.Status, request.Status))
            {
                return new BaseResponseModel<EventDto> { Code = 400, Message = "Invalid status transition." };
            }
            ev.Name = request.Name;
            ev.Type = request.Type;
            ev.LocationName = request.LocationName;
            ev.LocationAddress = request.LocationAddress;
            ev.TargetParticipant = request.TargetParticipant;
            ev.EventStartTime = request.EventStartTime;
            ev.EventEndTime = request.EventEndTime;
            ev.Status = request.Status;
            await _eventRepository.UpdateAsync(ev);
            var dto = new EventDto
            {
                Id = ev.Id,
                Name = ev.Name,
                Type = ev.Type,
                LocationName = ev.LocationName,
                LocationAddress = ev.LocationAddress,
                TargetParticipant = ev.TargetParticipant,
                EventStartTime = ev.EventStartTime,
                EventEndTime = ev.EventEndTime,
                Status = ev.Status,
                CreatedAt = ev.CreatedAt,
                UpdatedAt = ev.UpdatedAt
            };
            return new BaseResponseModel<EventDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<EventDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<bool>> DeleteEventAsync(long id)
    {
        try
        {
            var ev = await _eventRepository.FindAsync(id);
            if (ev == null)
                return new BaseResponseModel<bool> { Code = 404, Message = "Event not found", Data = false };
            await _eventRepository.DeleteAsync(id);
            return new BaseResponseModel<bool> { Code = 200, Data = true };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<bool> { Code = 500, Message = ex.Message, Data = false };
        }
    }

    private static bool IsValidStatusTransition(BDSS.Common.Enums.EventStatus current, BDSS.Common.Enums.EventStatus next)
    {
        // ComingSoon -> OnGoing, Full, Cancelled
        // OnGoing -> Full, Ended, Cancelled
        // Full -> OnGoing, Ended, Cancelled
        // Ended -> Ended (no change)
        // Cancelled -> Cancelled (no change)
        return (current == BDSS.Common.Enums.EventStatus.ComingSoon && (next == BDSS.Common.Enums.EventStatus.OnGoing || next == BDSS.Common.Enums.EventStatus.Full || next == BDSS.Common.Enums.EventStatus.Cancelled))
            || (current == BDSS.Common.Enums.EventStatus.OnGoing && (next == BDSS.Common.Enums.EventStatus.Full || next == BDSS.Common.Enums.EventStatus.Ended || next == BDSS.Common.Enums.EventStatus.Cancelled))
            || (current == BDSS.Common.Enums.EventStatus.Full && (next == BDSS.Common.Enums.EventStatus.OnGoing || next == BDSS.Common.Enums.EventStatus.Ended || next == BDSS.Common.Enums.EventStatus.Cancelled))
            || (current == next && (current == BDSS.Common.Enums.EventStatus.Ended || current == BDSS.Common.Enums.EventStatus.Cancelled));
    }
}