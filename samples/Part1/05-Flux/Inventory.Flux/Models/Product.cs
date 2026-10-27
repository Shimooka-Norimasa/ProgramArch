namespace Inventory.Flux.Models;

/// <summary>商品。Flux では状態を変更不可で扱うので record にする。</summary>
public sealed record Product(string Code, string Name, int Stock, int ReorderPoint)
{
    public bool NeedsReorder => Stock <= ReorderPoint;
}

public interface IProductRepository
{
    Task<IReadOnlyList<Product>> GetAllAsync();
}

public sealed class InMemoryProductRepository : IProductRepository
{
    private readonly IReadOnlyList<Product> _products =
    [
        new("P-001", "ボールペン（黒）", 120, 50),
        new("P-002", "A4コピー用紙", 30, 40),
        new("P-003", "ホッチキス", 15, 10),
    ];

    public async Task<IReadOnlyList<Product>> GetAllAsync()
    {
        // データベースや Web API からの取得を想定して、少し待つ
        await Task.Delay(300);
        return _products;
    }
}
