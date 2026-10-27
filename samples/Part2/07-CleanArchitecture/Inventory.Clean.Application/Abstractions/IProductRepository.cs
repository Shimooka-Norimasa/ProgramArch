using Inventory.Clean.Domain;

namespace Inventory.Clean.Application.Abstractions;

/// <summary>
/// 「商品をどこかから取ってきて、保存できること」という約束だけを決めたインターフェース。
/// 実装（EF Core など）は外側の Infrastructure 層に置く。これが「依存性の逆転」。
/// </summary>
public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Product?> FindByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
