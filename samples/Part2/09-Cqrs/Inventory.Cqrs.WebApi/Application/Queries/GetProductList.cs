using Inventory.Cqrs.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Cqrs.WebApi.Application.Queries;

public sealed record GetProductListQuery;

/// <summary>一覧画面に表示する形そのままのデータ。</summary>
public sealed record ProductListItem(string Code, string Name, int Stock, string Status);

/// <summary>
/// 読み取り側: エンティティを組み立てず、画面に必要な列だけを SQL で直接取り出す。
/// AsNoTracking で「変更を追跡しない＝読むだけ」と宣言している。
/// </summary>
public sealed class GetProductListHandler(InventoryDbContext db)
    : IQueryHandler<GetProductListQuery, IReadOnlyList<ProductListItem>>
{
    public async Task<IReadOnlyList<ProductListItem>> HandleAsync(GetProductListQuery query, CancellationToken cancellationToken = default) =>
        await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Code)
            .Select(p => new ProductListItem(p.Code, p.Name, p.Stock, p.Stock <= p.ReorderPoint ? "要発注" : "適正"))
            .ToListAsync(cancellationToken);
}
