using Inventory.Cqrs.WebApi.Domain;
using Inventory.Cqrs.WebApi.Infrastructure;

namespace Inventory.Cqrs.WebApi.Application.Commands;

public sealed record ReceiveStockCommand(string Code, int Quantity);

/// <summary>
/// 書き込み側: エンティティを読み込み、業務ルール（Product.Receive）を通してから保存する。
/// </summary>
public sealed class ReceiveStockHandler(InventoryDbContext db) : ICommandHandler<ReceiveStockCommand>
{
    public async Task HandleAsync(ReceiveStockCommand command, CancellationToken cancellationToken = default)
    {
        var product = await db.Products.FindAsync(new object[] { command.Code }, cancellationToken)
            ?? throw new ProductNotFoundException(command.Code);

        product.Receive(command.Quantity);
        await db.SaveChangesAsync(cancellationToken);
    }
}
