using Inventory.Layered.DataAccess;

namespace Inventory.Layered.Business;

/// <summary>
/// 業務ロジック層: 在庫の業務ルールを担当する。
/// 画面（HTTP）のことは知らないが、データアクセス層のクラスには直接依存している。
/// </summary>
public class InventoryService(ProductRepository repository)
{
    public async Task<IReadOnlyList<ProductDto>> GetProductsAsync()
    {
        var products = await repository.GetAllAsync();
        return products.Select(ProductDto.From).ToList();
    }

    public async Task<ProductDto> ReceiveAsync(string code, int quantity)
    {
        if (quantity <= 0)
        {
            throw new BusinessException("入庫数は1以上で指定してください。");
        }

        var product = await repository.FindByCodeAsync(code) ?? throw new ProductNotFoundException(code);
        product.Stock += quantity;
        await repository.SaveChangesAsync();
        return ProductDto.From(product);
    }

    public async Task<ProductDto> ShipAsync(string code, int quantity)
    {
        if (quantity <= 0)
        {
            throw new BusinessException("出庫数は1以上で指定してください。");
        }

        var product = await repository.FindByCodeAsync(code) ?? throw new ProductNotFoundException(code);
        if (quantity > product.Stock)
        {
            throw new BusinessException($"在庫が不足しています（在庫: {product.Stock}、出庫: {quantity}）。");
        }

        product.Stock -= quantity;
        await repository.SaveChangesAsync();
        return ProductDto.From(product);
    }
}
