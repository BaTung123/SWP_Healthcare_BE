using BDSS.DTOs;
using BDSS.DTOs.BloodRequestApplication;
using BDSS.Models.Entities;
using BDSS.Repositories.BloodRequestApplicationRepository;
using BDSS.Common.Enums;
using BDSS.Common.Utils;
using BDSS.Repositories.BloodStorageRepository;
using BDSS.Repositories.BloodExportRepository;

namespace BDSS.Services.BloodRequestApplication;

public class BloodRequestApplicationService : IBloodRequestApplicationService
{
    private readonly IBloodRequestApplicationRepository _repository;
    private readonly IBloodExportRepository _bloodExportRepository;
    private readonly IBloodStorageRepository _bloodStorageRepository;
    public BloodRequestApplicationService(IBloodRequestApplicationRepository repository, IBloodStorageRepository bloodStorageRepository, IBloodExportRepository bloodExportRepository)
    {
        _repository = repository;
        _bloodStorageRepository = bloodStorageRepository;
        _bloodExportRepository = bloodExportRepository;
    }

    public async Task<BaseResponseModel<GetAllBloodRequestApplicationsResponse>> GetAllBloodRequestApplicationsAsync()
    {
        var entities = await _repository.GetAllAsync();
        var dtos = entities.Select(e => ToDto(e)).ToList();
        return new BaseResponseModel<GetAllBloodRequestApplicationsResponse>
        {
            Code = 200,
            Data = new GetAllBloodRequestApplicationsResponse { BloodRequestApplications = dtos }
        };
    }

    public async Task<BaseResponseModel<BloodRequestApplicationDto>> GetBloodRequestApplicationByIdAsync(long id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity == null)
        {
            return new BaseResponseModel<BloodRequestApplicationDto> { Code = 404, Message = "Not found" };
        }
        return new BaseResponseModel<BloodRequestApplicationDto> { Code = 200, Data = ToDto(entity) };
    }

    public async Task<BaseResponseModel<BloodRequestApplicationDto>> CreateBloodRequestApplicationAsync(CreateBloodRequestApplicationRequest request)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(request.FullName) || request.Quantity <= 0)
        {
            return new BaseResponseModel<BloodRequestApplicationDto> { Code = 400, Message = "FullName and Quantity are required." };
        }
        var bloodStorage = await _bloodStorageRepository.GetByBloodTypeAsync(request.BloodType);
        var entity = new BDSS.Models.Entities.BloodRequestApplication
        {
            BloodStorageId = bloodStorage.Id,
            FullName = request.FullName,
            Dob = request.Dob,
            Gender = request.Gender,
            RequestReason = request.RequestReason,
            BloodType = request.BloodType,
            BloodTransferType = request.BloodTransferType,
            Status = BloodRequestStatus.Pending,
            Quantity = request.Quantity,
            Note = request.Note,
            IsUrged = request.IsUrged,
            PhoneNumber = request.PhoneNumber,
            BloodRequestDate = request.BloodRequestDate ?? DateTimeUtils.GetCurrentGmtPlus7()
        };
        var created = await _repository.AddAsync(entity);
        return new BaseResponseModel<BloodRequestApplicationDto> { Code = 201, Data = ToDto(created) };
    }

    public async Task<BaseResponseModel<BloodRequestApplicationDto>> UpdateBloodRequestApplicationStatusAsync(UpdateBloodRequestApplicationStatusRequest request)
    {
        try
        {
            var bloodRequestApp = await _repository.GetByIdAsync(request.Id);
            var bloodExport = await _bloodExportRepository.GetByBloodRequestApplicationIdAsync(request.Id);
            var bloodStorage = await _bloodStorageRepository.GetByIdAsync(bloodExport.BloodStorageId);
            if (bloodStorage == null)
            {
                return new BaseResponseModel<BloodRequestApplicationDto> { Code = 404, Message = "Blood storage not found" };
            }
            if (bloodExport == null)
            {
                return new BaseResponseModel<BloodRequestApplicationDto> { Code = 404, Message = "Blood export request not found" };
            }
            if (bloodRequestApp == null)
            {
                return new BaseResponseModel<BloodRequestApplicationDto> { Code = 404, Message = "Blood request application not found" };
            }
            if (!IsValidStatusTransition(bloodRequestApp.Status, request.Status))
            {
                return new BaseResponseModel<BloodRequestApplicationDto> { Code = 400, Message = "Invalid status transition" };
            }
            bloodRequestApp.Status = request.Status;
            if (bloodRequestApp.Status == BloodRequestStatus.Received)
            {
                bloodExport.Status = BloodExportStatus.Exported;
                await _bloodExportRepository.UpdateAsync(bloodExport);

                bloodStorage.Quantity -= bloodRequestApp.Quantity;
                await _bloodStorageRepository.UpdateAsync(bloodStorage);
            }
            await _repository.UpdateAsync(bloodRequestApp);
            return new BaseResponseModel<BloodRequestApplicationDto> { Code = 200, Data = ToDto(bloodRequestApp) };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodRequestApplicationDto> { Code = 500, Message = ex.Message };
        }
    }

    private static BloodRequestApplicationDto ToDto(BDSS.Models.Entities.BloodRequestApplication entity)
    {
        return new BloodRequestApplicationDto
        {
            Id = entity.Id,
            BloodStorageId = entity.BloodStorageId,
            FullName = entity.FullName,
            Dob = entity.Dob,
            Gender = entity.Gender,
            RequestReason = entity.RequestReason,
            BloodType = entity.BloodType,
            BloodTransferType = entity.BloodTransferType,
            Status = entity.Status,
            Quantity = entity.Quantity,
            Note = entity.Note,
            IsUrged = entity.IsUrged,
            PhoneNumber = entity.PhoneNumber,
            BloodRequestDate = entity.BloodRequestDate
        };
    }

    private static bool IsValidStatusTransition(BDSS.Common.Enums.BloodRequestStatus current, BDSS.Common.Enums.BloodRequestStatus next)
    {
        return (current == BDSS.Common.Enums.BloodRequestStatus.Pending && (next == BDSS.Common.Enums.BloodRequestStatus.Accepted || next == BDSS.Common.Enums.BloodRequestStatus.Denied))
            || (current == BDSS.Common.Enums.BloodRequestStatus.Accepted && next == BDSS.Common.Enums.BloodRequestStatus.Received)
            || (current == next && (current == BDSS.Common.Enums.BloodRequestStatus.Received || current == BDSS.Common.Enums.BloodRequestStatus.Denied));
    }
}