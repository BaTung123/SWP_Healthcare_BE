using BDSS.Models.Context;
using BDSS.Models.Entities;
using BDSS.Repositories.GenericRepository;

namespace BDSS.Repositories.BlogRepository;

public class BlogRepository : GenericRepository<Blog>, IBlogRepository
{
    public BlogRepository(BdssDbContext context) : base(context)
    {
    }
}