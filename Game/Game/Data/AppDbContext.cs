using Microsoft.EntityFrameworkCore;
using Game.Models;

namespace Game.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Spel> Spel { get; set; }
    }
}
