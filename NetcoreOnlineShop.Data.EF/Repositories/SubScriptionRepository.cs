using NetcoreOnlineShop.Data.Entities;
using NetcoreOnlineShop.Data.IRepositories;

namespace NetcoreOnlineShop.Data.EF.Repositories
{
    public class SubScriptionRepository : EFRepository<Subscription, int>, ISubscriptionRepository
    {
        public SubScriptionRepository(AppDbContext context) : base(context)
        {
        }
    }
}