using BDSS.DTOs;
using BDSS.DTOs.BloodImport;
using BDSS.Repositories.BloodDonationApplicationRepository;
using BDSS.Repositories.BloodImportRepository;
using BDSS.Repositories.BloodBagRepository;
using BDSS.Repositories.UserEventsRepository;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Services.BloodImport;

public class BloodImportService : IBloodImportService
{
    private readonly IBloodImportRepository _bloodImportRepository;
    private readonly IBloodDonationApplicationRepository _bloodDonationApplicationRepository;
    private readonly IUserEventsRepository _userEventsRepository;
    private readonly IBloodBagRepository _bloodBagRepository;

    public BloodImportService(IBloodImportRepository bloodImportRepository, IBloodDonationApplicationRepository bloodDonationApplicationRepository, IUserEventsRepository userEventsRepository, IBloodBagRepository bloodBagRepository)
    {
        _bloodImportRepository = bloodImportRepository;
        _bloodDonationApplicationRepository = bloodDonationApplicationRepository;
        _userEventsRepository = userEventsRepository;
        _bloodBagRepository = bloodBagRepository;
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
                    BloodBagId = b.BloodBagId,
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
            var bloodImport = await _bloodImportRepository.FindAsync(id);
            if (bloodImport == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "BloodImport not found" };
            var dto = new BloodImportDto
            {
                Id = bloodImport.Id,
                BloodBagId = bloodImport.BloodBagId,
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
            var bloodDonationApplication = await _bloodDonationApplicationRepository.FindAsync(request.BloodDonationApplicationId);
            var bloodBag = await _bloodBagRepository.GetBloodBagsByBloodTypeAsync(bloodDonationApplication.BloodType);
            var availableBloodBag = bloodBag.FirstOrDefault(bb => bb.Status == BDSS.Common.Enums.BloodBagStatus.Available);
            if (availableBloodBag == null)
                return new BaseResponseModel<BloodImportDto> { Code = 400, Message = "No available blood bag found for this blood type" };

            var bloodImport = new Models.Entities.BloodImport
            {
                BloodBagId = availableBloodBag.Id,
                BloodDonationApplicationId = request.BloodDonationApplicationId,
                Note = request.Note
            };
            var created = await _bloodImportRepository.AddAsync(bloodImport);
            var dto = new BloodImportDto
            {
                Id = created.Id,
                BloodBagId = created.BloodBagId,
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
            var bloodImport = await _bloodImportRepository.FindAsync(request.Id);
            if (bloodImport == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "Blood import request not found" };
            var bloodBag = await _bloodBagRepository.FindAsync(bloodImport.BloodBagId);
            if (bloodBag == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "Blood bag not found" };
            var bloodDonationApp = await _bloodDonationApplicationRepository.FindAsync(bloodImport.BloodDonationApplicationId.Value);
            if (bloodDonationApp == null)
                return new BaseResponseModel<BloodImportDto> { Code = 404, Message = "Blood donation application not found" };
            if (!IsValidStatusTransition(bloodImport.Status, request.Status))
            {
                return new BaseResponseModel<BloodImportDto> { Code = 400, Message = "Invalid status transition." };
            }
            bloodImport.Status = request.Status;
            bloodImport.Note = request.Note;
            bloodBag.Quantity += bloodDonationApp.Quantity;
            await _bloodImportRepository.UpdateAsync(bloodImport);

            if (bloodImport.Status == Common.Enums.BloodImportStatus.Imported && bloodImport.BloodDonationApplicationId != null)
            {
                var donationApp = await _bloodDonationApplicationRepository.FindAsync(bloodImport.BloodDonationApplicationId.Value);
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
                BloodBagId = bloodImport.BloodBagId,
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