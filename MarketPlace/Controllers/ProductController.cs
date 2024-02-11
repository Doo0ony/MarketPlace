using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    [Authorize(Roles = "Admin,Salesman")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Product
        public async Task<IActionResult> Index(string SortOrder, string SearchString)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            List<Product> products;

            if (_userManager.GetRolesAsync(currentUser).Result.FirstOrDefault() == "Admin")
                products = await _context.Products.Include(p => p.SubCategory)
                    .ToListAsync();
            else
                products = await _context.Products.Include(p => p.SubCategory)
                    .Where(x => x.AuthorId == currentUser.Id)
                    .ToListAsync();

            ViewData["CurrentFilter"] = SearchString;

            if (!String.IsNullOrEmpty(SearchString))
                products = products.Where(p => p.Name.Contains(SearchString)).ToList();

            ViewData["NameSortParam"] = String.IsNullOrEmpty(SortOrder) ? "name_sort" : "";
            ViewData["CostSortParam"] = SortOrder == "Type" ? "cost_sort" : "cost_sort";

            switch (SortOrder)
            {
                case "name_sort":

                default:
                    products = products.OrderBy(p => p.Name).ToList();
                    break;
                case "cost_sort":
                    products = products.OrderBy(p => p.Cost).ToList();
                    break;

            }

            return View(products);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,ProductThumbnail,Cost,SubCategoryId, Amount")] Product product)
        {
            product.AuthorId = _userManager.GetUserAsync(User).Result.Id;

            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
                return NotFound();
            
            var product = await _context.Products.FindAsync(id);
            
            if (product == null)
                return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);

            if (product.AuthorId != _userManager.GetUserAsync(User).Result.Id)
                if (_userManager.GetRolesAsync(currentUser).Result.FirstOrDefault() != "Admin")
                    return NotFound();

            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name", product.SubCategoryId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Description,ProductThumbnail,Cost,SubCategoryId,Amount")] Product product)
        {
            if (id != product.ProductId)
                return NotFound();

            var existingProduct = await _context.Products.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);

            product.AuthorId = existingProduct.AuthorId;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }

            ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "SubCategoryId", "Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.SubCategory)
                .FirstOrDefaultAsync(m => m.ProductId == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
                return Problem("Products is null.");

            var product = await _context.Products.FindAsync(id);

            if (product != null)
                _context.Products.Remove(product);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
