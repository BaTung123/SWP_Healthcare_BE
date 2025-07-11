using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class BloodImport : GenericModel
    {
        public long BloodStorageId { get; set; }
        public long? BloodDonationApplicationId { get; set; }
        public string Note { get; set; } = string.Empty;
        public BloodImportStatus Status { get; set; } = BloodImportStatus.Pending;
        public virtual BloodStorage BloodStorage { get; set; } = null!;
        public virtual BloodDonationApplication? BloodDonationApplication { get; set; }
    }
}