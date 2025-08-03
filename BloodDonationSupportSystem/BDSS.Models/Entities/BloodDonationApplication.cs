using BDSS.Common.Enums;
using BDSS.Common.Utils;

namespace BDSS.Models.Entities
{
    public class BloodDonationApplication : GenericModel
    {
        public long? UserId { get; set; } = null;
        public long? BloodBagId { get; set; } = null;
        public long? EventId { get; set; } = null;
        public string FullName { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateOnly DonationStartDate { get; set; } = DateOnly.FromDateTime(DateTimeUtils.GetCurrentGmtPlus7());
        public DateOnly DonationEndDate { get; set; } = DateOnly.MinValue;
        public BloodType BloodType { get; set; } = BloodType.O_Positive;
        public BloodDonationStatus Status { get; set; }
        public BloodTransferType BloodTransferType { get; set; } = BloodTransferType.WholeBlood;
        public int Quantity { get; set; } = 0;
        public string Note { get; set; } = string.Empty;
        public virtual BloodBag? BloodBag { get; set; }
        public virtual Event? Event { get; set; }
        public virtual User? User { get; set; }
    }
}