using BDSS.DTOs;
using BDSS.DTOs.BloodDonationApplication;

namespace BDSS.Services.BloodDonationApplication;

public interface IBloodDonationApplicationService
{
    Task<BaseResponseModel<GetAllBloodDonationApplicationsResponse>> GetAllBloodDonationApplicationsAsync();
    Task<BaseResponseModel<BloodDonationApplicationDto>> GetBloodDonationApplicationByIdAsync(long id);
    Task<BaseResponseModel<IEnumerable<BloodDonationApplicationDto>>> GetBloodDonationApplicationByUserIdAsync(long id);
    Task<BaseResponseModel<BloodDonationApplicationDto>> CreateBloodDonationApplicationAsync(CreateBloodDonationApplicationRequest request);
    Task<BaseResponseModel<BloodDonationApplicationDto>> UpdateBloodDonationApplicationStatusAsync(UpdateBloodDonationApplicationStatusRequest request);
    Task<BaseResponseModel<BloodDonationApplicationDto>> UpdateBloodDonationApplicationAsync(UpdateBloodDonationApplicationRequest request);
}