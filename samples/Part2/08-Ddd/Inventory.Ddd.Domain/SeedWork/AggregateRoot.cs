namespace Inventory.Ddd.Domain.SeedWork;

/// <summary>
/// 集約ルートの基底クラス。集約の中で起きた重要な出来事（ドメインイベント）をためておく。
/// ためたイベントは、保存のタイミングで Application 層が取り出して通知などに使う。
/// </summary>
public abstract class AggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = [];

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

/// <summary>ドメインイベント: 業務上の重要な出来事を表す。名前は過去形にする。</summary>
public interface IDomainEvent
{
    DateTimeOffset OccurredAt { get; }
}
