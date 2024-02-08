using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: OrdersController
        public async Task<ActionResult> Index()
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var orders = await _context.Orders.Where(x => x.UserId == currentUser.Id)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> Buy(int productId, ushort amount)
        {
            var currentUser = _userManager.GetUserAsync(User).Result;
            var user = await _context.ApplicationUsers.FindAsync(currentUser.Id);
            var product = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            double totalOrderPrice = amount * product.Cost;

            // check if product amount is not 0 and <= amount
            var amountOfProduct = product.Amount;

            if (amount == 0)
                return View("Index");

            if (amountOfProduct < amount)
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = $"<strong>На складе нет запрашиваемого количества.<strong> <br>" +
                    $"Максимальное количество товара для покупки: {amountOfProduct} шт",
                });

            if (user.Balance < totalOrderPrice)
                return View("Error", new ErrorViewModel
                {
                    ErrorMessage = $"<strong>Недостаточно средств на балансе!<strong> <br>" +
                    $"Текущий баланс: ({user.Balance}) <br>" +
                    $"Цена заказа: ({totalOrderPrice}) <br>" +
                    $"Недостающая сумма:({user.Balance - totalOrderPrice})",
                });

            // product amount - buy amount
            // balance - total price
            product.Amount -= amount;
            user.Balance -= totalOrderPrice;

            // add order to order list
            _context.Orders.Add(
                new Models.Order()
                {
                    Amount = amount,
                    Price = product.Cost,
                    Name = product.Name,
                    ProductId = productId,
                    OrderDate = DateTime.Now,
                    TotalPrice = totalOrderPrice,
                    UserId = currentUser.Id,
                });

            //Save
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
