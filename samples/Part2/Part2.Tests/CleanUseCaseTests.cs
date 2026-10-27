using Inventory.Clean.Application;
using Inventory.Clean.Application.Abstractions;
using Inventory.Clean.Application.Products;
using Inventory.Clean.Domain;

namespace Part2.Tests;

public class CleanUseCaseTests
{
    /// <summary>
    /// テスト用のリポジトリ。IProductRepository を実装するだけで、本物のデータベースの代わりになる。
    /// </summary>
    private sealed class InMemoryProductRepository(params Product[] products) : IProductRepository
    {
        public int SaveCount { get; private set; }

        public Task<IReadOnlyList<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
            Task.FromResult<IReadOnlyList<Product>>(products);

        public Task<Product?> FindByCodeAsync(string code, CancellationToken cancellationToken = default) =>
            Task.FromResult(products.FirstOrDefault(p => p.Code == code));

        public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SaveCount++;
            return Task.CompletedTask;
        }
    }

    [Fact]
    public async Task 入庫すると在庫が増えて保存される()
    {
        var repository = new InMemoryProductRepository(new Product("P-001", "ボールペン", 10, 5));
        var useCase = new ReceiveStockUseCase(repository);

        var result = await useCase.ExecuteAsync("P-001", 3);

        Assert.Equal(13, result.Stock);
        Assert.Equal(1, repository.SaveCount);
    }

    [Fact]
    public async Task 在庫不足で出庫すると例外になり保存されない()
    {
        var repository = new InMemoryProductRepository(new Product("P-001", "ボールペン", 2, 5));
        var useCase = new ShipStockUseCase(repository);

        await Assert.ThrowsAsync<DomainException>(() => useCase.ExecuteAsync("P-001", 3));
        Assert.Equal(0, repository.SaveCount);
    }

    [Fact]
    public async Task 存在しない商品は見つからない例外になる()
    {
        var useCase = new ShipStockUseCase(new InMemoryProductRepository());

        await Assert.ThrowsAsync<ProductNotFoundException>(() => useCase.ExecuteAsync("P-999", 1));
    }
}
