namespace BDSS.DTOs.BloodDonationApplication;

using BDSS.Common.Enums;
using BDSS.Common.Utils;

public class BloodDonationApplicationDto
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public long? BloodBagId { get; set; }
    public long? EventId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public BloodType BloodType { get; set; }
    public BloodTransferType BloodTransferType { get; set; }
    public BloodDonationStatus Status { get; set; }
    public int Quantity { get; set; } = 0;
    public string Note { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly DonationStartDate { get; set; }
    public DateOnly DonationEndDate { get; set; }
}

public class CreateBloodDonationApplicationRequest
{
    public long? UserId { get; set; }
    public long? EventId { get; set; }
    public string? FullName { get; set; } = string.Empty;
    public DateTime? Dob { get; set; }
    public string Gender { get; set; } = string.Empty;
    public BloodType BloodType { get; set; } = BloodType.O_Positive;
    public BloodTransferType BloodTransferType { get; set; } = BloodTransferType.WholeBlood;
    public int? Quantity { get; set; } = 0;
    public string Note { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateOnly DonationEndDate { get; set; } = DateOnly.FromDateTime(DateTimeUtils.GetCurrentGmtPlus7());
}

public class UpdateBloodDonationApplicationStatusRequest
{
    public long Id { get; set; }
    public BloodDonationStatus Status { get; set; }
    public string Note { get; set; } = string.Empty;
}

public class UpdateBloodDonationApplicationRequest
{
    public long Id { get; set; }
    public BloodType BloodType { get; set; } = BloodType.O_Positive;
    public BloodTransferType BloodTransferType { get; set; } = BloodTransferType.WholeBlood;
    public int? Quantity { get; set; } = 0;
}

public class GetAllBloodDonationApplicationsResponse
{
    public List<BloodDonationApplicationDto> BloodDonationApplications { get; set; } = new();
}