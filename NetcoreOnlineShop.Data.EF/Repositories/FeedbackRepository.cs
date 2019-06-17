using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class FeedbackRepository : EFRepository<Feedback, int>, IFeedbackRepository
    {
        public FeedbackRepository(AppDbContext context) : base(context)
        {
        }
    }
}