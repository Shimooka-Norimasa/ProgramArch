using Inventory.Clean.Application.Abstractions;
using Inventory.Clean.Domain;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Clean.Infrastructure.Persistence;

/// <summary>Application 層が決めた IProductRepository を、EF Core で実装したもの。</summary>
public class EfProductRepository(InventoryDbContext db) : IProductRepository
{
    public async Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        await db.Products.OrderBy(p => p.Code).ToListAsync(cancellationToken);

    public async Task<Product?> FindByCodeAsync(string code, CancellationToken cancellationToken = default) =>
        await db.Products.FindAsync(new object[] { code }, cancellationToken);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        db.SaveChangesAsync(cancellationToken);
}
