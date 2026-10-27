namespace Inventory.Clean.Domain;

/// <summary>
/// 商品エンティティ。在庫の業務ルールを持つ。
/// データベースや Web のことは何も知らない、ただの C# クラス。
/// </summary>
public class Product
{
    public Product(string code, string name, int stock, int reorderPoint)
    {
        if (stock < 0)
        {
            throw new DomainException("在庫数は0以上である必要があります。");
        }

        Code = code;
        Name = name;
        Stock = stock;
        ReorderPoint = reorderPoint;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public int Stock { get; private set; }
    public int ReorderPoint { get; private set; }

    public bool NeedsReorder => Stock <= ReorderPoint;

    public void Receive(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("入庫数は1以上で指定してください。");
        }

        Stock += quantity;
    }

    public void Ship(int quantity)
    {
        if (quantity <= 0)
        {
            throw new DomainException("出庫数は1以上で指定してください。");
        }

        if (quantity > Stock)
        {
            throw new DomainException($"在庫が不足しています（在庫: {Stock}、出庫: {quantity}）。");
        }

        Stock -= quantity;
    }
}
