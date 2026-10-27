using Inventory.Mvp.Models;
using Inventory.Mvp.Presenters;

namespace Part1.Tests;

public class MvpPresenterTests
{
    /// <summary>テスト用の偽物の View。画面を出さずに、Presenter からの指示を記録する。</summary>
    private sealed class FakeView : IInventoryView
    {
        public event EventHandler? ReceiveRequested;
        public event EventHandler? ShipRequested;

        public string? SelectedProductCode { get; set; }
        public string QuantityText { get; set; } = "";

        public IReadOnlyList<ProductRow> Rows { get; private set; } = [];
        public string? LastMessage { get; private set; }
        public string? LastError { get; private set; }

        public void ShowProducts(IReadOnlyList<ProductRow> rows) => Rows = rows;
        public void ShowMessage(string message) => LastMessage = message;
        public void ShowError(string message) => LastError = message;
        public void ClearQuantity() => QuantityText = "";

        // ボタンを押したことにする
        public void ClickReceive() => ReceiveRequested?.Invoke(this, EventArgs.Empty);
        public void ClickShip() => ShipRequested?.Invoke(this, EventArgs.Empty);
    }

    private static (FakeView View, InventoryPresenter Presenter) CreateSut()
    {
        var view = new FakeView();
        var presenter = new InventoryPresenter(view, new InMemoryProductRepository());
        presenter.Initialize();
        return (view, presenter);
    }

    [Fact]
    public void 初期表示で商品一覧が表示される()
    {
        var (view, _) = CreateSut();

        Assert.Equal(3, view.Rows.Count);
    }

    [Fact]
    public void 入庫すると在庫数が増えて一覧が更新される()
    {
        var (view, _) = CreateSut();
        view.SelectedProductCode = "P-001";
        view.QuantityText = "10";

        view.ClickReceive();

        Assert.Equal(130, view.Rows.Single(r => r.Code == "P-001").Stock);
        Assert.Equal("", view.QuantityText);
        Assert.Null(view.LastError);
    }

    [Fact]
    public void 在庫より多く出庫するとエラーが表示される()
    {
        var (view, _) = CreateSut();
        view.SelectedProductCode = "P-003";
        view.QuantityText = "999";

        view.ClickShip();

        Assert.Contains("在庫が不足", view.LastError);
        Assert.Equal(15, view.Rows.Single(r => r.Code == "P-003").Stock);
    }

    [Fact]
    public void 数量が数字でないとエラーが表示される()
    {
        var (view, _) = CreateSut();
        view.SelectedProductCode = "P-001";
        view.QuantityText = "abc";

        view.ClickReceive();

        Assert.Equal("数量は整数で入力してください。", view.LastError);
    }
}
