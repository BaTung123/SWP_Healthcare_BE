using BDSS.DTOs;
using BDSS.DTOs.BloodExport;

namespace BDSS.Services.BloodExport;

public interface IBloodExportService
{
    Task<BaseResponseModel<GetAllBloodExportsResponse>> GetAllBloodExportsAsync();
    Task<BaseResponseModel<BloodExportDto>> GetBloodExportByIdAsync(long id);
    Task<BaseResponseModel<BloodExportDto>> CreateBloodExportAsync(CreateBloodExportRequest request);
    Task<BaseResponseModel<BloodExportDto>> UpdateBloodExportStatusAsync(UpdateBloodExportStatusRequest request);
}