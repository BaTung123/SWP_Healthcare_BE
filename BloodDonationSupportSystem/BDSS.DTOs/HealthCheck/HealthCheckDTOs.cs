using BDSS.Common.Enums;

namespace BDSS.DTOs.HealthCheck;

public class HealthCheckDto
{
    public long Id { get; set; }
    public long? UserId { get; set; }
    public long? BloodDonationApplicationId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BloodPressure { get; set; } = string.Empty;
    public decimal Temperature { get; set; }
    public decimal Weight { get; set; }
    public BloodType BloodType { get; set; }
    public int HeartRate { get; set; }
    public decimal Hemoglobin { get; set; }
    public decimal Height { get; set; }
    public string HealthCheckResult { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class CreateHealthCheckRequest
{
    public long? UserId { get; set; }
    public long? BloodDonationApplicationId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BloodPressure { get; set; } = string.Empty;
    public decimal Temperature { get; set; }
    public decimal Weight { get; set; }
    public BloodType BloodType { get; set; } = BloodType.O_Positive;
    public int HeartRate { get; set; }
    public decimal Hemoglobin { get; set; }
    public decimal Height { get; set; }
    public string HealthCheckResult { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}

public class UpdateHealthCheckRequest
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string BloodPressure { get; set; } = string.Empty;
    public decimal Temperature { get; set; }
    public decimal Weight { get; set; }
    public BloodType BloodType { get; set; }
    public int HeartRate { get; set; }
    public decimal Hemoglobin { get; set; }
    public decimal Height { get; set; }
    public string HealthCheckResult { get; set; } = string.Empty;
    public string Note { get; set; } = string.Empty;
}

public class GetHealthCheckByIdRequest
{
    public long Id { get; set; }
}

public class GetAllHealthChecksResponse
{
    public List<HealthCheckDto> HealthChecks { get; set; } = new();
}

public class GetHealthChecksByUserIdRequest
{
    public long UserId { get; set; }
}

public class GetHealthChecksByBloodDonationApplicationIdRequest
{
    public long BloodDonationApplicationId { get; set; }
} 