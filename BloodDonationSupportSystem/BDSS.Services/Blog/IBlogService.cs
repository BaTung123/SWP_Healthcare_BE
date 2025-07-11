using BDSS.DTOs;
using BDSS.DTOs.Blog;

namespace BDSS.Services.Blog;

public interface IBlogService
{
    Task<BaseResponseModel<GetAllBlogsResponse>> GetAllBlogsAsync();
    Task<BaseResponseModel<BlogDto>> GetBlogByIdAsync(long id);
    Task<BaseResponseModel<BlogDto>> CreateBlogAsync(CreateBlogRequest request);
    Task<BaseResponseModel<BlogDto>> UpdateBlogAsync(UpdateBlogRequest request);
    Task<BaseResponseModel<bool>> DeleteBlogAsync(long id);
}