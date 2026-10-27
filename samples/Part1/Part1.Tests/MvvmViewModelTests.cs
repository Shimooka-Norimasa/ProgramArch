using Inventory.Mvvm.Models;
using Inventory.Mvvm.ViewModels;

namespace Part1.Tests;

public class MvvmViewModelTests
{
    [Fact]
    public void 商品と数量がそろうまでボタンは押せない()
    {
        var vm = new MainViewModel(new InMemoryProductRepository());

        Assert.False(vm.ReceiveCommand.CanExecute(null));

        vm.SelectedProduct = vm.Products[0];
        Assert.False(vm.ReceiveCommand.CanExecute(null));

        vm.QuantityText = "5";
        Assert.True(vm.ReceiveCommand.CanExecute(null));
    }

    [Fact]
    public void 出庫すると在庫数が減り変更が通知される()
    {
        var vm = new MainViewModel(new InMemoryProductRepository());
        var item = vm.Products[0];
        var changed = new List<string?>();
        item.PropertyChanged += (_, e) => changed.Add(e.PropertyName);

        vm.SelectedProduct = item;
        vm.QuantityText = "20";
        vm.ShipCommand.Execute(null);

        Assert.Equal(100, item.Stock);
        Assert.Contains(nameof(ProductItemViewModel.Stock), changed);
        Assert.False(vm.HasError);
    }

    [Fact]
    public void 在庫不足のときはエラー状態になる()
    {
        var vm = new MainViewModel(new InMemoryProductRepository());
        vm.SelectedProduct = vm.Products[2];
        vm.QuantityText = "999";

        vm.ShipCommand.Execute(null);

        Assert.True(vm.HasError);
        Assert.Contains("在庫が不足", vm.Message);
    }
}
