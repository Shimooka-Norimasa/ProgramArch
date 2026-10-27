using Inventory.Clean.Domain;

namespace Inventory.Clean.Application.Products;

public record ProductDto(string Code, string Name, int Stock, int ReorderPoint, bool NeedsReorder)
{
    public static ProductDto From(Product product) =>
        new(product.Code, product.Name, product.Stock, product.ReorderPoint, product.NeedsReorder);
}
