using Inventory.Mvc.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
// サンプルなのでメモリ上のリポジトリをシングルトンで共有する
builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>();

var app = builder.Build();

// URL「/{Controller}/{Action}」をコントローラーのメソッドに対応づける
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();
