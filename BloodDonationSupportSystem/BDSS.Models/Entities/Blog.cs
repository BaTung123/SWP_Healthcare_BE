namespace BDSS.Models.Entities
{
    public class Blog : GenericModel
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}