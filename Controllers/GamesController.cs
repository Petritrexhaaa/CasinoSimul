using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CasinoRoyale.Data;
using CasinoRoyale.Models;

public class SlotGameController : Controller
{
    private readonly UserManager<User> _userManager;
    private readonly ApplicationDbContext _context;

    public SlotGameController(UserManager<User> userManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    // Show Dice Game View
    public async Task<IActionResult> Dice()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewBag.Balance = user?.Balance ?? 0;
        return View();  // will load Views/SlotGame/Dice.cshtml
    }

    // Show Card Game View
    public async Task<IActionResult> Card()
    {
        var user = await _userManager.GetUserAsync(User);
        ViewBag.Balance = user?.Balance ?? 0;
        return View();  // will load Views/SlotGame/Card.cshtml
    }


    // Play Dice Game (POST)
    [HttpPost]
    public async Task<IActionResult> PlayLuckyRoll([FromBody] LuckyRollRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null || user.Balance < request.BetAmount)
            return BadRequest("Insufficient balance.");

        Random rnd = new();
        int die1 = rnd.Next(2, 7);
        int die2 = rnd.Next(2, 7);
        int total = die1 + die2;

        bool win = false;
        decimal payout = 0;

        if (request.Prediction == "Under" && total < 7) win = true;
        else if (request.Prediction == "Over" && total > 7) win = true;
        else if (request.Prediction == "Seven" && total == 7) win = true;

        if (win)
            payout = request.Prediction == "Seven" ? request.BetAmount * 5 : request.BetAmount * 2;

        user.Balance += win ? payout : -request.BetAmount;
        await _userManager.UpdateAsync(user);

        return Json(new
        {
            die1,
            die2,
            total,
            win,
            payout,
            balance = user.Balance
        });
    }

    // Play High Card Game (POST)
    [HttpPost]
    public async Task<IActionResult> PlayHighCard([FromBody] HighCardRequest request)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return BadRequest("User not found.");

        if (user.Balance < request.BetAmount)
            return BadRequest("Insufficient balance.");

        Random rnd = new();

        string[] values = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] suits = { "♠", "♥", "♦", "♣" };

        int playerIndex = rnd.Next(values.Length);
        int dealerIndex = rnd.Next(values.Length);

        string playerCard = values[playerIndex] + suits[rnd.Next(suits.Length)];
        string dealerCard = values[dealerIndex] + suits[rnd.Next(suits.Length)];

        string result;
        if (playerIndex > dealerIndex)
        {
            user.Balance += request.BetAmount;
            result = "win";
        }
        else if (playerIndex == dealerIndex)
        {
            result = "tie";
        }
        else
        {
            user.Balance -= request.BetAmount;
            result = "lose";
        }

        await _userManager.UpdateAsync(user);

        return Json(new
        {
            playerCard,
            dealerCard,
            result,
            balance = user.Balance
        });
    }

    // Request DTOs for model binding
    public class LuckyRollRequest
    {
        public string Prediction { get; set; } = "";
        public decimal BetAmount { get; set; }
    }

    public class HighCardRequest
    {
        public decimal BetAmount { get; set; }
    }
}
