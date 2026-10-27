using Inventory.Clean.Application.Abstractions;
using Inventory.Clean.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Clean.Infrastructure;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<InventoryDbContext>(options => options.UseSqlite(connectionString));

        // 「IProductRepository が必要なら EfProductRepository を渡す」という対応づけ
        services.AddScoped<IProductRepository, EfProductRepository>();
        return services;
    }

    /// <summary>サンプル用: データベースがなければ作成し、初期データを入れる。</summary>
    public static async Task EnsureDatabaseCreatedAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();
        await db.Database.EnsureCreatedAsync();
    }
}
