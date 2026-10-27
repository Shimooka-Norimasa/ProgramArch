using Inventory.VerticalSlice.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.VerticalSlice.WebApi.Features.Reports;

/// <summary>
/// 機能「発注レポート」。後から追加した機能だが、変更したのはこのファイルの追加と
/// FeatureEndpoints への1行だけ。他の機能のファイルには一切触れていない。
/// </summary>
public static class GetReorderReport
{
    public sealed record Response(string Code, string Name, int Stock, int ReorderPoint, int Shortage);

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/reports/reorder", HandleAsync);

    private static async Task<List<Response>> HandleAsync(InventoryDbContext db, CancellationToken ct) =>
        await db.Products
            .AsNoTracking()
            .Where(p => p.Stock <= p.ReorderPoint)
            .OrderByDescending(p => p.ReorderPoint - p.Stock)
            .Select(p => new Response(p.Code, p.Name, p.Stock, p.ReorderPoint, p.ReorderPoint - p.Stock))
            .ToListAsync(ct);
}
