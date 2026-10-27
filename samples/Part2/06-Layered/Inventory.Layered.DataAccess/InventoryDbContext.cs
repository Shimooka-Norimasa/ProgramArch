using Microsoft.EntityFrameworkCore;

namespace Inventory.Layered.DataAccess;

public class InventoryDbContext(DbContextOptions<InventoryDbContext> options) : DbContext(options)
{
    public DbSet<ProductEntity> Products => Set<ProductEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProductEntity>(product =>
        {
            product.ToTable("Products");
            product.HasKey(p => p.Code);
            product.HasData(
                new ProductEntity { Code = "P-001", Name = "ボールペン（黒）", Stock = 120, ReorderPoint = 50 },
                new ProductEntity { Code = "P-002", Name = "A4コピー用紙", Stock = 30, ReorderPoint = 40 },
                new ProductEntity { Code = "P-003", Name = "ホッチキス", Stock = 15, ReorderPoint = 10 });
        });
    }
}
