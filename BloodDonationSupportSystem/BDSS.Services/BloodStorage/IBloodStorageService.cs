using BDSS.DTOs;
using BDSS.DTOs.BloodStorage;

namespace BDSS.Services.BloodStorage;

public interface IBloodStorageService
{
    Task<BaseResponseModel<GetAllBloodStoragesResponse>> GetAllBloodStoragesAsync();
    Task<BaseResponseModel<BloodStorageDto>> GetBloodStorageByIdAsync(long id);
    Task<BaseResponseModel<BloodStorageDto>> CreateBloodStorageAsync(CreateBloodStorageRequest request);
    Task<BaseResponseModel<BloodStorageDto>> UpdateBloodStorageAsync(UpdateBloodStorageRequest request);
    Task<BaseResponseModel<bool>> DeleteBloodStorageAsync(long id);
}