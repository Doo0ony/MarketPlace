using MarketPlace.Data;
using MarketPlace.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SubCategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SubCategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SubCategory
        public async Task<IActionResult> Index()
        {
            return View(
                await _context.SubCategories.Include(s => s.Category).ToListAsync());
        }

        // GET: SubCategory/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SubCategories == null)
                return NotFound();

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);

            if (subCategory == null)
                return NotFound();

            return View(subCategory);
        }

        // GET: SubCategory/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SubCategoryId,Name,SubCategoryThumbnail,CategoryId")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategory/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SubCategories == null)
                return NotFound();

            var subCategory = await _context.SubCategories.FindAsync(id);

            if (subCategory == null)
                return NotFound();

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", subCategory.CategoryId);
            return View(subCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SubCategoryId,Name,SubCategoryThumbnail,CategoryId")] SubCategory subCategory)
        {
            if (id != subCategory.SubCategoryId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubCategoryExists(subCategory.SubCategoryId))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", subCategory.CategoryId);

            return View(subCategory);
        }

        // GET: SubCategory/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SubCategories == null)
                return NotFound();

            var subCategory = await _context.SubCategories
                .Include(s => s.Category)
                .FirstOrDefaultAsync(m => m.SubCategoryId == id);

            if (subCategory == null)
                return NotFound();

            return View(subCategory);
        }

        // POST: SubCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SubCategories == null)
                return Problem("SubCategories is null.");

            var subCategory = await _context.SubCategories.FindAsync(id);

            if (subCategory != null)
                _context.SubCategories.Remove(subCategory);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
            return (_context.SubCategories?.Any(e => e.SubCategoryId == id)).GetValueOrDefault();
        }
    }
}
