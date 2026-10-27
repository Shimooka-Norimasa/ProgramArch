using CommunityToolkit.Mvvm.ComponentModel;
using Inventory.Mvvm.Models;

namespace Inventory.Mvvm.ViewModels;

/// <summary>
/// 一覧の1行分の ViewModel。Model（Product）を包み、
/// 値が変わったことを画面に通知する（PropertyChanged）役割を持つ。
/// </summary>
public partial class ProductItemViewModel(Product product) : ObservableObject
{
    public string Code => product.Code;
    public string Name => product.Name;
    public int Stock => product.Stock;
    public int ReorderPoint => product.ReorderPoint;
    public string Status => product.NeedsReorder ? "要発注" : "";

    public void Receive(int quantity)
    {
        product.Receive(quantity);
        NotifyStockChanged();
    }

    public void Ship(int quantity)
    {
        product.Ship(quantity);
        NotifyStockChanged();
    }

    private void NotifyStockChanged()
    {
        // 在庫数が変わると「状態」も変わるので、両方を通知する
        OnPropertyChanged(nameof(Stock));
        OnPropertyChanged(nameof(Status));
    }
}
