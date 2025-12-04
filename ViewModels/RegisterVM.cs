using System.ComponentModel.DataAnnotations;

namespace CasinoRoyale.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string Username { get; set; } = null!;

        [Required, EmailAddress]
        public string Email { get; set; } = null!;

        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public string Role { get; set; } = "Player";
    }
}
