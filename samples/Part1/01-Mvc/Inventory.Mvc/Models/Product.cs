namespace Inventory.Mvc.Models;

/// <summary>
/// 商品（Model）。在庫に関するルールはここに集める。
/// </summary>
public class Product(string code, string name, int stock, int reorderPoint)
{
    public string Code { get; } = code;
    public string Name { get; } = name;
    public int Stock { get; private set; } = stock;

    /// <summary>発注点。在庫数がこの値以下になったら発注が必要。</summary>
    public int ReorderPoint { get; } = reorderPoint;

    public bool NeedsReorder => Stock <= ReorderPoint;

    public void Receive(int quantity)
    {
        if (quantity <= 0)
        {
            throw new InventoryException("入庫数は1以上で指定してください。");
        }

        Stock += quantity;
    }

    public void Ship(int quantity)
    {
        if (quantity <= 0)
        {
            throw new InventoryException("出庫数は1以上で指定してください。");
        }

        if (quantity > Stock)
        {
            throw new InventoryException($"在庫が不足しています（在庫: {Stock}、出庫: {quantity}）。");
        }

        Stock -= quantity;
    }
}

/// <summary>在庫ルールに違反したときの例外。</summary>
public class InventoryException(string message) : Exception(message);
