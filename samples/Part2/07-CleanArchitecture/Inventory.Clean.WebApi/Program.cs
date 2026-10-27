using Inventory.Clean.Application.Products;
using Inventory.Clean.Infrastructure;
using Inventory.Clean.WebApi;

var builder = WebApplication.CreateBuilder(args);

// ===== 組み立て: どのインターフェースにどの実装を使うかを決めるのはここだけ =====
builder.Services.AddInfrastructure("Data Source=inventory-clean.db");
builder.Services.AddScoped<GetProductsUseCase>();
builder.Services.AddScoped<ReceiveStockUseCase>();
builder.Services.AddScoped<ShipStockUseCase>();
builder.Services.AddExceptionHandler<InventoryExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
await app.Services.EnsureDatabaseCreatedAsync();

// ===== エンドポイント: HTTP とユースケースの橋渡しだけをする =====
app.MapGet("/products", (GetProductsUseCase useCase, CancellationToken ct) =>
    useCase.ExecuteAsync(ct));

app.MapPost("/products/{code}/receive", (string code, StockRequest request, ReceiveStockUseCase useCase, CancellationToken ct) =>
    useCase.ExecuteAsync(code, request.Quantity, ct));

app.MapPost("/products/{code}/ship", (string code, StockRequest request, ShipStockUseCase useCase, CancellationToken ct) =>
    useCase.ExecuteAsync(code, request.Quantity, ct));

app.Run();

public record StockRequest(int Quantity);
