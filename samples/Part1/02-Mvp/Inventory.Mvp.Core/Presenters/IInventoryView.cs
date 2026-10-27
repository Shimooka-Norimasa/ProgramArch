namespace Inventory.Mvp.Presenters;

/// <summary>
/// Presenter から見た「画面」。
/// Presenter はこのインターフェースだけを知っていて、WinForms のフォームのことは知らない。
/// そのため、テストでは偽物の View（FakeView）に差し替えられる。
/// </summary>
public interface IInventoryView
{
    /// <summary>入庫ボタンが押された。</summary>
    event EventHandler? ReceiveRequested;

    /// <summary>出庫ボタンが押された。</summary>
    event EventHandler? ShipRequested;

    /// <summary>選択中の商品コード（未選択なら null）。</summary>
    string? SelectedProductCode { get; }

    /// <summary>数量欄に入力された文字列。</summary>
    string QuantityText { get; }

    void ShowProducts(IReadOnlyList<ProductRow> rows);
    void ShowMessage(string message);
    void ShowError(string message);
    void ClearQuantity();
}

/// <summary>一覧の1行に表示する内容。表示用に整えた値だけを持つ。</summary>
public record ProductRow(string Code, string Name, int Stock, int ReorderPoint, string Status);
