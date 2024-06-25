using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class UserOrderRepository: IUserOrderRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserOrderRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task <IEnumerable<Order>>UserOrders()
        {
            var userId=GetUserId();
            if (string.IsNullOrEmpty(userId))
                throw new Exception("User is not logged-in");
            var orders =await _db.Orders
                            .Include(x=>x.OrderDetails)
                            .ThenInclude(x=>x.Book)
                            .ThenInclude(x=>x.Genre)
                            .Where(a => a.UserId == userId)
                            .ToListAsync();
            return orders;
        }
        private string GetUserId()
        {
            var pricipal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(pricipal);
            return userId;
        }
    }
}
