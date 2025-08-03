namespace BDSS.DTOs.Blog;

public class BlogDto
{
    public long Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class CreateBlogRequest
{
    public string ImageUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class UpdateBlogRequest
{
    public long Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

public class DeleteBlogRequest
{
    public long Id { get; set; }
}

public class GetBlogByIdRequest
{
    public long Id { get; set; }
}

public class GetAllBlogsResponse
{
    public IEnumerable<BlogDto> Blogs { get; set; } = new List<BlogDto>();
}