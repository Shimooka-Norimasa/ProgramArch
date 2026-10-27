using Inventory.Ddd.Domain.SeedWork;

namespace Inventory.Ddd.Domain.Products;

/// <summary>
/// 値オブジェクト: 在庫数（0以上）。
/// 値を書き換えるのではなく、計算結果として新しい StockQuantity を返す（不変）。
/// </summary>
public sealed record StockQuantity
{
    public StockQuantity(int value)
    {
        if (value < 0)
        {
            throw new DomainException("在庫数は0以上である必要があります。");
        }

        Value = value;
    }

    public int Value { get; }

    public StockQuantity Add(Quantity quantity) => new(Value + quantity.Value);

    public StockQuantity Subtract(Quantity quantity)
    {
        if (quantity.Value > Value)
        {
            throw new DomainException($"在庫が不足しています（在庫: {Value}、出庫: {quantity.Value}）。");
        }

        return new(Value - quantity.Value);
    }

    public bool IsAtOrBelow(StockQuantity other) => Value <= other.Value;

    public override string ToString() => Value.ToString();
}
