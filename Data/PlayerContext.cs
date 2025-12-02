using Microsoft.EntityFrameworkCore;
using CasinoSimulation.Models;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;

namespace CasinoSimulation.Data
{
    // The DbContext is the essential bridge between your C# models and the physical database tables.
    public class PlayerContext : DbContext
    {
        public PlayerContext(DbContextOptions<PlayerContext> options)
            : base(options)
        {
        }

        // Represents the Player table in the database.
        public DbSet<Player> Players { get; set; } = default!;

        // Represents the GameHistory table in the database.
        public DbSet<GameHistory> GameHistory { get; set; } = default!;


        // Optionally, configure model behavior here:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Enforce unique usernames (good database practice)
            modelBuilder.Entity<Player>()
                .HasIndex(p => p.Username)
                .IsUnique();

            // Set the table names explicitly
            modelBuilder.Entity<Player>().ToTable("Player");
            modelBuilder.Entity<GameHistory>().ToTable("GameHistory");
        }
    }
}