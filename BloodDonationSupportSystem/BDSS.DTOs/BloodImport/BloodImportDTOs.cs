namespace BDSS.DTOs.BloodImport;

using BDSS.Common.Enums;

public class BloodImportDto
{
    public long Id { get; set; }
    public long BloodBagId { get; set; }
    public long? BloodDonationApplicationId { get; set; }
    public string Note { get; set; } = string.Empty;
    public BloodImportStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBloodImportRequest
{
    public long BloodDonationApplicationId { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class UpdateBloodImportStatusRequest
{
    public long Id { get; set; }
    public BloodImportStatus Status { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class UpdateBloodImportRequest
{
    public long Id { get; set; }
    public long BloodBagId { get; set; }
    public long? BloodDonationApplicationId { get; set; }
    public string Note { get; set; } = string.Empty;
    public BloodImportStatus Status { get; set; }
}

public class DeleteBloodImportRequest
{
    public long Id { get; set; }
}

public class GetBloodImportByIdRequest
{
    public long Id { get; set; }
}

public class GetAllBloodImportsResponse
{
    public IEnumerable<BloodImportDto> BloodImports { get; set; } = new List<BloodImportDto>();
}