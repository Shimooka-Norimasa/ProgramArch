namespace Inventory.Ddd.Domain.Products;

public enum MovementType
{
    Receipt,  // 入庫
    Shipment, // 出庫
}

/// <summary>
/// エンティティ: 入出庫の履歴1件。Id で区別される（同じ数量・同じ時刻でも別の履歴）。
/// Product 集約の一部なので、Product を通してしか作れない（コンストラクターは internal）。
/// </summary>
public sealed class StockMovement
{
    internal StockMovement(MovementType type, Quantity quantity, DateTimeOffset occurredAt)
    {
        Id = Guid.NewGuid();
        Type = type;
        Quantity = quantity;
        OccurredAt = occurredAt;
    }

    public Guid Id { get; }
    public MovementType Type { get; }
    public Quantity Quantity { get; }
    public DateTimeOffset OccurredAt { get; }
}
