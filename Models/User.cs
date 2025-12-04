using Microsoft.AspNetCore.Identity;

namespace CasinoRoyale.Models

{
    public class User : IdentityUser
    {
        public decimal Balance { get; set; } = 0m;
        public string Role { get; set; } = "Player";
    }
}
