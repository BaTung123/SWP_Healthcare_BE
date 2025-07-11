using BDSS.Common.Utils;
using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.UserOtpRepository
{
    public class UserOtpRepository : GenericRepository<UserOtp>, IUserOtpRepository
    {
        public UserOtpRepository(BdssDbContext context) : base(context)
        {
        }

        public async Task<UserOtp> GetLatestOtpByUserIdAndPurposeAsync(long userId, string purpose)
        {
            return await Entities
                .Where(o => o.UserId == userId && o.PurposeType == purpose && !o.IsUsed)
                .OrderByDescending(o => o.CreatedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<UserOtp> GetByOtpCodeAsync(string otpCode)
        {
            return await Entities
                .FirstOrDefaultAsync(o => o.OtpCode == otpCode && !o.IsUsed);
        }

        public async Task<bool> VerifyOtpAsync(long userId, string otpCode, string purpose)
        {
            var otp = await Entities
                .FirstOrDefaultAsync(o =>
                    o.UserId == userId &&
                    o.OtpCode == otpCode &&
                    o.PurposeType == purpose &&
                    !o.IsUsed &&
                    o.ExpiryTime > DateTimeUtils.GetCurrentGmtPlus7());

            if (otp == null)
            {
                return false;
            }

            otp.IsUsed = true;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}