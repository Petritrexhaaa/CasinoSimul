using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CasinoRoyale.Models;

namespace CasinoRoyale.Controllers

{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<User> _userManager;

        public HomeController(ILogger<HomeController> logger, UserManager<User> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> GameHub()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role == "Admin")
                return RedirectToAction("Index", "Admin");

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                // If no user found (maybe not logged in), redirect to login or handle accordingly
                return RedirectToAction("Login", "Account");
            }

            ViewBag.Balance = user.Balance;

            return View();
        }

    }
}
