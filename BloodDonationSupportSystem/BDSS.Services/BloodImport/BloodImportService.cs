using BDSS.DTOs;
using BDSS.DTOs.BloodImport;
using BDSS.Repositories.BloodDonationApplicationRepository;
using BDSS.Repositories.BloodImportRepository;
using BDSS.Repositories.BloodStorageRepository;
using BDSS.Repositories.UserEventsRepository;

namespace BDSS.Services.BloodImport;

public class BloodImportService : IBloodImportService
{
    private readonly IBloodImportRepository _bloodImportRepository;
    private readonly IBloodDonationApplicationRepository _bloodDonationApplicationRepository;
    private readonly IUserEventsRepository _userEventsRepository;
    private readonly IBloodStorageRepository _bloodStorageRepository;

    public BloodImportService(IBloodImportRepository bloodImportRepository, IBloodDonationApplicationRepository bloodDonationApplicationRepository, IUserEventsRepository userEventsRepository, IBloodStorageRepository bloodStorageRepository)
    {
        _bloodImportRepository = bloodImportRepository;
        _bloodDonationApplicationRepository = bloodDonationApplicationRepository;
        _userEventsRepository = userEventsRepository;
        _bloodStorageRepository = bloodStorageRepository;
    }

    public async Task<BaseResponseModel<GetAllBloodImportsResponse>> GetAllBloodImportsAsync()
    {
        try
        {
            var bloodImports = await _bloodImportRepository.GetAllAsync();
            var response = new GetAllBloodImportsResponse
            {
                BloodImports = bloodImports.Select(b => new BloodImportDto
                {
                    Id = b.Id,
                    BloodStorageId = b.BloodStorageId,
                    BloodDonationApplicationId = b.BloodDonationApplicationId,
                    Note = b.Note,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
            };
            return new BaseResponseModel<GetAllBloodImportsResponse> { Code = 200, Data = response };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllBloodImportsResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodImportDto>> GetBloodImportByIdAsync(long id)
    {
        try
        {
            var bloodImport = await _bloodImportRepository.GetByIdAsync(id);
            if (bloodImport == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "BloodImport not found" };
            var dto = new BloodImportDto
            {
                Id = bloodImport.Id,
                BloodStorageId = bloodImport.BloodStorageId,
                BloodDonationApplicationId = bloodImport.BloodDonationApplicationId,
                Note = bloodImport.Note,
                Status = bloodImport.Status,
                CreatedAt = bloodImport.CreatedAt,
                UpdatedAt = bloodImport.UpdatedAt
            };
            return new BaseResponseModel<BloodImportDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodImportDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodImportDto>> CreateBloodImportAsync(CreateBloodImportRequest request)
    {
        try
        {
            var bloodDonationApplication = await _bloodDonationApplicationRepository.GetByIdAsync(request.BloodDonationApplicationId);
            var bloodStorage = await _bloodStorageRepository.GetByBloodTypeAsync(bloodDonationApplication.BloodType);
            var bloodImport = new Models.Entities.BloodImport
            {
                BloodStorageId = bloodStorage.Id,
                BloodDonationApplicationId = request.BloodDonationApplicationId,
                Note = request.Note
            };
            var created = await _bloodImportRepository.AddAsync(bloodImport);
            var dto = new BloodImportDto
            {
                Id = created.Id,
                BloodStorageId = created.BloodStorageId,
                BloodDonationApplicationId = created.BloodDonationApplicationId,
                Note = created.Note,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
            return new BaseResponseModel<BloodImportDto> { Code = 201, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodImportDto> { Code = 500, Message = ex.Message };
        }
    }

    private static bool IsValidStatusTransition(BDSS.Common.Enums.BloodImportStatus current, BDSS.Common.Enums.BloodImportStatus next)
    {
        return (current == BDSS.Common.Enums.BloodImportStatus.Pending && (next == BDSS.Common.Enums.BloodImportStatus.Accepted || next == BDSS.Common.Enums.BloodImportStatus.Denied))
            || (current == BDSS.Common.Enums.BloodImportStatus.Accepted && next == BDSS.Common.Enums.BloodImportStatus.Imported)
            || (current == next && (current == BDSS.Common.Enums.BloodImportStatus.Imported || current == BDSS.Common.Enums.BloodImportStatus.Denied));
    }

    public async Task<BaseResponseModel<BloodImportDto>> UpdateBloodImportStatusAsync(UpdateBloodImportStatusRequest request)
    {
        try
        {
            var bloodImport = await _bloodImportRepository.GetByIdAsync(request.Id);
            if (bloodImport == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "Blood import request not found" };
            var bloodStorage = await _bloodStorageRepository.GetByIdAsync(bloodImport.BloodStorageId);
            if (bloodStorage == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "Blood storage not found" };
            var bloodDonationApp = await _bloodDonationApplicationRepository.GetByIdAsync(bloodImport.BloodDonationApplicationId.Value);
            if (bloodDonationApp == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "Blood donation application not found" };
            if (!IsValidStatusTransition(bloodImport.Status, request.Status))
            {
                return new BaseResponseModel<BloodImportDto> { Code = 400, Message = "Invalid status transition." };
            }
            bloodImport.Status = request.Status;
            bloodImport.Note = request.Note;
            bloodStorage.Quantity += bloodDonationApp.Quantity;
            await _bloodImportRepository.UpdateAsync(bloodImport);

            if (bloodImport.Status == Common.Enums.BloodImportStatus.Imported && bloodImport.BloodDonationApplicationId != null)
            {
                var donationApp = await _bloodDonationApplicationRepository.GetByIdAsync(bloodImport.BloodDonationApplicationId.Value);
                if (donationApp != null)
                {
                    donationApp.Status = Common.Enums.BloodDonationStatus.Donated;
                    await _bloodDonationApplicationRepository.UpdateAsync(donationApp);
                    if (donationApp.UserId != null && donationApp.EventId != null)
                    {
                        var userEvent = await _userEventsRepository.FindByUserAndEventAsync(donationApp.UserId.Value, donationApp.EventId.Value);
                        if (userEvent != null)
                        {
                            userEvent.Quantity = donationApp.Quantity;
                            userEvent.UserEventsStatus = BDSS.Common.Enums.UserEventsStatus.Attended;
                            await _userEventsRepository.UpdateAsync(userEvent);
                        }
                    }
                }
            }

            var dto = new BloodImportDto
            {
                Id = bloodImport.Id,
                BloodStorageId = bloodImport.BloodStorageId,
                BloodDonationApplicationId = bloodImport.BloodDonationApplicationId,
                Note = bloodImport.Note,
                Status = bloodImport.Status,
                CreatedAt = bloodImport.CreatedAt,
                UpdatedAt = bloodImport.UpdatedAt
            };
            return new BaseResponseModel<BloodImportDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodImportDto>
            {
                Code = 500,
                Message = ex.Message
            };
        }
    }
}