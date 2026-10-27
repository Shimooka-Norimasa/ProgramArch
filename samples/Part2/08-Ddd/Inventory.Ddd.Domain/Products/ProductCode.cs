using System.Text.RegularExpressions;
using Inventory.Ddd.Domain.SeedWork;

namespace Inventory.Ddd.Domain.Products;

/// <summary>
/// 値オブジェクト: 商品コード。
/// 「P-数字3桁」という形式のチェックをこのクラスに閉じ込める。
/// 一度作れば必ず正しい形式なので、使う側で毎回チェックしなくてよい。
/// </summary>
public sealed partial record ProductCode
{
    public ProductCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !Pattern().IsMatch(value))
        {
            throw new DomainException($"商品コードの形式が正しくありません: '{value}'（例: P-001）");
        }

        Value = value;
    }

    public string Value { get; }

    public override string ToString() => Value;

    [GeneratedRegex(@"^P-\d{3}$")]
    private static partial Regex Pattern();
}
