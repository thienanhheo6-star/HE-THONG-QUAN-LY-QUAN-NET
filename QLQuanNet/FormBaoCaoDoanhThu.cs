using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLQuanNet
{
    public partial class FormBaoCaoDoanhThu : Form
    {
        private ReportService _service = new ReportService();

        public FormBaoCaoDoanhThu()
        {
            InitializeComponent();
            dateTimePickerFrom.Value = DateTime.Today.AddDays(-7);
            dateTimePickerTo.Value = DateTime.Today;
        }

        private void ButtonLoad_Click(object sender, EventArgs e)
        {
            var from = dateTimePickerFrom.Value.Date;
            var to = dateTimePickerTo.Value.Date.AddDays(1).AddTicks(-1);
            var data = _service.GetRevenueByDay(from, to);
            dataGridView1.DataSource = data.Select(d => new { Date = d.Date.ToString("yyyy-MM-dd"), d.Amount }).ToList();

            var series = chart1.Series[0];
            series.Points.Clear();
            foreach (var r in data)
            {
                series.Points.AddXY(r.Date.ToString("yyyy-MM-dd"), r.Amount);
            }
        }

        private void ButtonExportCsv_Click(object sender, EventArgs e)
        {
            if (dataGridView1.DataSource == null) return;
            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files|*.csv";
                sfd.FileName = "BaoCaoDoanhThu.csv";
                if (sfd.ShowDialog() != DialogResult.OK) return;

                using (var w = new System.IO.StreamWriter(sfd.FileName))
                {
                    w.WriteLine("Date,Amount");
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        if (row.IsNewRow) continue;
                        object dateCell = null;
                        object amountCell = null;
                        // Try by column name first
                        if (dataGridView1.Columns.Contains("Date")) dateCell = row.Cells["Date"].Value;
                        if (dataGridView1.Columns.Contains("Amount")) amountCell = row.Cells["Amount"].Value;
                        // Fallback to index-based access
                        if (dateCell == null && row.Cells.Count > 0) dateCell = row.Cells[0].Value;
                        if (amountCell == null && row.Cells.Count > 1) amountCell = row.Cells[1].Value;

                        var dateText = dateCell?.ToString() ?? "";
                        var amountText = amountCell?.ToString() ?? "";
                        w.WriteLine($"{dateText},{amountText}");
                    }
                }

                MessageBox.Show("Export thành công.");
            }
        }
    }
}
