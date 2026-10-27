namespace Inventory.Ddd.Domain.SeedWork;

/// <summary>業務ルールに違反したときの例外。</summary>
public class DomainException(string message) : Exception(message);
