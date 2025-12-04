using System.Xml.Linq;
using CasinoRoyale.Models;

namespace CasinoRoyale.Models
{
    public class BalanceRecord
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }   // amount added or removed
        public string? UserId { get; set; }
        public User? User { get; set; } = null!;
    }
}
