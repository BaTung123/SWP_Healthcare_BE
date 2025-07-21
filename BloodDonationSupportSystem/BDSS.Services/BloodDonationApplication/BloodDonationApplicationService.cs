using BDSS.Common.Enums;
using BDSS.Common.Utils;
using BDSS.DTOs;
using BDSS.DTOs.BloodDonationApplication;
using BDSS.Models.Entities;
using BDSS.Repositories.BloodDonationApplicationRepository;
using BDSS.Repositories.BloodStorageRepository;
using BDSS.Repositories.UserEventsRepository;

namespace BDSS.Services.BloodDonationApplication;

public class BloodDonationApplicationService : IBloodDonationApplicationService
{
    private readonly IBloodDonationApplicationRepository _repository;
    private readonly IBloodStorageRepository _bloodStorageRepository;
    private readonly IUserEventsRepository _userEventsRepository;
    public BloodDonationApplicationService(IBloodDonationApplicationRepository repository, IUserEventsRepository userEventsRepository, IBloodStorageRepository bloodStorageRepository)
    {
        _repository = repository;
        _userEventsRepository = userEventsRepository;
        _bloodStorageRepository = bloodStorageRepository;

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
        var bloodStorage = await _bloodStorageRepository.GetByBloodTypeAsync(request.BloodType);
        if (bloodStorage == null)
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 404, Message = "Blood storage not found" };
        }

        // Check if user has a previous donation within the last 3 months
        if (request.UserId.HasValue)
        {
            var lastApplication = await _repository.GetLatestByUserIdAsync(request.UserId.Value);
            if (lastApplication != null && lastApplication.DonationEndDate != null)
            {
                var minNextDonationDate = lastApplication.DonationEndDate.AddMonths(3);
                if (request.DonationEndDate < minNextDonationDate)
                {
                    return new BaseResponseModel<BloodDonationApplicationDto>
                    {
                        Code = 400,
                        Message = $"You must wait at least 3 months between donations. Your last donation was on {lastApplication.DonationEndDate:yyyy-MM-dd}."
                    };
                }
            }
        }

        if (request.EventId.HasValue)
        {
            var userEvent = new Models.Entities.UserEvents
            {
                EventId = request.EventId.Value,
                UserId = request.UserId.Value,
                UserEventsStatus = UserEventsStatus.Registered
            };
            await _userEventsRepository.AddAsync(userEvent);
        }

        var entity = new BDSS.Models.Entities.BloodDonationApplication
        {
            BloodStorageId = bloodStorage.Id,
            UserId = request.UserId,
            EventId = request.EventId,
            FullName = request.FullName,
            Dob = request.Dob,
            Gender = request.Gender,
            BloodType = request.BloodType,
            BloodTransferType = request.BloodTransferType,
            Status = BloodDonationStatus.Pending,
            Quantity = request.Quantity.Value,
            Note = request.Note,
            PhoneNumber = request.PhoneNumber,
            DonationStartDate = DateOnly.FromDateTime(DateTimeUtils.GetCurrentGmtPlus7()),
            DonationEndDate = request.DonationEndDate
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
        entity.Note = request.Note;
        await _repository.UpdateAsync(entity);

        return new BaseResponseModel<BloodDonationApplicationDto> { Code = 200, Data = ToDto(entity) };
    }

    public async Task<BaseResponseModel<BloodDonationApplicationDto>> UpdateBloodDonationApplicationAsync(UpdateBloodDonationApplicationRequest request)
    {
        var bloodStorage = await _bloodStorageRepository.GetByBloodTypeAsync(request.BloodType);
        if (bloodStorage == null)
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 404, Message = "Blood storage not found" };
        }
        var entity = await _repository.GetByIdAsync(request.Id);
        if (entity == null)
        {
            return new BaseResponseModel<BloodDonationApplicationDto> { Code = 404, Message = "Not found" };
        }
        entity.BloodStorageId = bloodStorage.Id;
        entity.BloodType = request.BloodType;
        entity.BloodTransferType = request.BloodTransferType;
        entity.Quantity = request.Quantity.Value;
        await _repository.UpdateAsync(entity);
        return new BaseResponseModel<BloodDonationApplicationDto> { Code = 200, Data = ToDto(entity) };
    }

    private static BloodDonationApplicationDto ToDto(BDSS.Models.Entities.BloodDonationApplication entity)
    {
        return new BloodDonationApplicationDto
        {
            Id = entity.Id,
            BloodStorageId = entity.BloodStorageId,
            EventId = entity.EventId,
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