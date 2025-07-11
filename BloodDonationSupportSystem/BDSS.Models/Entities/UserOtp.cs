using BDSS.Common.Utils;

namespace BDSS.Models.Entities
{
    public class UserOtp : GenericModel
    {
        public long UserId { get; set; } = 0;
        public string OtpCode { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; } = DateTimeUtils.GetCurrentGmtPlus7();
        public bool IsUsed { get; set; } = false;
        public string PurposeType { get; set; } = string.Empty;
        public virtual User User { get; set; } = null!;
    }
}