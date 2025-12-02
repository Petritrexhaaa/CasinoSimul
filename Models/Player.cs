using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoSimulation.Models
{
    // Represents a user/player in the casino simulation.
    public class Player
    {
        [Key]
        public int PlayerId { get; set; } // Primary Key

        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        // In a real app, you would use a secure library for hashing/salting passwords.
        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18, 2)")] // Ensure precise currency storage
        public decimal CurrentFunds { get; set; } = 1000.00m; // Default starting bankroll
    }
}