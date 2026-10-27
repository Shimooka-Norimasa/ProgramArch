using Fluxor;

namespace Inventory.Flux.Store;

/// <summary>
/// Reducer: (今の状態, Action) → 新しい状態。状態が変わるのはここだけ。
/// 非同期処理や外部へのアクセスは書かない（それは Effect の仕事）。
/// </summary>
public static class InventoryReducers
{
    [ReducerMethod]
    public static InventoryState OnLoadProducts(InventoryState state, LoadProductsAction action) =>
        state with { IsLoading = true };

    [ReducerMethod]
    public static InventoryState OnLoadProductsSucceeded(InventoryState state, LoadProductsSucceededAction action) =>
        state with { IsLoading = false, Products = [.. action.Products] };

    [ReducerMethod]
    public static InventoryState OnSelectProduct(InventoryState state, SelectProductAction action) =>
        state with { SelectedCode = action.Code, Message = null, IsError = false };

    [ReducerMethod]
    public static InventoryState OnReceiveStock(InventoryState state, ReceiveStockAction action)
    {
        if (state.SelectedProduct is not { } product)
        {
            return Error(state, "商品を選択してください。");
        }

        if (action.Quantity <= 0)
        {
            return Error(state, "入庫数は1以上で指定してください。");
        }

        return ChangeStock(state, product.Stock + action.Quantity, $"{product.Name} を {action.Quantity} 個入庫しました。");
    }

    [ReducerMethod]
    public static InventoryState OnShipStock(InventoryState state, ShipStockAction action)
    {
        if (state.SelectedProduct is not { } product)
        {
            return Error(state, "商品を選択してください。");
        }

        if (action.Quantity <= 0)
        {
            return Error(state, "出庫数は1以上で指定してください。");
        }

        if (action.Quantity > product.Stock)
        {
            return Error(state, $"在庫が不足しています（在庫: {product.Stock}、出庫: {action.Quantity}）。");
        }

        return ChangeStock(state, product.Stock - action.Quantity, $"{product.Name} を {action.Quantity} 個出庫しました。");
    }

    private static InventoryState ChangeStock(InventoryState state, int newStock, string message)
    {
        var product = state.SelectedProduct!;
        return state with
        {
            Products = state.Products.Replace(product, product with { Stock = newStock }),
            Message = message,
            IsError = false,
        };
    }

    private static InventoryState Error(InventoryState state, string message) =>
        state with { Message = message, IsError = true };
}
