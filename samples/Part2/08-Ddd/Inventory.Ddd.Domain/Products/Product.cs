using Inventory.Ddd.Domain.SeedWork;

namespace Inventory.Ddd.Domain.Products;

/// <summary>
/// 集約ルート: 商品。在庫数と入出庫履歴をひとまとまり（集約）として守る。
/// 外から在庫数や履歴を直接いじる手段はなく、必ず Receive / Ship を通る。
/// そのため「在庫数と履歴の合計が合わない」という状態は起こりえない。
/// </summary>
public sealed class Product : AggregateRoot
{
    private readonly List<StockMovement> _movements = [];

    public Product(ProductCode code, string name, StockQuantity stock, StockQuantity reorderPoint)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("商品名を入力してください。");
        }

        Code = code;
        Name = name;
        Stock = stock;
        ReorderPoint = reorderPoint;
    }

    public ProductCode Code { get; }
    public string Name { get; private set; }
    public StockQuantity Stock { get; private set; }
    public StockQuantity ReorderPoint { get; private set; }

    public IReadOnlyList<StockMovement> Movements => _movements.AsReadOnly();

    public bool NeedsReorder => Stock.IsAtOrBelow(ReorderPoint);

    public void Receive(Quantity quantity, DateTimeOffset occurredAt)
    {
        Stock = Stock.Add(quantity);
        _movements.Add(new StockMovement(MovementType.Receipt, quantity, occurredAt));
    }

    public void Ship(Quantity quantity, DateTimeOffset occurredAt)
    {
        var wasAboveReorderPoint = !NeedsReorder;

        Stock = Stock.Subtract(quantity); // 在庫不足なら StockQuantity が例外を投げる
        _movements.Add(new StockMovement(MovementType.Shipment, quantity, occurredAt));

        // 発注点を「下回った瞬間」だけイベントを出す（すでに下回っていたら出さない）
        if (wasAboveReorderPoint && NeedsReorder)
        {
            AddDomainEvent(new ReorderPointReached(Code, Stock, occurredAt));
        }
    }

    public void ChangeReorderPoint(StockQuantity reorderPoint) => ReorderPoint = reorderPoint;
}

/// <summary>ドメインイベント: 在庫が発注点に達した。</summary>
public sealed record ReorderPointReached(ProductCode Code, StockQuantity Stock, DateTimeOffset OccurredAt) : IDomainEvent;
