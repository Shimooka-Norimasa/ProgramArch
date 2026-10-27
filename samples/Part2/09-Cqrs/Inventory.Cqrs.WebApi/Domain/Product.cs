namespace Inventory.Cqrs.WebApi.Domain;

/// <summary>
/// 商品エンティティ。Command（書き込み）側だけが使う。
/// Query（読み取り）側はこのクラスを経由せず、必要な形のデータを直接取り出す。
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

public class ProductNotFoundException(string code) : Exception($"商品 {code} が見つかりません。");
