using System.Text;
using Inventory.Mvu;

Console.OutputEncoding = Encoding.UTF8;

// MVU のループ: View で表示 → 入力を Msg に変換 → Update で新しい状態を作る → また View
var state = AppState.Initial;
while (state.IsRunning)
{
    if (!Console.IsOutputRedirected)
    {
        Console.Clear();
    }

    Console.Write(InventoryApp.View(state));
    Console.Write("> ");

    var input = Console.ReadLine();
    Msg msg = input is null ? new Quit() : InventoryApp.Parse(input);
    state = InventoryApp.Update(state, msg);
}
