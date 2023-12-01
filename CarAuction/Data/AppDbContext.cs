using CarAuction.Models;
using Microsoft.EntityFrameworkCore;

namespace CarAuction.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Make> Makes { get; set; }
    }
}
