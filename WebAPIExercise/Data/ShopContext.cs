using Microsoft.EntityFrameworkCore;
using WebAPIExercise.Data.Models;

namespace WebAPIExercise.Data
{
    public class ShopContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public ShopContext(DbContextOptions<ShopContext> options) : base(options) {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>().HasOne(item => item.Order).WithMany().OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<OrderItem>().HasOne(item => item.Product).WithMany().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
