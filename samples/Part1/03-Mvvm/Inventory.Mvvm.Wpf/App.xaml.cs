using System.Windows;
using Inventory.Mvvm.Models;
using Inventory.Mvvm.ViewModels;

namespace Inventory.Mvvm.Wpf;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // View に ViewModel を DataContext として渡す。これで XAML のバインディングがつながる。
        var window = new MainWindow
        {
            DataContext = new MainViewModel(new InMemoryProductRepository()),
        };
        window.Show();
    }
}
