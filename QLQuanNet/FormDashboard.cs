using System;
using System.Windows.Forms;
using System.Linq;

namespace QLQuanNet
{
    public partial class FormDashboard : Form
    {
        private DashboardService dashboardService = new DashboardService();
        private Timer refreshTimer;

        // subscription delegate so we can unsubscribe on close
        private Action<DataChangeType> onDataChangedHandler;

        public FormDashboard()
        {
            InitializeComponent();
            refreshTimer = new Timer
            {
                Interval = 5000 // Refresh mỗi 5 giây
            };
            refreshTimer.Tick += RefreshTimer_Tick;

            // Subscribe to global data-change events to refresh immediately
            onDataChangedHandler = (type) => {
                try
                {
                    // Only refresh full dashboard when relevant change types occur
                    if (type == DataChangeType.TopUp || type == DataChangeType.AddPlayTime || type == DataChangeType.ServiceUsed)
                    {
                        if (this.IsHandleCreated)
                        {
                            this.BeginInvoke(new Action(() => RefreshDashboard()));
                        }
                    }
                    else
                    {
                        // For other types we still update summary stats periodically via timer
                    }
                }
                catch { }
            };
            AppEvents.DataChanged += onDataChangedHandler;
        }

        private void FormDashboard_Load(object sender, EventArgs e)
        {
            RefreshDashboard();
            refreshTimer.Start();
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Only refresh summary labels on timer tick; grid updates are driven by AppEvents
            RefreshStatsOnly();
        }

        private void RefreshStatsOnly()
        {
            var stats = dashboardService.GetTodayStats();
            labelTodayRevenue.Text = $"{stats.TodayRevenue:N0} VND";
            labelActiveMachines.Text = $"{stats.ActiveMachines} máy";
            labelTotalBalance.Text = $"{stats.TotalBalance:N0} VND";
        }

        private void RefreshDashboard()
        {
            RefreshStatsOnly();

            dataGridView1.DataSource = dashboardService.GetRecentTransactions(10);
            // Tùy chỉnh hiển thị cột
            if (dataGridView1.Columns.Contains("Thoigian"))
            {
                dataGridView1.Columns["Thoigian"].HeaderText = "Thời gian";
            }
            if (dataGridView1.Columns.Contains("TienNap"))
            {
                dataGridView1.Columns["TienNap"].HeaderText = "Tiền nạp";
                dataGridView1.Columns["TienNap"].DefaultCellStyle.Format = "N0";
            }
            if (dataGridView1.Columns.Contains("Services"))
            {
                dataGridView1.Columns["Services"].HeaderText = "Tổng dịch vụ";
                dataGridView1.Columns["Services"].DefaultCellStyle.Format = "N0";
            }
            if (dataGridView1.Columns.Contains("Account"))
            {
                dataGridView1.Columns["Account"].HeaderText = "Tài khoản";
            }
            // Ẩn các cột không cần thiết
            foreach (var col in new[] { "EventType", "Id", "MaTK" })
            {
                if (dataGridView1.Columns.Contains(col)) dataGridView1.Columns[col].Visible = false;
            }
        }

        private void FormDashboard_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshTimer?.Stop();
            refreshTimer?.Dispose();
            if (onDataChangedHandler != null) AppEvents.DataChanged -= onDataChangedHandler;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
