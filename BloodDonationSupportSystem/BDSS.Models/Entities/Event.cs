using BDSS.Common.Enums;

namespace BDSS.Models.Entities
{
    public class Event : GenericModel
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string LocationName { get; set; } = string.Empty;
        public string LocationAddress { get; set; } = string.Empty;
        public int TargetParticipant { get; set; } = 0;
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public EventStatus Status { get; set; } = EventStatus.ComingSoon;
        public virtual ICollection<UserEvents> UserEvents { get; set; } = new List<UserEvents>();
    }
}