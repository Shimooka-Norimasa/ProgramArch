using Inventory.Mvp.Models;

namespace Inventory.Mvp.Presenters;

/// <summary>
/// Presenter: View からの通知を受け、入力をチェックし、Model を操作し、
/// 結果を View に「表示して」と指示する。画面の処理の流れはすべてここにある。
/// </summary>
public class InventoryPresenter
{
    private readonly IInventoryView _view;
    private readonly IProductRepository _repository;

    public InventoryPresenter(IInventoryView view, IProductRepository repository)
    {
        _view = view;
        _repository = repository;

        _view.ReceiveRequested += (_, _) => Execute((product, quantity) => product.Receive(quantity), "入庫");
        _view.ShipRequested += (_, _) => Execute((product, quantity) => product.Ship(quantity), "出庫");
    }

    /// <summary>画面の初期表示。</summary>
    public void Initialize() => RefreshProducts();

    private void Execute(Action<Product, int> operation, string operationName)
    {
        var code = _view.SelectedProductCode;
        if (code is null)
        {
            _view.ShowError("商品を選択してください。");
            return;
        }

        if (!int.TryParse(_view.QuantityText, out var quantity))
        {
            _view.ShowError("数量は整数で入力してください。");
            return;
        }

        var product = _repository.FindByCode(code);
        if (product is null)
        {
            _view.ShowError($"商品 {code} が見つかりません。");
            return;
        }

        try
        {
            operation(product, quantity);
        }
        catch (InventoryException ex)
        {
            _view.ShowError(ex.Message);
            return;
        }

        RefreshProducts();
        _view.ClearQuantity();
        _view.ShowMessage($"{product.Name} を {quantity} 個{operationName}しました。");
    }

    private void RefreshProducts()
    {
        var rows = _repository.GetAll()
            .Select(p => new ProductRow(p.Code, p.Name, p.Stock, p.ReorderPoint, p.NeedsReorder ? "要発注" : ""))
            .ToList();
        _view.ShowProducts(rows);
    }
}
