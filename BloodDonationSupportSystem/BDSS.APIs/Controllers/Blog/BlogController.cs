using BDSS.DTOs.Blog;
using BDSS.Services.Blog;
using Microsoft.AspNetCore.Mvc;


namespace BDSS.APIs.Controllers.Blog;

[ApiController]
[Route("api/[controller]")]
public class BlogController : ControllerBase
{
    private readonly IBlogService _blogService;
    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _blogService.GetAllBlogsAsync();
        return StatusCode(result.Code, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(long id)
    {
        var result = await _blogService.GetBlogByIdAsync(id);
        return StatusCode(result.Code, result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBlogRequest request)
    {
        var result = await _blogService.CreateBlogAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateBlogRequest request)
    {
        var result = await _blogService.UpdateBlogAsync(request);
        return StatusCode(result.Code, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(long id)
    {
        var result = await _blogService.DeleteBlogAsync(id);
        return StatusCode(result.Code, result);
    }
}