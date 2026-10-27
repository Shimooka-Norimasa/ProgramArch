using Inventory.Layered.DataAccess;

namespace Inventory.Layered.Business;

/// <summary>
/// プレゼンテーション層に返すデータ。データベースのクラスをそのまま外に出さないために使う。
/// </summary>
public record ProductDto(string Code, string Name, int Stock, int ReorderPoint, bool NeedsReorder)
{
    public static ProductDto From(ProductEntity entity) =>
        new(entity.Code, entity.Name, entity.Stock, entity.ReorderPoint, entity.Stock <= entity.ReorderPoint);
}
