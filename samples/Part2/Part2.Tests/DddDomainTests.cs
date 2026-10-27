using Inventory.Ddd.Domain.Products;
using Inventory.Ddd.Domain.SeedWork;

namespace Part2.Tests;

public class DddDomainTests
{
    private static readonly DateTimeOffset Now = new(2026, 4, 1, 9, 0, 0, TimeSpan.FromHours(9));

    private static Product CreateProduct(int stock = 30, int reorderPoint = 10) =>
        new(new ProductCode("P-001"), "ボールペン", new StockQuantity(stock), new StockQuantity(reorderPoint));

    [Theory]
    [InlineData("")]
    [InlineData("001")]
    [InlineData("P-1")]
    public void 形式が正しくない商品コードは作れない(string value)
    {
        Assert.Throws<DomainException>(() => new ProductCode(value));
    }

    [Fact]
    public void 値オブジェクトは値が同じなら等しい()
    {
        Assert.Equal(new ProductCode("P-001"), new ProductCode("P-001"));
        Assert.Equal(new Quantity(5), new Quantity(5));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-5)]
    public void 数量0以下は作れない(int value)
    {
        Assert.Throws<DomainException>(() => new Quantity(value));
    }

    [Fact]
    public void 入庫すると在庫が増え履歴が残る()
    {
        var product = CreateProduct(stock: 30);

        product.Receive(new Quantity(5), Now);

        Assert.Equal(new StockQuantity(35), product.Stock);
        var movement = Assert.Single(product.Movements);
        Assert.Equal(MovementType.Receipt, movement.Type);
    }

    [Fact]
    public void 在庫不足の出庫は失敗し在庫も履歴も変わらない()
    {
        var product = CreateProduct(stock: 3);

        Assert.Throws<DomainException>(() => product.Ship(new Quantity(5), Now));
        Assert.Equal(new StockQuantity(3), product.Stock);
        Assert.Empty(product.Movements);
    }

    [Fact]
    public void 発注点を下回った瞬間だけイベントが発生する()
    {
        var product = CreateProduct(stock: 15, reorderPoint: 10);

        product.Ship(new Quantity(3), Now);  // 15 → 12: まだ発注点より上
        Assert.Empty(product.DomainEvents);

        product.Ship(new Quantity(2), Now);  // 12 → 10: 発注点に到達
        var domainEvent = Assert.IsType<ReorderPointReached>(Assert.Single(product.DomainEvents));
        Assert.Equal(new StockQuantity(10), domainEvent.Stock);

        product.Ship(new Quantity(1), Now);  // 10 → 9: すでに下回っているので新しいイベントは出ない
        Assert.Single(product.DomainEvents);
    }
}
