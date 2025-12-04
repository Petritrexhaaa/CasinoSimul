using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CasinoRoyale.Models;

namespace CasinoRoyale.Data
{
    // ✅ Inherit from IdentityDbContext<User> to use your custom User class
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        // ✅ Don't redeclare DbSet<User> – it's managed by IdentityDbContext<User>
        public DbSet<BalanceRecord> BalanceRecords { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<BalanceRecord>()
                .Property(b => b.Amount)
                .HasPrecision(18, 2);
        }
    }
}
