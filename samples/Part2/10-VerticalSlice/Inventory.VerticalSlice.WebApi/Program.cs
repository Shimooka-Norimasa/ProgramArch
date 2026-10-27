using Inventory.VerticalSlice.WebApi;
using Inventory.VerticalSlice.WebApi.Data;
using Inventory.VerticalSlice.WebApi.Features;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(options => options.UseSqlite("Data Source=inventory-slice.db"));
builder.Services.AddExceptionHandler<DomainExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<InventoryDbContext>().Database.EnsureCreatedAsync();
}

app.MapFeatures();

app.Run();
