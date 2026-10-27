using Inventory.Cqrs.WebApi;
using Inventory.Cqrs.WebApi.Application;
using Inventory.Cqrs.WebApi.Application.Commands;
using Inventory.Cqrs.WebApi.Application.Queries;
using Inventory.Cqrs.WebApi.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlite("Data Source=inventory-cqrs.db"));

// Command 側
builder.Services.AddScoped<ICommandHandler<ReceiveStockCommand>, ReceiveStockHandler>();
builder.Services.AddScoped<ICommandHandler<ShipStockCommand>, ShipStockHandler>();

// Query 側
builder.Services.AddScoped<IQueryHandler<GetProductListQuery, IReadOnlyList<ProductListItem>>, GetProductListHandler>();
builder.Services.AddScoped<IQueryHandler<GetReorderReportQuery, IReadOnlyList<ReorderReportItem>>, GetReorderReportHandler>();

builder.Services.AddExceptionHandler<InventoryExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<InventoryDbContext>().Database.EnsureCreatedAsync();
}

// ===== Query: GET。データを返すだけで、状態は変えない =====
app.MapGet("/products", (IQueryHandler<GetProductListQuery, IReadOnlyList<ProductListItem>> handler, CancellationToken ct) =>
    handler.HandleAsync(new GetProductListQuery(), ct));

app.MapGet("/reports/reorder", (IQueryHandler<GetReorderReportQuery, IReadOnlyList<ReorderReportItem>> handler, CancellationToken ct) =>
    handler.HandleAsync(new GetReorderReportQuery(), ct));

// ===== Command: POST。状態を変えるだけで、データは返さない（204 No Content） =====
app.MapPost("/products/{code}/receive", async (string code, StockRequest request, ICommandHandler<ReceiveStockCommand> handler, CancellationToken ct) =>
{
    await handler.HandleAsync(new ReceiveStockCommand(code, request.Quantity), ct);
    return Results.NoContent();
});

app.MapPost("/products/{code}/ship", async (string code, StockRequest request, ICommandHandler<ShipStockCommand> handler, CancellationToken ct) =>
{
    await handler.HandleAsync(new ShipStockCommand(code, request.Quantity), ct);
    return Results.NoContent();
});

app.Run();

public record StockRequest(int Quantity);
