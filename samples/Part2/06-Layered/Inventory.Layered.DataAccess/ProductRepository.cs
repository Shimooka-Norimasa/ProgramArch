using Microsoft.EntityFrameworkCore;

namespace Inventory.Layered.DataAccess;

/// <summary>
/// データアクセス層: データベースの読み書きだけを担当する。業務ルールは書かない。
/// </summary>
public class ProductRepository(InventoryDbContext db)
{
    public async Task<IReadOnlyList<ProductEntity>> GetAllAsync() =>
        await db.Products.OrderBy(p => p.Code).ToListAsync();

    public async Task<ProductEntity?> FindByCodeAsync(string code) =>
        await db.Products.FindAsync(code);

    public Task SaveChangesAsync() => db.SaveChangesAsync();
}
