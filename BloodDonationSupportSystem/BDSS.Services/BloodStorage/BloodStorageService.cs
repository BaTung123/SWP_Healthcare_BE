using BDSS.DTOs;
using BDSS.DTOs.BloodStorage;
using BDSS.Repositories.BloodStorageRepository;

namespace BDSS.Services.BloodStorage;

public class BloodStorageService : IBloodStorageService
{
    private readonly IBloodStorageRepository _repository;
    public BloodStorageService(IBloodStorageRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponseModel<GetAllBloodStoragesResponse>> GetAllBloodStoragesAsync()
    {
        try
        {
            var bloodStorages = await _repository.GetAllAsync();
            var response = new GetAllBloodStoragesResponse
            {
                BloodStorages = bloodStorages.Select(b => new BloodStorageDto
                {
                    Id = b.Id,
                    BloodType = b.BloodType,
                    Quantity = b.Quantity,
                    Status = b.Status,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
            };
            return new BaseResponseModel<GetAllBloodStoragesResponse> { Code = 200, Data = response };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllBloodStoragesResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodStorageDto>> GetBloodStorageByIdAsync(long id)
    {
        try
        {
            var bloodStorage = await _repository.GetByIdAsync(id);
            if (bloodStorage == null)
                return new BaseResponseModel<BloodStorageDto> { Code = 404, Message = "BloodStorage not found" };
            var dto = new BloodStorageDto
            {
                Id = bloodStorage.Id,
                BloodType = bloodStorage.BloodType,
                Quantity = bloodStorage.Quantity,
                Status = bloodStorage.Status,
                CreatedAt = bloodStorage.CreatedAt,
                UpdatedAt = bloodStorage.UpdatedAt
            };
            return new BaseResponseModel<BloodStorageDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodStorageDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodStorageDto>> CreateBloodStorageAsync(CreateBloodStorageRequest request)
    {
        try
        {
            var bloodStorage = new BDSS.Models.Entities.BloodStorage
            {
                BloodType = request.BloodType,
                Quantity = request.Quantity,
                Status = request.Status
            };
            var created = await _repository.AddAsync(bloodStorage);
            var dto = new BloodStorageDto
            {
                Id = created.Id,
                BloodType = created.BloodType,
                Quantity = created.Quantity,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
            return new BaseResponseModel<BloodStorageDto> { Code = 201, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodStorageDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BloodStorageDto>> UpdateBloodStorageAsync(UpdateBloodStorageRequest request)
    {
        try
        {
            var bloodStorage = await _repository.GetByIdAsync(request.Id);
            if (bloodStorage == null)
                return new BaseResponseModel<BloodStorageDto> { Code = 404, Message = "BloodStorage not found" };
            if (!IsValidStatusTransition(bloodStorage.Status, request.Status))
            {
                return new BaseResponseModel<BloodStorageDto> { Code = 400, Message = "Invalid status transition." };
            }
            bloodStorage.BloodType = request.BloodType;
            bloodStorage.Quantity = request.Quantity;
            bloodStorage.Status = request.Status;
            await _repository.UpdateAsync(bloodStorage);
            var dto = new BloodStorageDto
            {
                Id = bloodStorage.Id,
                BloodType = bloodStorage.BloodType,
                Quantity = bloodStorage.Quantity,
                Status = bloodStorage.Status,
                CreatedAt = bloodStorage.CreatedAt,
                UpdatedAt = bloodStorage.UpdatedAt
            };
            return new BaseResponseModel<BloodStorageDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BloodStorageDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<bool>> DeleteBloodStorageAsync(long id)
    {
        try
        {
            var bloodStorage = await _repository.GetByIdAsync(id);
            if (bloodStorage == null)
                return new BaseResponseModel<bool> { Code = 404, Message = "BloodStorage not found", Data = false };
            await _repository.DeleteAsync(id);
            return new BaseResponseModel<bool> { Code = 200, Data = true };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<bool> { Code = 500, Message = ex.Message, Data = false };
        }
    }

    private static bool IsValidStatusTransition(BDSS.Common.Enums.BloodStorageStatus current, BDSS.Common.Enums.BloodStorageStatus next)
    {
        return (current == BDSS.Common.Enums.BloodStorageStatus.NotEnough && next == BDSS.Common.Enums.BloodStorageStatus.Enough)
            || (current == BDSS.Common.Enums.BloodStorageStatus.Enough && next == BDSS.Common.Enums.BloodStorageStatus.NotEnough)
            || (current == next);
    }
}