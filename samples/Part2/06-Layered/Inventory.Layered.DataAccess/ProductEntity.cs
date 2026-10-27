namespace Inventory.Layered.DataAccess;

/// <summary>
/// データベースの Products テーブルに対応するクラス。
/// レイヤードでは、業務ロジック層がこのクラスを直接使う（第7章でこの点を問題にする）。
/// </summary>
public class ProductEntity
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public int Stock { get; set; }
    public int ReorderPoint { get; set; }
}
