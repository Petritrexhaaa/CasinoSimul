using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CasinoSimulation.Models
{
    // Tracks the results of every game played for statistics.
    public class GameHistory
    {
        [Key]
        public int HistoryId { get; set; } // Primary Key

        // Foreign key relationship to the Player model
        public int PlayerId { get; set; }
        public Player Player { get; set; } = default!; // Navigation property

        [Required]
        [StringLength(20)]
        public string GameType { get; set; } = string.Empty; // e.g., "Blackjack", "Craps"

        [Column(TypeName = "decimal(18, 2)")]
        public decimal BetAmount { get; set; }

        [Required]
        [StringLength(20)]
        public string Result { get; set; } = string.Empty; // e.g., "Win", "Loss", "Push"

        [Column(TypeName = "decimal(18, 2)")]
        // The net change in funds (+10.00 for a win, -10.00 for a loss)
        public decimal PayoutChange { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}