using Fluxor;
using Inventory.Flux.Models;

namespace Inventory.Flux.Store;

/// <summary>
/// Effect: 非同期処理や外部アクセスなど、Reducer に書けない「副作用」を担当する。
/// 結果は新しい Action として Dispatch し、状態の更新は Reducer に任せる。
/// </summary>
public sealed class InventoryEffects(IProductRepository repository)
{
    [EffectMethod]
    public async Task HandleLoadProducts(LoadProductsAction action, IDispatcher dispatcher)
    {
        var products = await repository.GetAllAsync();
        dispatcher.Dispatch(new LoadProductsSucceededAction(products));
    }
}
