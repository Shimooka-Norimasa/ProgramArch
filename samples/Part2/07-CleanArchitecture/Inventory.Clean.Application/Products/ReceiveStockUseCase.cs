using Inventory.Clean.Application.Abstractions;

namespace Inventory.Clean.Application.Products;

/// <summary>
/// ユースケース: 入庫する。
/// 「取り出す → エンティティに処理を頼む → 保存する」という手順だけを書く。
/// 在庫のルールそのものは Domain の Product が持っている。
/// </summary>
public class ReceiveStockUseCase(IProductRepository repository)
{
    public async Task<ProductDto> ExecuteAsync(string code, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await repository.FindByCodeAsync(code, cancellationToken)
            ?? throw new ProductNotFoundException(code);

        product.Receive(quantity);
        await repository.SaveChangesAsync(cancellationToken);

        return ProductDto.From(product);
    }
}
