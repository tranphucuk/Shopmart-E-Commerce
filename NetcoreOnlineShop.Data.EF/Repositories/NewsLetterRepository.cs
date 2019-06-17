using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class NewsLetterRepository : EFRepository<NewsLetter, int>, INewsLetterRepository
    {
        public NewsLetterRepository(AppDbContext context) : base(context)
        {
        }
    }
}