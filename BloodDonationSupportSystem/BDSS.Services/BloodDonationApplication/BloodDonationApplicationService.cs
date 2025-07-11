using BDSS.DTOs;
using BDSS.DTOs.BloodDonationApplication;
using BDSS.Models.Entities;
using BDSS.Repositories.BloodDonationApplicationRepository;
using BDSS.Common.Enums;
using BDSS.Repositories.GenericRepository;
using BDSS.Repositories.UserEventsRepository;

namespace BDSS.Services.BloodDonationApplication;

public class BloodDonationApplicationService : IBloodDonationApplicationService
{
    private readonly IBloodDonationApplicationRepository _repository;

    private readonly IUserEventsRepository _userEventsRepository;
    public BloodDonationApplicationService(IBloodDonationApplicationRepository repository, IUserEventsRepository userEventsRepository)
    {
        _repository = repository;
        _userEventsRepository = userEventsRepository;
    }

    public async Task<BaseResponseModel<GetAllBloodDonationApplicationsResponse>> GetAllBloodDonationApplicationsAsync()
    {
        var entities = await _repository.GetAllAsync();
        var dtos = entities.Select(e => ToDto(e)).ToList();
        return new BaseResponseModel<GetAllBloodDonationApplicationsResponse>
        {
            Code = 200,
            Data = new GetAllBloodDonationApplicationsResponse { BloodDonationApplications = dtos }
        };
    }

    public async Task<BaseResponseModel<BloodDonationApplicationDto>> GetBloodDonationApplicationByIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 404, Message = "Not found" };
        }
        return new BaseResponseModel<BloodDonationApplicationDto> { Code = 200, Data = ToDto(entity) };
    }

    public async Task<BaseResponseModel<IEnumerable<BloodDonationApplicationDto>>> GetBloodDonationApplicationByUserIdAsync(long userId)
    {
        var entities = await _repository.GetByUserIdAsync(userId);
        var dtos = entities.Select(ToDto);
        return new BaseResponseModel<IEnumerable<BloodDonationApplicationDto>> { Code = 200, Data = dtos };
    }

    public async Task<BaseResponseModel<BloodDonationApplicationDto>> CreateBloodDonationApplicationAsync(CreateBloodDonationApplicationRequest request)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.FullName) || request.Quantity <= 0)
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 400, Message = "FullName and Quantity are required." };
        }
        var entity = new BDSS.Models.Entities.BloodDonationApplication
        {
            BloodStorageId = request.BloodStorageId,
            FullName = request.FullName,
            Dob = request.Dob,
            Gender = request.Gender,
            BloodType = request.BloodType,
            BloodTransferType = request.BloodTransferType,
            Status = BloodDonationStatus.Pending,
            Quantity = request.Quantity.HasValue ? request.Quantity.Value : 0,
            Note = request.Note,
            PhoneNumber = request.PhoneNumber,
            DonationStartDate = request.DonationStartDate ?? DateOnly.FromDateTime(DateTime.Now),
            DonationEndDate = request.DonationEndDate ?? DateOnly.MinValue
        };
        var created = await _repository.AddAsync(entity);
        return new BaseResponseModel<BloodDonationApplicationDto> { Code = 201, Data = ToDto(created) };
    }

    public async Task<BaseResponseModel<BloodDonationApplicationDto>> UpdateBloodDonationApplicationStatusAsync(UpdateBloodDonationApplicationStatusRequest request)
    {
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 404, Message = "Not found" };
        }
        // Update fields
        if (!IsValidStatusTransition(entity.Status, request.Status))
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 400, Message = "Invalid status transition." };
        }
        entity.Status = request.Status;
        await _repository.UpdateAsync(entity);

        // Advanced logic: If status is Donated, update UserEvents
        // (Removed: now handled in BloodImportService)
        return new BaseResponseModel<BloodDonationApplicationDto> { Code = 200, Data = ToDto(entity) };
    }

    private static BloodDonationApplicationDto ToDto(BDSS.Models.Entities.BloodDonationApplication entity)
    {
        return new BloodDonationApplicationDto
        {
            Id = entity.Id,
            BloodStorageId = entity.BloodStorageId,
            FullName = entity.FullName,
            Dob = entity.Dob,
            Gender = entity.Gender,
            BloodType = entity.BloodType,
            BloodTransferType = entity.BloodTransferType,
            Status = entity.Status,
            Quantity = entity.Quantity,
            Note = entity.Note,
            PhoneNumber = entity.PhoneNumber,
            DonationStartDate = entity.DonationStartDate,
            DonationEndDate = entity.DonationEndDate
        };
    }

    private static bool IsValidStatusTransition(BDSS.Common.Enums.BloodDonationStatus current, BDSS.Common.Enums.BloodDonationStatus next)
    {
        return (current == BDSS.Common.Enums.BloodDonationStatus.Pending && (next == BDSS.Common.Enums.BloodDonationStatus.Accepted || next == BDSS.Common.Enums.BloodDonationStatus.Denied))
            || (current == BDSS.Common.Enums.BloodDonationStatus.Accepted && next == BDSS.Common.Enums.BloodDonationStatus.Donated)
            || (current == next && (current == BDSS.Common.Enums.BloodDonationStatus.Donated || current == BDSS.Common.Enums.BloodDonationStatus.Denied));
    }
}