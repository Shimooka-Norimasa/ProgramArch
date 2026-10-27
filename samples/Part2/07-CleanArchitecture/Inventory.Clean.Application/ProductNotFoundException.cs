namespace Inventory.Clean.Application;

public class ProductNotFoundException(string code) : Exception($"商品 {code} が見つかりません。");
