using Inventory.Clean.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Clean.Infrastructure.Persistence;

/// <summary>
/// EF Core の設定はすべてここ（外側）に置く。Domain の Product には EF Core の属性などを付けない。
/// </summary>
public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(product =>
        {
            product.ToTable("Products");
            product.HasKey(p => p.Code);
            product.Ignore(p => p.NeedsReorder);
            product.HasData(
                new Product("P-001", "ボールペン（黒）", 120, 50),
                new Product("P-002", "A4コピー用紙", 30, 40),
                new Product("P-003", "ホッチキス", 15, 10));
        });
    }
}
