using System.Text;

namespace Inventory.Mvu;

/// <summary>
/// MVU の Update と View。どちらも「引数だけから結果を計算する」純粋な関数。
/// </summary>
public static class InventoryApp
{
    // ===== Update: (今の状態, メッセージ) → 新しい状態 =====
    // 状態が変わるのはここだけ。元の state は書き換えず、新しい state を返す。
    public static AppState Update(AppState state, Msg msg) => msg switch
    {
        SelectProduct m when m.Index >= 0 && m.Index < state.Products.Count =>
            state with { SelectedIndex = m.Index, Message = null, IsError = false },
        SelectProduct =>
            Error(state, "その番号の商品はありません。"),

        ReceiveStock m when m.Quantity <= 0 =>
            Error(state, "入庫数は1以上で指定してください。"),
        ReceiveStock m =>
            ChangeStock(state, m.Quantity, $"{m.Quantity}個入庫しました。"),

        ShipStock m when m.Quantity <= 0 =>
            Error(state, "出庫数は1以上で指定してください。"),
        ShipStock m when m.Quantity > state.SelectedProduct.Stock =>
            Error(state, $"在庫が不足しています（在庫: {state.SelectedProduct.Stock}、出庫: {m.Quantity}）。"),
        ShipStock m =>
            ChangeStock(state, -m.Quantity, $"{m.Quantity}個出庫しました。"),

        InvalidInput m =>
            Error(state, $"コマンドが分かりません: {m.Input}"),
        Quit =>
            state with { IsRunning = false },

        _ => state,
    };

    private static AppState ChangeStock(AppState state, int delta, string message)
    {
        var product = state.SelectedProduct;
        var updated = product with { Stock = product.Stock + delta };
        return state with
        {
            Products = state.Products.SetItem(state.SelectedIndex, updated),
            Message = $"{product.Name}: {message}",
            IsError = false,
        };
    }

    private static AppState Error(AppState state, string message) =>
        state with { Message = message, IsError = true };

    // ===== View: 状態 → 画面の内容 =====
    // 状態を受け取って表示内容を作るだけ。状態を変更することはない。
    public static string View(AppState state)
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== 在庫管理（MVU） ===");
        for (var i = 0; i < state.Products.Count; i++)
        {
            var p = state.Products[i];
            var cursor = i == state.SelectedIndex ? ">" : " ";
            var status = p.NeedsReorder ? "要発注" : "";
            sb.AppendLine($"{cursor} [{i + 1}] {p.Code} {p.Name,-12} 在庫:{p.Stock,4} 発注点:{p.ReorderPoint,4} {status}");
        }

        sb.AppendLine();
        if (state.Message is not null)
        {
            sb.AppendLine((state.IsError ? "[エラー] " : "") + state.Message);
        }

        sb.AppendLine("コマンド: select <番号> / in <数量> / out <数量> / quit");
        return sb.ToString();
    }

    // ===== 入力 → Msg への変換 =====
    public static Msg Parse(string input)
    {
        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return parts switch
        {
            ["quit"] => new Quit(),
            ["select", var n] when int.TryParse(n, out var number) => new SelectProduct(number - 1),
            ["in", var q] when int.TryParse(q, out var quantity) => new ReceiveStock(quantity),
            ["out", var q] when int.TryParse(q, out var quantity) => new ShipStock(quantity),
            _ => new InvalidInput(input),
        };
    }
}
