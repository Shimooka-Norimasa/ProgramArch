using Inventory.VerticalSlice.WebApi.Data;

namespace Inventory.VerticalSlice.WebApi.Features.Stock;

/// <summary>機能「出庫」。</summary>
public static class ShipStock
{
    public sealed record Request(int Quantity);
    public sealed record Response(string Code, int Stock);

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/products/{code}/ship", HandleAsync);

    private static async Task<IResult> HandleAsync(string code, Request request, InventoryDbContext db, CancellationToken ct)
    {
        var product = await db.Products.FindAsync(new object[] { code }, ct);
        if (product is null)
        {
            return Results.NotFound(new { error = $"商品 {code} が見つかりません。" });
        }

        product.Ship(request.Quantity);
        await db.SaveChangesAsync(ct);

        return Results.Ok(new Response(product.Code, product.Stock));
    }
}
