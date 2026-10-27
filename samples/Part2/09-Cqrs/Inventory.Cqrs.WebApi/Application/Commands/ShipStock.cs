using Inventory.Cqrs.WebApi.Domain;
using Inventory.Cqrs.WebApi.Infrastructure;

namespace Inventory.Cqrs.WebApi.Application.Commands;

public sealed record ShipStockCommand(string Code, int Quantity);

public sealed class ShipStockHandler(InventoryDbContext db) : ICommandHandler<ShipStockCommand>
{
    public async Task HandleAsync(ShipStockCommand command, CancellationToken cancellationToken = default)
    {
        var product = await db.Products.FindAsync(new object[] { command.Code }, cancellationToken)
            ?? throw new ProductNotFoundException(command.Code);

        product.Ship(command.Quantity);
        await db.SaveChangesAsync(cancellationToken);
    }
}
