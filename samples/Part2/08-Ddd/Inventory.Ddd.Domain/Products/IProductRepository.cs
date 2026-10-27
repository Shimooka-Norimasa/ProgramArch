namespace Inventory.Ddd.Domain.Products;

/// <summary>
/// リポジトリ: 集約単位で保存・復元する。
/// StockMovement 用のリポジトリは作らない（集約の外から履歴だけを操作させないため）。
/// </summary>
public interface IProductRepository
{
    Task<Product?> FindAsync(ProductCode code, CancellationToken cancellationToken = default);
    Task AddAsync(Product product, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
