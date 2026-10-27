namespace Inventory.Cqrs.WebApi.Application;

/// <summary>
/// Command（状態を変える操作）を処理する。結果のデータは返さない。
/// </summary>
public interface ICommandHandler<in TCommand>
{
    Task HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
/// Query（データを読むだけの操作）を処理する。状態は一切変えない。
/// </summary>
public interface IQueryHandler<in TQuery, TResult>
{
    Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
