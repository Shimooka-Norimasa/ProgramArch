using Inventory.VerticalSlice.WebApi.Data;

namespace Inventory.VerticalSlice.WebApi.Features.Stock;

/// <summary>機能「入庫」。</summary>
public static class ReceiveStock
{
    public sealed record Request(int Quantity);
    public sealed record Response(string Code, int Stock);

    public static void MapEndpoint(IEndpointRouteBuilder app) =>
        app.MapPost("/products/{code}/receive", HandleAsync);

    private static async Task<IResult> HandleAsync(string code, Request request, InventoryDbContext db, CancellationToken ct)
    {
        var product = await db.Products.FindAsync(new object[] { code }, ct);
        if (product is null)
        {
            return Results.NotFound(new { error = $"商品 {code} が見つかりません。" });
        }

        product.Receive(request.Quantity); // ルール違反は DomainException → 400 に変換される
        await db.SaveChangesAsync(ct);

        return Results.Ok(new Response(product.Code, product.Stock));
    }
}
