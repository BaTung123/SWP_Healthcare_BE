using BDSS.DTOs;
using BDSS.DTOs.Event;

namespace BDSS.Services.Event;

public interface IEventService
{
    Task<BaseResponseModel<GetAllEventsResponse>> GetAllEventsAsync();
    Task<BaseResponseModel<EventDto>> GetEventByIdAsync(long id);
    Task<BaseResponseModel<EventDto>> CreateEventAsync(CreateEventRequest request);
    Task<BaseResponseModel<EventDto>> UpdateEventAsync(UpdateEventRequest request);
    Task<BaseResponseModel<bool>> DeleteEventAsync(long id);
}