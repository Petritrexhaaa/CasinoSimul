using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CasinoRoyale.Data;
using CasinoRoyale.Models;
using System.Threading.Tasks;

namespace CasinoRoyale.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminController(UserManager<User> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult TopUpBalance()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> TopUpBalance(string userIdentifier, int amount)
        {
            if (amount <= 0)
            {
                ModelState.AddModelError("", "Amount must be greater than zero.");
                return View();
            }

            var user = await _userManager.FindByNameAsync(userIdentifier)
                        ?? await _userManager.FindByEmailAsync(userIdentifier);

            if (user == null)
            {
                ModelState.AddModelError("", "User not found.");
                return View();
            }

            user.Balance += amount;
            await _userManager.UpdateAsync(user);

            ViewBag.Message = $"✅ {amount} coins added to {user.UserName}'s balance.";
            return View();
        }
    }
}
