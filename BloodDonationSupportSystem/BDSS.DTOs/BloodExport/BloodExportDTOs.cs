namespace BDSS.DTOs.BloodExport;

using BDSS.Common.Enums;

public class BloodExportDto
{
    public long Id { get; set; }
    public long BloodBagId { get; set; }
    public long? BloodRequestApplicationId { get; set; }
    public string Note { get; set; } = string.Empty;
    public BloodExportStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateBloodExportRequest
{
    public long? BloodRequestApplicationId { get; set; }
    public long BloodBagId { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class UpdateBloodExportRequest
{
    public long Id { get; set; }
    public long BloodBagId { get; set; }
    public long? BloodRequestApplicationId { get; set; }
    public string Note { get; set; } = string.Empty;
    public BloodExportStatus Status { get; set; }
}

public class UpdateBloodExportStatusRequest
{
    public long Id { get; set; }
    public BloodExportStatus Status { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class DeleteBloodExportRequest
{
    public long Id { get; set; }
}

public class GetBloodExportByIdRequest
{
    public long Id { get; set; }
}

public class GetAllBloodExportsResponse
{
    public IEnumerable<BloodExportDto> BloodExports { get; set; } = new List<BloodExportDto>();
}