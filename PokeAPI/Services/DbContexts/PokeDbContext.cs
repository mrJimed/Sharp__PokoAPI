using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokeAPI.Models;

namespace PokeAPI.Services.DbContexts
{
    public class PokeDbContext : DbContext
    {
        public DbSet<FightStat> Statistics { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;

        public PokeDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
