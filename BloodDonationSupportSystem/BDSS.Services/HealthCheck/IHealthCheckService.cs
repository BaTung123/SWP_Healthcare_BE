using BDSS.DTOs;
using BDSS.DTOs.HealthCheck;

namespace BDSS.Services.HealthCheck;

public interface IHealthCheckService
{
    Task<BaseResponseModel<GetAllHealthChecksResponse>> GetAllHealthChecksAsync();
    Task<BaseResponseModel<HealthCheckDto>> GetHealthCheckByIdAsync(GetHealthCheckByIdRequest request);
    Task<BaseResponseModel<IEnumerable<HealthCheckDto>>> GetHealthChecksByUserIdAsync(GetHealthChecksByUserIdRequest request);
    Task<BaseResponseModel<IEnumerable<HealthCheckDto>>> GetHealthChecksByBloodDonationApplicationIdAsync(GetHealthChecksByBloodDonationApplicationIdRequest request);
    Task<BaseResponseModel<HealthCheckDto>> CreateHealthCheckAsync(CreateHealthCheckRequest request);
    Task<BaseResponseModel<HealthCheckDto>> UpdateHealthCheckAsync(UpdateHealthCheckRequest request);
    Task<BaseResponseModel<bool>> DeleteHealthCheckAsync(long id);
}