using Inventory.VerticalSlice.WebApi.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inventory.VerticalSlice.WebApi.Data;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("Products");
            product.HasKey(p => p.Code);
            product.HasData(
                new Product("P-001", "ボールペン（黒）", 120, 50),
                new Product("P-002", "A4コピー用紙", 30, 40),
                new Product("P-003", "ホッチキス", 15, 10),
                new Product("P-004", "付箋（黄）", 8, 20));
        });
    }
}
