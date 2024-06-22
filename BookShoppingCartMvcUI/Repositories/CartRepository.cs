using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookShoppingCartMvcUI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartRepository(ApplicationDbContext db, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<bool> AddItem(int bookId, int qty)
        {
            //save cart
            //cartDetail -> error
            using var transaction = _db.Database.BeginTransaction();
            try
            {

                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return false;
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                //cart detail
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    cartItem = new CartDetail
                    {
                        BookId = bookId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception ex) { return false; }


        }
        public async Task<IEnumerable<ShoppingCart>> GetUsercart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid UserId");
            var shoppingCart = await _db.ShoppingCarts
                                    .Include(a => a.CartDetails)
                                    .ThenInclude(a => a.Book)
                                    .ThenInclude(a => a.Genre)
                                    .Where(a => a.UserId == userId).ToListAsync();
            return shoppingCart;
        }
        public async Task<bool> RemoveItem(int bookId)
        {
            //save cart
            //cartDetail -> error
            using var transaction = _db.Database.BeginTransaction();
            try
            {

                string userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    return false;
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    return false;
                }
                //cart detail
                var cartItem = _db.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.BookId == bookId);
                if (cartItem is null)  
                    return false; 
                else if (cartItem.Quantity == 1)
                {
                    _db.CartDetails.Remove(cartItem);
                }
                else
                {
                   cartItem.Quantity= cartItem.Quantity-1;
                }
                _db.SaveChanges();
               // transaction.Commit();
                return true;
            }
            catch (Exception ex) { return false; }


        }

        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }

    

        private string GetUserId()
        {
            var pricipal = _httpContextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(pricipal);
            return userId;
        }
    }

}
