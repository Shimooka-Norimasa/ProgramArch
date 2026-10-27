namespace Inventory.VerticalSlice.WebApi.Domain;

/// <summary>
/// 複数の機能で共有する業務ルールは、機能フォルダの外に置く。
/// </summary>
public class Product
{
    public Product(string code, string name, int stock, int reorderPoint)
    {
        Code = code;
        Name = name;
        Stock = stock;
        ReorderPoint = reorderPoint;
    }

    public string Code { get; private set; }
    public string Name { get; private set; }
    public int Stock { get; private set; }
    public int ReorderPoint { get; private set; }

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

public class DomainException(string message) : Exception(message);
