using CasinoRoyale.Models;

namespace CasinoRoyale.Models

{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string? Type { get; set; } // "Add", "Win", "Loss"
        public DateTime Timestamp { get; set; }

        public int UserId { get; set; }
        public User? User { get; set; }
    }

}
