using BDSS.DTOs.UserEvents;
using BDSS.DTOs;
using System.Threading.Tasks;

namespace BDSS.Services.UserEvents;

public interface IUserEventsService
{
    Task<BaseResponseModel<RegisterUserToEventResponse>> RegisterUserToEventAsync(RegisterUserToEventRequest request);
}