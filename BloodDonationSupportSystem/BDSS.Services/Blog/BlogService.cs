using BDSS.DTOs;
using BDSS.DTOs.Blog;
using BDSS.Repositories.BlogRepository;

namespace BDSS.Services.Blog;

public class BlogService : IBlogService
{
    private readonly IBlogRepository _blogRepository;
    public BlogService(IBlogRepository blogRepository)
    {
        _blogRepository = blogRepository;
    }

    public async Task<BaseResponseModel<GetAllBlogsResponse>> GetAllBlogsAsync()
    {
        try
        {
            var blogs = await _blogRepository.GetAllAsync();
            var response = new GetAllBlogsResponse
            {
                Blogs = blogs.Select(b => new BlogDto
                {
                    Id = b.Id,
                    ImageUrl = b.ImageUrl,
                    Title = b.Title,
                    Description = b.Description,
                    CreatedAt = b.CreatedAt,
                    UpdatedAt = b.UpdatedAt
                })
            };
            return new BaseResponseModel<GetAllBlogsResponse> { Code = 200, Data = response };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<GetAllBlogsResponse> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BlogDto>> GetBlogByIdAsync(long id)
    {
        try
        {
            var blog = await _blogRepository.FindAsync(id);
            if (blog == null)
                return new BaseResponseModel<BlogDto> { Code = 404, Message = "Blog not found" };
            var dto = new BlogDto
            {
                Id = blog.Id,
                ImageUrl = blog.ImageUrl,
                Title = blog.Title,
                Description = blog.Description,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt
            };
            return new BaseResponseModel<BlogDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BlogDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BlogDto>> CreateBlogAsync(CreateBlogRequest request)
    {
        try
        {
            var blog = new Models.Entities.Blog
            {
                ImageUrl = request.ImageUrl,
                Title = request.Title,
                Description = request.Description
            };
            var created = await _blogRepository.AddAsync(blog);
            var dto = new BlogDto
            {
                Id = created.Id,
                ImageUrl = created.ImageUrl,
                Title = created.Title,
                Description = created.Description,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt
            };
            return new BaseResponseModel<BlogDto> { Code = 201, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BlogDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<BlogDto>> UpdateBlogAsync(UpdateBlogRequest request)
    {
        try
        {
            var blog = await _blogRepository.FindAsync(request.Id);
            if (blog == null)
                return new BaseResponseModel<BlogDto> { Code = 404, Message = "Blog not found" };
            blog.ImageUrl = request.ImageUrl;
            blog.Title = request.Title;
            blog.Description = request.Description;
            await _blogRepository.UpdateAsync(blog);
            var dto = new BlogDto
            {
                Id = blog.Id,
                ImageUrl = blog.ImageUrl,
                Title = blog.Title,
                Description = blog.Description,
                CreatedAt = blog.CreatedAt,
                UpdatedAt = blog.UpdatedAt
            };
            return new BaseResponseModel<BlogDto> { Code = 200, Data = dto };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<BlogDto> { Code = 500, Message = ex.Message };
        }
    }

    public async Task<BaseResponseModel<bool>> DeleteBlogAsync(long id)
    {
        try
        {
            var blog = await _blogRepository.FindAsync(id);
            if (blog == null)
                return new BaseResponseModel<bool> { Code = 404, Message = "Blog not found", Data = false };
            await _blogRepository.DeleteAsync(id);
            return new BaseResponseModel<bool> { Code = 200, Data = true };
        }
        catch (Exception ex)
        {
            return new BaseResponseModel<bool> { Code = 500, Message = ex.Message, Data = false };
        }
    }
}