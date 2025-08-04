using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class BloodExport : GenericModel
    {
        public long? BloodBagId { get; set; }
        public long? BloodRequestApplicationId { get; set; }
        public string Note { get; set; } = string.Empty;
        public BloodExportStatus Status { get; set; }
        public virtual BloodBag BloodBag { get; set; } = null!;
        public virtual BloodRequestApplication? BloodRequestApplication { get; set; }
    }
}