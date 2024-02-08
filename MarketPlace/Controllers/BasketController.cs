using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public BasketController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            var productId = await _context.Basket
                .Where(bu => bu.UserBasketId == currentUser.Id)
                .Select(i => i.ProductId)
                .ToListAsync();

            List<Product>? products = new List<Product>();

            if (productId != null)
                products = _context.Products
                    .Where(item => productId.Contains(item.ProductId)).ToListAsync().GetAwaiter().GetResult();

            return products != null ?
                          View(products) :
                          Problem("Basket is null."); ;

        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            await _context.Basket
                .AddAsync(new UserBasket
                {
                    UserBasketId = currentUser.Id,
                    ProductId = id,
                    UsersBasketProducts = null
                });

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;

            var entry = _context.Basket
                    .Where(x => x.ProductId == id)
                    .Where(x => x.UserBasketId == currentUser.Id)
                    .ToListAsync().GetAwaiter().GetResult();

            if (entry != null)
            {
                foreach(var item in entry)
                    _context.Basket.Remove(item);

                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
