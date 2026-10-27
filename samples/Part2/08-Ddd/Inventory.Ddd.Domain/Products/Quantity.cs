using Inventory.Ddd.Domain.SeedWork;

namespace Inventory.Ddd.Domain.Products;

/// <summary>
/// 値オブジェクト: 入出庫の数量（1以上）。
/// int のままだと「0個入庫」「-5個出庫」が作れてしまうが、この型なら作れない。
/// </summary>
public sealed record Quantity
{
    public Quantity(int value)
    {
        if (value <= 0)
        {
            throw new DomainException("数量は1以上で指定してください。");
        }

        Value = value;
    }

    public int Value { get; }

    public override string ToString() => Value.ToString();
}
