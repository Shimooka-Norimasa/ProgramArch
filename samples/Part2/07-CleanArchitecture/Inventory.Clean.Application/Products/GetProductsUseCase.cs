using Inventory.Clean.Application.Abstractions;

namespace Inventory.Clean.Application.Products;

/// <summary>ユースケース: 商品一覧を取得する。</summary>
public class GetProductsUseCase(IProductRepository repository)
{
    public async Task<IReadOnlyList<ProductDto>> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var products = await repository.GetAllAsync(cancellationToken);
        return products.Select(ProductDto.From).ToList();
    }
}
