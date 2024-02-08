using MarketPlace.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CatalogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Catalog
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                         View(await _context.Categories.ToListAsync()) :
                         Problem("Categories  is null.");
        }

        // GET: Catalog/SubCatalog/5
        public async Task<ActionResult> SubCatalog(int? id)
        {
            if (id == null || _context.SubCategories == null)
                return NotFound();

            var category = await _context.SubCategories
                .Where(x => x.CategoryId == id)
                .ToListAsync();

            if (category == null)
                return NotFound();

            return View(category);
        }

        // GET: Catalog/SubCatalog/Products
        public async Task<ActionResult> Products(int? id)
        {
            if (id == null || _context.SubCategories == null)
                return NotFound();

            var category = await _context.Products
                .Where(s => s.SubCategoryId == id)
                .ToListAsync();

            if (category == null)
                return NotFound();

            return View(category);
        }

        public async Task<IActionResult> ProductInfo(int id)
        {
            return View(await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id));
        }
    }
}
