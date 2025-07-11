namespace BDSS.DTOs.UserOtp
{
    public class GenerateOtpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string PurposeType { get; set; } = string.Empty;
        public int ExpiryTimeInMinutes { get; set; } = 5; // Default 5 minutes
    }

    public class GenerateOtpResponse
    {
        public string OtpCode { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string Email { get; set; } = string.Empty;
        public string OtpCode { get; set; } = string.Empty;
        public string PurposeType { get; set; } = string.Empty;
    }

    public class VerifyOtpResponse
    {
        public bool IsValid { get; set; }
    }
}