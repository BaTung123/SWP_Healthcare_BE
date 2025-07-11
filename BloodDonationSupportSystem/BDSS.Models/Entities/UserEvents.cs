using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class UserEvents : GenericModel
    {
        public long UserId { get; set; }
        public long EventId { get; set; }
        public int Quantity { get; set; } = 0;
        public UserEventsStatus UserEventsStatus { get; set; } = UserEventsStatus.Registered;
        public virtual User? User { get; set; } = null;
        public virtual Event? Event { get; set; } = null;
    }
}