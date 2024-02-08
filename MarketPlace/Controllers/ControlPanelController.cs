using MarketPlace.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Controllers
{
    [Authorize(Roles = "Admin,Salesman")]
    public class ControlPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ControlPanelController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: ControlPanelController
        public async Task<ActionResult> Index()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var role = _userManager.GetRolesAsync(currentUser).Result.FirstOrDefault();

            ViewBag.Role = role;
            return View();
        }
    }
}
