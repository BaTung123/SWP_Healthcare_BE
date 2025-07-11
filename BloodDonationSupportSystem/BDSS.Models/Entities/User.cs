using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class User : GenericModel
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? AvatarUrl { get; set; } = null;
        public string? Phone { get; set; } = null;
        public string? Gender { get; set; } = null;
        public BloodType? BloodType { get; set; } = null;
        public DateTime? Dob { get; set; } = null;
        public UserRole Role { get; set; } = UserRole.Customer;
        public bool IsVerified { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public virtual ICollection<UserEvents> UserEvents { get; set; } = new List<UserEvents>();
    }
}