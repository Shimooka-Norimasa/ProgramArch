using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Mvvm.Models;

namespace Inventory.Mvvm.ViewModels;

/// <summary>
/// 画面全体の ViewModel。画面に表示する「状態」と、画面から実行できる「操作（コマンド）」を持つ。
/// View（XAML）のことは一切知らない。View がバインディングでこのクラスにつながってくる。
/// </summary>
public partial class MainViewModel : ObservableObject
{
    public MainViewModel(IProductRepository repository)
    {
        Products = new(repository.GetAll().Select(p => new ProductItemViewModel(p)));
    }

    public ObservableCollection<ProductItemViewModel> Products { get; }

    // [ObservableProperty] を付けると、変更通知付きのプロパティ SelectedProduct が自動生成される。
    // [NotifyCanExecuteChangedFor] は「この値が変わったらボタンの押せる/押せないを再判定して」という指定。
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReceiveCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShipCommand))]
    private ProductItemViewModel? _selectedProduct;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(ReceiveCommand))]
    [NotifyCanExecuteChangedFor(nameof(ShipCommand))]
    private string _quantityText = "";

    [ObservableProperty]
    private string _message = "";

    [ObservableProperty]
    private bool _hasError;

    // [RelayCommand] を付けると、ボタンにバインドできる ReceiveCommand / ShipCommand が自動生成される
    [RelayCommand(CanExecute = nameof(CanOperate))]
    private void Receive() => Execute((item, quantity) => item.Receive(quantity), "入庫");

    [RelayCommand(CanExecute = nameof(CanOperate))]
    private void Ship() => Execute((item, quantity) => item.Ship(quantity), "出庫");

    /// <summary>商品が選択され、数量が整数のときだけボタンを押せる。</summary>
    private bool CanOperate() => SelectedProduct is not null && int.TryParse(QuantityText, out _);

    private void Execute(Action<ProductItemViewModel, int> operation, string operationName)
    {
        if (SelectedProduct is not { } item || !int.TryParse(QuantityText, out var quantity))
        {
            return;
        }

        try
        {
            operation(item, quantity);
            Message = $"{item.Name} を {quantity} 個{operationName}しました。";
            HasError = false;
            QuantityText = "";
        }
        catch (InventoryException ex)
        {
            // MessageBox は出さない。エラーも「状態」として持ち、表示方法は View に任せる。
            Message = ex.Message;
            HasError = true;
        }
    }
}
