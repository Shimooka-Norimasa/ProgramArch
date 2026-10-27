using Inventory.Mvu;

namespace Part1.Tests;

public class MvuUpdateTests
{
    [Fact]
    public void 入庫すると新しい状態が返り元の状態は変わらない()
    {
        var before = AppState.Initial;

        var after = InventoryApp.Update(before, new ReceiveStock(10));

        Assert.Equal(130, after.SelectedProduct.Stock);
        Assert.Equal(120, before.SelectedProduct.Stock); // 元の状態はそのまま
    }

    [Fact]
    public void 在庫より多く出庫するとエラーになり在庫は変わらない()
    {
        var state = InventoryApp.Update(AppState.Initial, new SelectProduct(2));

        var after = InventoryApp.Update(state, new ShipStock(999));

        Assert.True(after.IsError);
        Assert.Equal(15, after.SelectedProduct.Stock);
    }

    [Theory]
    [InlineData("select 2", typeof(SelectProduct))]
    [InlineData("in 5", typeof(ReceiveStock))]
    [InlineData("out 3", typeof(ShipStock))]
    [InlineData("quit", typeof(Quit))]
    [InlineData("hello", typeof(InvalidInput))]
    public void 入力をメッセージに変換できる(string input, Type expected)
    {
        Assert.IsType(expected, InventoryApp.Parse(input));
    }
}
