using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class BloodImport : GenericModel
    {
        public long? BloodBagId { get; set; }
        public long? BloodDonationApplicationId { get; set; }
        public string Note { get; set; } = string.Empty;
        public BloodImportStatus Status { get; set; }
        public virtual BloodBag BloodBag { get; set; } = null!;
        public virtual BloodDonationApplication? BloodDonationApplication { get; set; }
    }
}