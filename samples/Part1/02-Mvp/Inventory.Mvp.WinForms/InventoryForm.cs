using Inventory.Mvp.Presenters;

namespace Inventory.Mvp.WinForms;

/// <summary>
/// View: 部品の配置と、Presenter への通知・Presenter からの表示指示だけを担当する。
/// if 文による判定や在庫の計算はここには一切書かない（Passive View）。
/// </summary>
/// <remarks>
/// 教材として全体を見渡せるように、デザイナーファイルを使わずコードで画面を組み立てている。
/// </remarks>
public sealed class InventoryForm : Form, IInventoryView
{
    private readonly DataGridView _grid = new()
    {
        Dock = DockStyle.Fill,
        ReadOnly = true,
        AutoGenerateColumns = false,
        AllowUserToAddRows = false,
        MultiSelect = false,
        SelectionMode = DataGridViewSelectionMode.FullRowSelect,
        AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
    };

    private readonly TextBox _quantityTextBox = new() { Width = 80 };
    private readonly Button _receiveButton = new() { Text = "入庫", AutoSize = true };
    private readonly Button _shipButton = new() { Text = "出庫", AutoSize = true };
    private readonly Label _messageLabel = new() { AutoSize = true, Padding = new Padding(8, 6, 0, 0) };

    public InventoryForm()
    {
        Text = "在庫管理（MVP）";
        Width = 720;
        Height = 400;

        _grid.Columns.AddRange(
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProductRow.Code), HeaderText = "商品コード" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProductRow.Name), HeaderText = "商品名" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProductRow.Stock), HeaderText = "在庫数" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProductRow.ReorderPoint), HeaderText = "発注点" },
            new DataGridViewTextBoxColumn { DataPropertyName = nameof(ProductRow.Status), HeaderText = "状態" });

        var operationPanel = new FlowLayoutPanel { Dock = DockStyle.Bottom, Height = 40, Padding = new Padding(4) };
        operationPanel.Controls.Add(new Label { Text = "数量:", AutoSize = true, Padding = new Padding(0, 6, 0, 0) });
        operationPanel.Controls.Add(_quantityTextBox);
        operationPanel.Controls.Add(_receiveButton);
        operationPanel.Controls.Add(_shipButton);
        operationPanel.Controls.Add(_messageLabel);

        Controls.Add(_grid);
        Controls.Add(operationPanel);

        // ボタンが押されたことを Presenter に伝えるだけ
        _receiveButton.Click += (_, _) => ReceiveRequested?.Invoke(this, EventArgs.Empty);
        _shipButton.Click += (_, _) => ShipRequested?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler? ReceiveRequested;
    public event EventHandler? ShipRequested;

    public string? SelectedProductCode => (_grid.CurrentRow?.DataBoundItem as ProductRow)?.Code;

    public string QuantityText => _quantityTextBox.Text;

    public void ShowProducts(IReadOnlyList<ProductRow> rows)
    {
        // 一覧を更新しても選択中の行が変わらないようにする
        var selectedCode = SelectedProductCode;
        _grid.DataSource = rows.ToList();

        foreach (DataGridViewRow row in _grid.Rows)
        {
            if ((row.DataBoundItem as ProductRow)?.Code == selectedCode)
            {
                _grid.CurrentCell = row.Cells[0];
                break;
            }
        }
    }

    public void ShowMessage(string message)
    {
        _messageLabel.ForeColor = Color.DarkGreen;
        _messageLabel.Text = message;
    }

    public void ShowError(string message)
    {
        _messageLabel.Text = "";
        MessageBox.Show(this, message, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public void ClearQuantity() => _quantityTextBox.Clear();
}
