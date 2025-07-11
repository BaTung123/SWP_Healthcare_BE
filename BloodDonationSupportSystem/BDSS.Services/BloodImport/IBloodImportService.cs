using BDSS.DTOs;
using BDSS.DTOs.BloodImport;

namespace BDSS.Services.BloodImport;

public interface IBloodImportService
{
    Task<BaseResponseModel<GetAllBloodImportsResponse>> GetAllBloodImportsAsync();
    Task<BaseResponseModel<BloodImportDto>> GetBloodImportByIdAsync(long id);
    Task<BaseResponseModel<BloodImportDto>> CreateBloodImportAsync(CreateBloodImportRequest request);
    Task<BaseResponseModel<BloodImportDto>> UpdateBloodImportStatusAsync(UpdateBloodImportStatusRequest request);
}