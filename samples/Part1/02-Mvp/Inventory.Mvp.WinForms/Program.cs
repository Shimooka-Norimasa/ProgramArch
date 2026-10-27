using Inventory.Mvp.Models;
using Inventory.Mvp.Presenters;

namespace Inventory.Mvp.WinForms;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        // View・Model・Presenter をここで組み合わせる
        var form = new InventoryForm();
        var presenter = new InventoryPresenter(form, new InMemoryProductRepository());
        presenter.Initialize();

        Application.Run(form);
    }
}
