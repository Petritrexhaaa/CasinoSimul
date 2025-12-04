using System.ComponentModel.DataAnnotations;

namespace CasinoRoyale.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; } = null!;  // changed from Email

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        public bool RememberMe { get; set; }
    }

}
