using BDSS.Common.Utils;

namespace BDSS.Models.Entities
{
    public class GenericModel
    {
        public long Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTimeUtils.GetCurrentGmtPlus7();
        public DateTime UpdatedAt { get; set; } = DateTimeUtils.GetCurrentGmtPlus7();
    }
}