namespace BDSS.DTOs.BloodStorage;

using BDSS.Common.Enums;

public class BloodStorageDto
{
    public long Id { get; set; }
    public BloodType BloodType { get; set; }
    public int Quantity { get; set; }
    public BloodStorageStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBloodStorageRequest
{
    public BloodType BloodType { get; set; }
    public int Quantity { get; set; }
    public BloodStorageStatus Status { get; set; } = BloodStorageStatus.Enough;
}

public class UpdateBloodStorageRequest
{
    public long Id { get; set; }
    public BloodType BloodType { get; set; }
    public int Quantity { get; set; }
    public BloodStorageStatus Status { get; set; }
}

public class DeleteBloodStorageRequest
{
    public long Id { get; set; }
}

public class GetBloodStorageByIdRequest
{
    public long Id { get; set; }
}

public class GetAllBloodStoragesResponse
{
    public IEnumerable<BloodStorageDto> BloodStorages { get; set; } = new List<BloodStorageDto>();
}