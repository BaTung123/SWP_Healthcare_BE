using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace BDSS.Repositories.BlogRepository;

public class BlogRepository : GenericRepository<Blog>, IBlogRepository
{
    public BlogRepository(BdssDbContext context) : base(context) { }

    public async Task<Blog?> GetByIdAsync(long blogId)
    {
        return await Entities.FirstOrDefaultAsync(b => b.Id == blogId && !b.IsDeleted);
    }
}