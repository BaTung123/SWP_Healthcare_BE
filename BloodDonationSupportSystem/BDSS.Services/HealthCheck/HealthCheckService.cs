using BDSS.Common.Utils;
using BDSS.DTOs;
using BDSS.DTOs.HealthCheck;
using BDSS.Repositories.HealthCheckRepository;

namespace BDSS.Services.HealthCheck;

public class HealthCheckService : IHealthCheckService
{
    private readonly IHealthCheckRepository _repository;

    public HealthCheckService(IHealthCheckRepository repository)
    {
        _repository = repository;
    }

    public async Task<BaseResponseModel<GetAllHealthChecksResponse>> GetAllHealthChecksAsync()
    {
        try
        {
            var entities = await _repository.GetAllAsync();
            var dtos = entities.Select(e => ToDto(e)).ToList();
            return new BaseResponseModel<GetAllHealthChecksResponse>
            {
                Code = 200,
                Data = new GetAllHealthChecksResponse { HealthChecks = dtos }
            };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllHealthChecksResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<HealthCheckDto>> GetHealthCheckByIdAsync(GetHealthCheckByIdRequest request)
    {
        try
        {
            var entity = await _repository.FindAsync(request.Id);
            if (entity == null)
            {
                return new BaseResponseModel<HealthCheckDto> { Code = 404, Message = "Health check not found" };
            }
            return new BaseResponseModel<HealthCheckDto> { Code = 200, Data = ToDto(entity) };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<HealthCheckDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<IEnumerable<HealthCheckDto>>> GetHealthChecksByUserIdAsync(GetHealthChecksByUserIdRequest request)
    {
        try
        {
            var entities = await _repository.GetByUserIdAsync(request.UserId);
            var dtos = entities.Select(ToDto);
            return new BaseResponseModel<IEnumerable<HealthCheckDto>> { Code = 200, Data = dtos };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<IEnumerable<HealthCheckDto>> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<IEnumerable<HealthCheckDto>>> GetHealthChecksByBloodDonationApplicationIdAsync(GetHealthChecksByBloodDonationApplicationIdRequest request)
    {
        try
        {
            var entities = await _repository.GetByBloodDonationApplicationIdAsync(request.BloodDonationApplicationId);
            var dtos = entities.Select(ToDto);
            return new BaseResponseModel<IEnumerable<HealthCheckDto>> { Code = 200, Data = dtos };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<IEnumerable<HealthCheckDto>> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<HealthCheckDto>> CreateHealthCheckAsync(CreateHealthCheckRequest request)
    {
        try
        {
            var entity = new BDSS.Models.Entities.HealthCheck
            {
                UserId = request.UserId,
                BloodDonationApplicationId = request.BloodDonationApplicationId,
                FullName = request.FullName,
                BloodPressure = request.BloodPressure,
                Temperature = request.Temperature,
                Weight = request.Weight,
                BloodType = request.BloodType,
                HeartRate = request.HeartRate,
                Hemoglobin = request.Hemoglobin,
                Height = request.Height,
                HealthCheckResult = request.HealthCheckResult,
                Note = request.Note
            };

            var created = await _repository.AddAsync(entity);
            return new BaseResponseModel<HealthCheckDto> { Code = 201, Data = ToDto(created) };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<HealthCheckDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<HealthCheckDto>> UpdateHealthCheckAsync(UpdateHealthCheckRequest request)
    {
        try
        {
            var entity = await _repository.FindAsync(request.Id);
            if (entity == null)
            {
                return new BaseResponseModel<HealthCheckDto> { Code = 404, Message = "Health check not found" };
            }

            entity.FullName = request.FullName;
            entity.BloodPressure = request.BloodPressure;
            entity.Temperature = request.Temperature;
            entity.Weight = request.Weight;
            entity.BloodType = request.BloodType;
            entity.HeartRate = request.HeartRate;
            entity.Hemoglobin = request.Hemoglobin;
            entity.Height = request.Height;
            entity.HealthCheckResult = request.HealthCheckResult;
            entity.Note = request.Note;
            entity.UpdatedAt = DateTimeUtils.GetCurrentGmtPlus7();

            await _repository.UpdateAsync(entity);
            return new BaseResponseModel<HealthCheckDto> { Code = 200, Data = ToDto(entity) };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<HealthCheckDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<bool>> DeleteHealthCheckAsync(long id)
    {
        try
        {
            var entity = await _repository.FindAsync(id);
            if (entity == null)
            {
                return new BaseResponseModel<bool> { Code = 404, Message = "Health check not found" };
            }

            await _repository.DeleteAsync(id);
            return new BaseResponseModel<bool> { Code = 200, Data = true, Message = "Health check deleted successfully" };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<bool> { Code = 500, Message = ex.Message };
        }
    }

    private static HealthCheckDto ToDto(BDSS.Models.Entities.HealthCheck entity)
    {
        return new HealthCheckDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            BloodDonationApplicationId = entity.BloodDonationApplicationId,
            FullName = entity.FullName,
            BloodPressure = entity.BloodPressure,
            Temperature = entity.Temperature,
            Weight = entity.Weight,
            BloodType = entity.BloodType,
            HeartRate = entity.HeartRate,
            Hemoglobin = entity.Hemoglobin,
            Height = entity.Height,
            HealthCheckResult = entity.HealthCheckResult,
            Note = entity.Note,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}