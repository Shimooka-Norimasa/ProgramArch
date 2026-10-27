namespace Inventory.Mvc.Models;

public interface IProductRepository
{
    IReadOnlyList<Product> GetAll();
    Product? FindByCode(string code);
}

/// <summary>
/// メモリ上に商品を保持するリポジトリ。
/// 実際のアプリではデータベースに保存する（第2部で扱う）。
/// </summary>
public class InMemoryProductRepository : IProductRepository
{
    private readonly List<Product> _products =
    [
        new("P-001", "ボールペン（黒）", 120, 50),
        new("P-002", "A4コピー用紙", 30, 40),
        new("P-003", "ホッチキス", 15, 10),
    ];

    public IReadOnlyList<Product> GetAll() => _products;

    public Product? FindByCode(string code) => _products.FirstOrDefault(p => p.Code == code);
}
