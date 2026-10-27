using Inventory.VerticalSlice.WebApi.Features.Products;
using Inventory.VerticalSlice.WebApi.Features.Reports;
using Inventory.VerticalSlice.WebApi.Features.Stock;

namespace Inventory.VerticalSlice.WebApi.Features;

/// <summary>全機能のエンドポイントを登録する。機能を足すときはここに1行追加する。</summary>
public static class FeatureEndpoints
{
    public static void MapFeatures(this IEndpointRouteBuilder app)
    {
        GetProducts.MapEndpoint(app);
        ReceiveStock.MapEndpoint(app);
        ShipStock.MapEndpoint(app);
        GetReorderReport.MapEndpoint(app);
    }
}
