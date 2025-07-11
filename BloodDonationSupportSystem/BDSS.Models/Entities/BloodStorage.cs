using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class BloodStorage : GenericModel
    {
        public BloodType BloodType { get; set; }
        public int Quantity { get; set; } = 0; // in ml
        public BloodStorageStatus Status { get; set; } = BloodStorageStatus.Enough;
        public virtual ICollection<BloodImport> BloodImports { get; set; } = new List<BloodImport>();
        public virtual ICollection<BloodExport> BloodExports { get; set; } = new List<BloodExport>();
    }
}