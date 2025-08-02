using BDSS.Common.Enums;
using BDSS.Common.Utils;

namespace BDSS.Models.Entities
{
    public class BloodRequestApplication : GenericModel
    {
        public long? BloodBagId { get; set; } = null;
        public string FullName { get; set; } = string.Empty;
        public DateTime? Dob { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string RequestReason { get; set; } = string.Empty;
        public BloodType BloodType { get; set; } = BloodType.O_Positive;
        public BloodTransferType BloodTransferType { get; set; } = BloodTransferType.WholeBlood;
        public BloodRequestStatus Status { get; set; } = BloodRequestStatus.Pending;
        public int Quantity { get; set; } = 0;
        public string Note { get; set; } = string.Empty;
        public bool IsUrged { get; set; } = false;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime BloodRequestDate { get; set; } = DateTimeUtils.GetCurrentGmtPlus7();
        public virtual BloodBag? BloodBag { get; set; }
    }
}