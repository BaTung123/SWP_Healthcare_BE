using BDSS.DTOs.BloodBag;
using BDSS.Models.Entities;
using BDSS.Repositories.BloodBagRepository;
using BDSS.Common.Enums;

namespace BDSS.Services.BloodBag;

public class BloodBagService : IBloodBagService
{
    private readonly IBloodBagRepository _bloodBagRepository;

    public BloodBagService(IBloodBagRepository bloodBagRepository)
    {
        _bloodBagRepository = bloodBagRepository;
    }

    public async Task<BloodBagDto> CreateBloodBagAsync(CreateBloodBagRequest request)
    {
        var bloodBag = new Models.Entities.BloodBag
        {
            BagNumber = request.BagNumber,
            BloodType = request.BloodType,
            Quantity = request.Quantity,
            CollectionDate = request.CollectionDate,
            ExpiryDate = request.ExpiryDate,
            Status = request.Status,
            Notes = request.Notes
        };

        var createdBloodBag = await _bloodBagRepository.AddAsync(bloodBag);
        return MapToDto(createdBloodBag);
    }

    public async Task<BloodBagDto> GetBloodBagByIdAsync(long id)
    {
        var bloodBag = await _bloodBagRepository.FindAsync(id);
        if (bloodBag == null)
            throw new ArgumentException("Blood bag not found");

        return MapToDto(bloodBag);
    }

    public async Task<IEnumerable<BloodBagDto>> GetAllBloodBagsAsync()
    {
        var bloodBags = await _bloodBagRepository.GetAllAsync();
        return bloodBags.Select(MapToDto);
    }

    public async Task<BloodBagDto> UpdateBloodBagAsync(UpdateBloodBagRequest request)
    {
        var bloodBag = await _bloodBagRepository.FindAsync(request.Id);
        if (bloodBag == null)
            throw new ArgumentException("Blood bag not found");

        bloodBag.BagNumber = request.BagNumber;
        bloodBag.BloodType = request.BloodType;
        bloodBag.Quantity = request.Quantity;
        bloodBag.CollectionDate = request.CollectionDate;
        bloodBag.ExpiryDate = request.ExpiryDate;
        bloodBag.Status = request.Status;
        bloodBag.Notes = request.Notes;

        await _bloodBagRepository.UpdateAsync(bloodBag);
        var updatedBloodBag = await _bloodBagRepository.FindAsync(request.Id);
        return MapToDto(updatedBloodBag);
    }

    public async Task<bool> DeleteBloodBagAsync(long id)
    {
        var bloodBag = await _bloodBagRepository.FindAsync(id);
        if (bloodBag == null)
            return false;

        await _bloodBagRepository.DeleteAsync(bloodBag.Id);
        return true;
    }

    public async Task<IEnumerable<BloodBagDto>> GetAvailableBloodBagsAsync()
    {
        var bloodBags = await _bloodBagRepository.GetAvailableBloodBagsAsync();
        return bloodBags.Select(MapToDto);
    }

    public async Task<IEnumerable<BloodBagDto>> GetBloodBagsByBloodTypeAsync(BloodType bloodType)
    {
        var bloodBags = await _bloodBagRepository.GetBloodBagsByBloodTypeAsync(bloodType);
        return bloodBags.Select(MapToDto);
    }

    public async Task<IEnumerable<BloodBagDto>> GetExpiredBloodBagsAsync()
    {
        var bloodBags = await _bloodBagRepository.GetExpiredBloodBagsAsync();
        return bloodBags.Select(MapToDto);
    }

    private static BloodBagDto MapToDto(BDSS.Models.Entities.BloodBag bloodBag)
    {
        return new BDSS.DTOs.BloodBag.BloodBagDto
        {
            Id = bloodBag.Id,
            BagNumber = bloodBag.BagNumber,
            BloodType = bloodBag.BloodType,
            Quantity = bloodBag.Quantity,
            CollectionDate = bloodBag.CollectionDate,
            ExpiryDate = bloodBag.ExpiryDate,
            Status = bloodBag.Status,
            Notes = bloodBag.Notes,
            CreatedAt = bloodBag.CreatedAt,
            UpdatedAt = bloodBag.UpdatedAt
        };
    }
}