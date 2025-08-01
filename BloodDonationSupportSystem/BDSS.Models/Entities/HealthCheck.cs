using BDSS.Common.Enums;
using BDSS.Common.Utils;

namespace BDSS.Models.Entities
{
    public class HealthCheck : GenericModel
    {
        public long? UserId { get; set; } = null;
        public long? BloodDonationApplicationId { get; set; } = null;
        public string FullName { get; set; } = string.Empty;
        public string BloodPressure { get; set; } = string.Empty; // mmHg (e.g., "120/80")
        public decimal Temperature { get; set; } = 0; // °C
        public decimal Weight { get; set; } = 0; // kg
        public BloodType BloodType { get; set; } = BloodType.O_Positive;
        public int HeartRate { get; set; } = 0; // beats per minute
        public decimal Hemoglobin { get; set; } = 0; // g/dL
        public decimal Height { get; set; } = 0; // cm
        public string HealthCheckResult { get; set; } = string.Empty;
        public string Note { get; set; } = string.Empty;

        // Navigation properties
        public virtual User? User { get; set; }
        public virtual BloodDonationApplication? BloodDonationApplication { get; set; }
    }
}