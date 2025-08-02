using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class BloodBag : GenericModel
    {
        public string BagNumber { get; set; } = string.Empty;
        public BloodType BloodType { get; set; }
        public int Quantity { get; set; } = 0; // in ml
        public DateTime CollectionDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public BloodBagStatus Status { get; set; } = BloodBagStatus.Available;
        public string? Notes { get; set; }
        
        // Navigation properties
        public virtual ICollection<BloodImport> BloodImports { get; set; } = new List<BloodImport>();
        public virtual ICollection<BloodExport> BloodExports { get; set; } = new List<BloodExport>();
    }
} 