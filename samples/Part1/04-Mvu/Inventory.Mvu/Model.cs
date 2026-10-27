using System.Collections.Immutable;

namespace Inventory.Mvu;

// ===== Model: アプリの状態。すべて変更不可（record + ImmutableList） =====

public sealed record ProductState(string Code, string Name, int Stock, int ReorderPoint)
{
    public bool NeedsReorder => Stock <= ReorderPoint;
}

public sealed record AppState(
    ImmutableList<ProductState> Products,
    int SelectedIndex,
    string? Message,
    bool IsError,
    bool IsRunning)
{
    public ProductState SelectedProduct => Products[SelectedIndex];

    public static AppState Initial { get; } = new(
        [
            new("P-001", "ボールペン（黒）", 120, 50),
            new("P-002", "A4コピー用紙", 30, 40),
            new("P-003", "ホッチキス", 15, 10),
        ],
        SelectedIndex: 0,
        Message: null,
        IsError: false,
        IsRunning: true);
}

// ===== Msg: 「何が起きたか」を表すメッセージ =====

public abstract record Msg;
public sealed record SelectProduct(int Index) : Msg;
public sealed record ReceiveStock(int Quantity) : Msg;
public sealed record ShipStock(int Quantity) : Msg;
public sealed record InvalidInput(string Input) : Msg;
public sealed record Quit : Msg;
