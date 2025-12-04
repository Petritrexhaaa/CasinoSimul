using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using CasinoRoyale.Data;
using CasinoRoyale.Models;
using CasinoRoyale.ViewModels;
using System.Threading.Tasks;

namespace CasinoRoyale.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Register
        public IActionResult Register() => View();

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var exists = await _userManager.FindByNameAsync(model.Username);
            if (exists != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(model);
            }

            var user = new User
            {
                UserName = model.Username,
                Email = model.Email,
                Balance = 1000m,
                Role = model.Role
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);

                var balanceRecord = new BalanceRecord
                {
                    UserId = user.Id,
                    Amount = 1000m
                };

                _context.BalanceRecords.Add(balanceRecord);
                await _context.SaveChangesAsync();

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("GameHub", "Home");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View(model);
        }

        // GET: /Account/Login
        public IActionResult Login() => View();

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            return RedirectToAction("GameHub", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Deposit([FromForm] UpdateBalanceVM model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || model.Amount <= 0)
                return BadRequest();

            user.Balance += model.Amount;

            // Save to DB
            await _userManager.UpdateAsync(user);

            return Json(new
            {
                newBalance = user.Balance,
                newBalanceFormatted = string.Format("{0:C0}", user.Balance)
            });
        }

        // GET: /Account/Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
