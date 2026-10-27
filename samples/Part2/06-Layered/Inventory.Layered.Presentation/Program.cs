using Inventory.Layered.Business;
using Inventory.Layered.DataAccess;
using Inventory.Layered.Presentation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataAccessLayer("Data Source=inventory-layered.db");
builder.Services.AddBusinessLayer();
builder.Services.AddExceptionHandler<BusinessExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
await app.Services.EnsureDatabaseCreatedAsync();

// プレゼンテーション層: HTTP の入出力だけを担当し、処理は業務ロジック層に任せる
app.MapGet("/products", (InventoryService service) => service.GetProductsAsync());

app.MapPost("/products/{code}/receive", (string code, StockRequest request, InventoryService service) =>
    service.ReceiveAsync(code, request.Quantity));

app.MapPost("/products/{code}/ship", (string code, StockRequest request, InventoryService service) =>
    service.ShipAsync(code, request.Quantity));

app.Run();

public record StockRequest(int Quantity);
