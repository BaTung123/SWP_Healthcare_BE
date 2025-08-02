namespace BDSS.DTOs.BloodBag;

using BDSS.Common.Enums;

public class BloodBagDto
{
    public long Id { get; set; }
    public string BagNumber { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public int Quantity { get; set; }
    public DateTime CollectionDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public BloodBagStatus Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBloodBagRequest
{
    public string BagNumber { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public int Quantity { get; set; }
    public DateTime CollectionDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public BloodBagStatus Status { get; set; } = BloodBagStatus.Available;
    public string? Notes { get; set; }
}

public class UpdateBloodBagRequest
{
    public long Id { get; set; }
    public string BagNumber { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public int Quantity { get; set; }
    public DateTime CollectionDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public BloodBagStatus Status { get; set; }
    public string? Notes { get; set; }
}

public class DeleteBloodBagRequest
{
    public long Id { get; set; }
}

public class GetBloodBagByIdRequest
{
    public long Id { get; set; }
}

public class GetAllBloodBagsResponse
{
    public IEnumerable<BloodBagDto> BloodBags { get; set; } = new List<BloodBagDto>();
} 