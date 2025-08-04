using BDSS.DTOs;
using BDSS.DTOs.BloodExport;
using BDSS.Repositories.BloodExportRepository;
using BDSS.Repositories.BloodRequestApplicationRepository;
using BDSS.Repositories.BloodBagRepository;

namespace BDSS.Services.BloodExport;

public class BloodExportService : IBloodExportService
{
    private readonly IBloodExportRepository _bloodExportRepository;
    private readonly IBloodRequestApplicationRepository _bloodRequestApplicationRepository;
    private readonly IBloodBagRepository _bloodBagRepository;
    public BloodExportService(IBloodExportRepository bloodExportRepository, IBloodRequestApplicationRepository bloodRequestApplicationRepository, IBloodBagRepository bloodBagRepository)
    {
        _bloodExportRepository = bloodExportRepository;
        _bloodRequestApplicationRepository = bloodRequestApplicationRepository;
        _bloodBagRepository = bloodBagRepository;
    }

    public async Task<BaseResponseModel<GetAllBloodExportsResponse>> GetAllBloodExportsAsync()
    {
        try
        {
            var bloodExports = await _bloodExportRepository.GetAllAsync();
            var response = new GetAllBloodExportsResponse
            {
                BloodExports = bloodExports.Select(b => new BloodExportDto
                {
                    Id = b.Id,
                    BloodBagId = b.BloodBagId,
                    BloodRequestApplicationId = b.BloodRequestApplicationId,
                    Note = b.Note,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
            };
            return new BaseResponseModel<GetAllBloodExportsResponse> { Code = 200, Data = response };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllBloodExportsResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodExportDto>> GetBloodExportByIdAsync(long id)
    {
        try
        {
            var bloodExport = await _bloodExportRepository.FindAsync(id);
            if (bloodExport == null)
                return new BaseResponseModel<BloodExportDto> { Code = 404, Message = "BloodExport not found" };
            var dto = new BloodExportDto
            {
                Id = bloodExport.Id,
                BloodBagId = bloodExport.BloodBagId,
                BloodRequestApplicationId = bloodExport.BloodRequestApplicationId,
                Note = bloodExport.Note,
                Status = bloodExport.Status,
                CreatedAt = bloodExport.CreatedAt,
                UpdatedAt = bloodExport.UpdatedAt
            };
            return new BaseResponseModel<BloodExportDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodExportDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodExportDto>> CreateBloodExportAsync(CreateBloodExportRequest request)
    {
        try
        {
            var bloodBag = await _bloodBagRepository.FindAsync(request.BloodBagId);
            if (bloodBag == null)
                return new BaseResponseModel<BloodExportDto> { Code = 404, Message = "BloodBag not found" };
            if (bloodBag.Status != BDSS.Common.Enums.BloodBagStatus.Available)
                return new BaseResponseModel<BloodExportDto> { Code = 400, Message = "BloodBag is not available" };
            bloodBag.Status = BDSS.Common.Enums.BloodBagStatus.Exported;
            await _bloodBagRepository.UpdateAsync(bloodBag);
            var bloodExport = new Models.Entities.BloodExport
            {
                BloodBagId = request.BloodBagId,
                BloodRequestApplicationId = request.BloodRequestApplicationId,
                Note = request.Note
            };
            var created = await _bloodExportRepository.AddAsync(bloodExport);
            var dto = new BloodExportDto
            {
                Id = created.Id,
                BloodBagId = created.BloodBagId,
                BloodRequestApplicationId = created.BloodRequestApplicationId,
                Note = created.Note,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
            return new BaseResponseModel<BloodExportDto> { Code = 201, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodExportDto> { Code = 500, Message = ex.Message };
        }
    }

    private static bool IsValidStatusTransition(BDSS.Common.Enums.BloodExportStatus current, BDSS.Common.Enums.BloodExportStatus next)
    {
        return (current == BDSS.Common.Enums.BloodExportStatus.Pending && (next == BDSS.Common.Enums.BloodExportStatus.Accepted || next == BDSS.Common.Enums.BloodExportStatus.Denied || next == BDSS.Common.Enums.BloodExportStatus.Exported))
            || (current == BDSS.Common.Enums.BloodExportStatus.Accepted && next == BDSS.Common.Enums.BloodExportStatus.Exported)
            || (current == next && (current == BDSS.Common.Enums.BloodExportStatus.Exported || current == BDSS.Common.Enums.BloodExportStatus.Denied));
    }

    public async Task<BaseResponseModel<BloodExportDto>> UpdateBloodExportStatusAsync(UpdateBloodExportStatusRequest request)
    {
        try
        {
            var bloodExport = await _bloodExportRepository.FindAsync(request.Id);
            if (bloodExport == null)
                return new BaseResponseModel<BloodExportDto> { Code = 404, Message = "BloodExport not found" };

            if (!IsValidStatusTransition(bloodExport.Status, request.Status))
            {
                return new BaseResponseModel<BloodExportDto> { Code = 400, Message = "Invalid status transition." };
            }
            bloodExport.Status = request.Status;
            bloodExport.Note = request.Note;
            await _bloodExportRepository.UpdateAsync(bloodExport);

            // Advanced logic: If status is Exported, update related BloodRequestApplication
            if (bloodExport.Status == BDSS.Common.Enums.BloodExportStatus.Exported && bloodExport.BloodRequestApplicationId != null)
            {
                var requestApp = await _bloodRequestApplicationRepository.FindAsync(bloodExport.BloodRequestApplicationId.Value);
                if (requestApp != null)
                {
                    requestApp.Status = BDSS.Common.Enums.BloodRequestStatus.Received;
                    requestApp.BloodBagId = bloodExport.BloodBagId;
                    await _bloodRequestApplicationRepository.UpdateAsync(requestApp);
                }
            }

            var dto = new BloodExportDto
            {
                Id = bloodExport.Id,
                BloodBagId = bloodExport.BloodBagId,
                BloodRequestApplicationId = bloodExport.BloodRequestApplicationId,
                Note = bloodExport.Note,
                Status = bloodExport.Status,
                CreatedAt = bloodExport.CreatedAt,
                UpdatedAt = bloodExport.UpdatedAt
            };
            return new BaseResponseModel<BloodExportDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodExportDto>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }
}