using Inventory.Cqrs.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Cqrs.WebApi.Application.Queries;

public sealed record GetReorderReportQuery;

/// <summary>発注レポートの1行。不足数の多い順に並べる。</summary>
public sealed record ReorderReportItem(string Code, string Name, int Stock, int ReorderPoint, int Shortage);

/// <summary>
/// 読み取り側: 帳票用の集計。書き込み側の Product クラスには「不足数」などという項目はないが、
/// 読み取り側は画面の都合に合わせて自由に形を決められる。
/// </summary>
public sealed class GetReorderReportHandler(InventoryDbContext db)
    : IQueryHandler<GetReorderReportQuery, IReadOnlyList<ReorderReportItem>>
{
    public async Task<IReadOnlyList<ReorderReportItem>> HandleAsync(GetReorderReportQuery query, CancellationToken cancellationToken = default) =>
        await db.Products
            .AsNoTracking()
            .Where(p => p.Stock <= p.ReorderPoint)
            .OrderByDescending(p => p.ReorderPoint - p.Stock)
            .Select(p => new ReorderReportItem(p.Code, p.Name, p.Stock, p.ReorderPoint, p.ReorderPoint - p.Stock))
            .ToListAsync(cancellationToken);
}
