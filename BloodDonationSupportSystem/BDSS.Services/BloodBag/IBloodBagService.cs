using BDSS.DTOs.BloodBag;

namespace BDSS.Services.BloodBag;

public interface IBloodBagService
{
    Task<BloodBagDto> CreateBloodBagAsync(CreateBloodBagRequest request);
    Task<BloodBagDto> GetBloodBagByIdAsync(long id);
    Task<IEnumerable<BloodBagDto>> GetAllBloodBagsAsync();
    Task<BloodBagDto> UpdateBloodBagAsync(UpdateBloodBagRequest request);
    Task<bool> DeleteBloodBagAsync(long id);
    Task<IEnumerable<BloodBagDto>> GetAvailableBloodBagsAsync();
    Task<IEnumerable<BloodBagDto>> GetBloodBagsByBloodTypeAsync(BDSS.Common.Enums.BloodType bloodType);
    Task<IEnumerable<BloodBagDto>> GetExpiredBloodBagsAsync();
}