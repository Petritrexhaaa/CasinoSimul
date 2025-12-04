using Microsoft.AspNetCore.Mvc;
using CasinoRoyale.Data;
using CasinoRoyale.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace CasinoRoyale.Controllers
{
    [Authorize]
    public class SlotGameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public SlotGameController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Slot()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account"); // or handle as you prefer
            }

            ViewBag.Balance = user.Balance;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateBalance([FromBody] BalanceUpdateModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Invalid data.");

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            user.Balance += model.Amount;

            if (user.Balance < 0)
                return BadRequest("Insufficient balance.");

            await _context.SaveChangesAsync();

            return Ok(new { balance = user.Balance });
        }



        [HttpGet]
        public async Task<IActionResult> GetBalance()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            return Ok(new { balance = user.Balance });
        }



        public class BalanceUpdateModel
        {
            public decimal Amount { get; set; }
        }
    }
}
