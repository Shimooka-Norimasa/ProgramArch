using Microsoft.Extensions.DependencyInjection;

namespace Inventory.Layered.Business;

public static class BusinessServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessLayer(this IServiceCollection services)
    {
        services.AddScoped<InventoryService>();
        return services;
    }
}
