using System.Collections.Immutable;
using Fluxor;
using Inventory.Flux.Models;

namespace Inventory.Flux.Store;

/// <summary>
/// Store が持つ在庫画面の状態。アプリ内でこの状態はただ1つで、すべての部品がこれを参照する。
/// </summary>
[FeatureState]
public sealed record InventoryState
{
    public bool IsLoading { get; init; }
    public ImmutableList<Product> Products { get; init; } = [];
    public string? SelectedCode { get; init; }
    public string? Message { get; init; }
    public bool IsError { get; init; }

    public Product? SelectedProduct => Products.FirstOrDefault(p => p.Code == SelectedCode);
}
