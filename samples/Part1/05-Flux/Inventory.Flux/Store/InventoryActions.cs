using Inventory.Flux.Models;

namespace Inventory.Flux.Store;

// Action: 「何が起きたか」を表すデータ。部品は状態を直接書き換えず、Action を Dispatch する。

public sealed record LoadProductsAction;
public sealed record LoadProductsSucceededAction(IReadOnlyList<Product> Products);
public sealed record SelectProductAction(string Code);
public sealed record ReceiveStockAction(int Quantity);
public sealed record ShipStockAction(int Quantity);
