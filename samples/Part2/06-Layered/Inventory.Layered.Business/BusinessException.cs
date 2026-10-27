namespace Inventory.Layered.Business;

/// <summary>業務ルールに違反したときの例外。</summary>
public class BusinessException(string message) : Exception(message);

public class ProductNotFoundException(string code) : BusinessException($"商品 {code} が見つかりません。");
