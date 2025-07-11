using BDSS.DTOs;
using BDSS.DTOs.BloodRequestApplication;

namespace BDSS.Services.BloodRequestApplication;

public interface IBloodRequestApplicationService
{
    Task<BaseResponseModel<GetAllBloodRequestApplicationsResponse>> GetAllBloodRequestApplicationsAsync();
    Task<BaseResponseModel<BloodRequestApplicationDto>> GetBloodRequestApplicationByIdAsync(long id);
    Task<BaseResponseModel<BloodRequestApplicationDto>> CreateBloodRequestApplicationAsync(CreateBloodRequestApplicationRequest request);
    Task<BaseResponseModel<BloodRequestApplicationDto>> UpdateBloodRequestApplicationStatusAsync(UpdateBloodRequestApplicationStatusRequest request);
}