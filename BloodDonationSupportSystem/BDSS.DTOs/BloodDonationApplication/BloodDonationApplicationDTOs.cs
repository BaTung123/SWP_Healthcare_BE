namespace BDSS.DTOs.BloodDonationApplication;

using BDSS.Common.Enums;

public class BloodDonationApplicationDto
{
    public long Id { get; set; }
    public long? BloodStorageId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public BloodTransferType BloodTransferType { get; set; }
    public BloodDonationStatus Status { get; set; }
    public int Quantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly DonationStartDate { get; set; }
    public DateOnly DonationEndDate { get; set; }
}

public class CreateBloodDonationApplicationRequest
{
    public long? BloodStorageId { get; set; }
    public long? UserId { get; set; }
    public long? EventId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public BloodTransferType BloodTransferType { get; set; }
    public int? Quantity { get; set; }
    public string Note { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly? DonationStartDate { get; set; }
    public DateOnly? DonationEndDate { get; set; }
}

public class UpdateBloodDonationApplicationStatusRequest
{
    public long Id { get; set; }
    public BloodDonationStatus Status { get; set; }
}

public class GetAllBloodDonationApplicationsResponse
{
    public List<BloodDonationApplicationDto> BloodDonationApplications { get; set; } = new();
}