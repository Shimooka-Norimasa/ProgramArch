using Inventory.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Mvc.Controllers;

/// <summary>
/// Controller: リクエストを受け取り、Model に処理を頼み、表示する View を選ぶ。
/// 在庫ルールそのもの（在庫不足の判定など）はここに書かない。
/// </summary>
public class ProductsController(IProductRepository repository) : Controller
{
    // GET /Products
    public IActionResult Index() => View(repository.GetAll());

    // POST /Products/Receive
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Receive(string code, int quantity) =>
        Execute(code, product => product.Receive(quantity), $"{quantity}個入庫しました。");

    // POST /Products/Ship
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Ship(string code, int quantity) =>
        Execute(code, product => product.Ship(quantity), $"{quantity}個出庫しました。");

    private IActionResult Execute(string code, Action<Product> operation, string successMessage)
    {
        var product = repository.FindByCode(code);
        if (product is null)
        {
            return NotFound();
        }

        try
        {
            operation(product);
            TempData["Message"] = $"{product.Name}: {successMessage}";
        }
        catch (InventoryException ex)
        {
            TempData["Error"] = ex.Message;
        }

        // POST の後は一覧へリダイレクトする（PRG パターン）。
        // ブラウザの再読み込みで同じ入出庫が二重に実行されるのを防ぐ。
        return RedirectToAction(nameof(Index));
    }
}
