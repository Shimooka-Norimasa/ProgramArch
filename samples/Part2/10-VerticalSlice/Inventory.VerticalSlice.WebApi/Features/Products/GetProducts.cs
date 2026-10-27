using Inventory.VerticalSlice.WebApi.Data;
using Microsoft.EntityFrameworkCore;

namespace Inventory.VerticalSlice.WebApi.Features.Products;

/// <summary>
/// 機能「商品一覧」。URL・応答の形・処理がこの1ファイルにすべて入っている。
/// </summary>
public static class GetProducts
{
    public sealed record Response(string Code, string Name, int Stock, int ReorderPoint, bool NeedsReorder);

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapGet("/products", HandleAsync);

    private static async Task<List<Response>> HandleAsync(InventoryDbContext db, CancellationToken ct) =>
        await db.Products
            .AsNoTracking()
            .OrderBy(p => p.Code)
            .Select(p => new Response(p.Code, p.Name, p.Stock, p.ReorderPoint, p.Stock <= p.ReorderPoint))
            .ToListAsync(ct);
}
