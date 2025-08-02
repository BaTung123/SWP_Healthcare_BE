namespace BDSS.DTOs.BloodRequestApplication;

using BDSS.Common.Enums;

public class BloodRequestApplicationDto
{
    public long Id { get; set; }
    public long? BloodBagId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string RequestReason { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public BloodTransferType BloodTransferType { get; set; }
    public BloodRequestStatus Status { get; set; }
    public int Quantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public bool IsUrged { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime BloodRequestDate { get; set; }
}

public class CreateBloodRequestApplicationRequest
{
    public string FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string RequestReason { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public BloodTransferType BloodTransferType { get; set; }
    public int Quantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public bool IsUrged { get; set; }
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime? BloodRequestDate { get; set; }
}

public class UpdateBloodRequestApplicationStatusRequest
{
    public long Id { get; set; }
    public BloodRequestStatus Status { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class GetAllBloodRequestApplicationsResponse
{
    public List<BloodRequestApplicationDto> BloodRequestApplications { get; set; } = new();
}