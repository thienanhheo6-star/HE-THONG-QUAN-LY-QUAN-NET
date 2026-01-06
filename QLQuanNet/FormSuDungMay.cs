using DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;

namespace QLQuanNet
{
    public partial class FormSuDungMay : Form
    {
        private Model1 db;
        private Timer realtimeTimer;
        private BindingSource suDungMaySource;
        private SuDungMay _hoaDonPhien;
        private MAYTINH _hoaDonMay;
        private TaiKhoanNguoiChoi _hoaDonTK;
        private decimal _hoaDonTongTien;
        private decimal _hoaDonTienDichVu;
        private TimeSpan _hoaDonThoiGian;


        public FormSuDungMay()
        {
            InitializeComponent();

            db = new Model1();

            suDungMaySource = new BindingSource();
            dgvSuDungMay.AutoGenerateColumns = true;
            dgvSuDungMay.DataSource = suDungMaySource;

            realtimeTimer = new Timer { Interval = 1000 };
            realtimeTimer.Tick += (s, e) => UpdateRealTimeDisplay();

            this.FormClosed += (s, e) =>
            {
                realtimeTimer.Stop();
                db?.Dispose();
            };
            ApplyStyles();
        }


        private void FormSuDungMay_Load(object sender, EventArgs e)
        {
            RefreshData();
            UpdateRealTimeDisplay(); // gọi 1 lần đầu
            realtimeTimer.Start();
        }


        private void RefreshData()
        {
            db.Dispose();
            db = new Model1();

            cboMay.DataSource = db.MAYTINHs
                .Where(x => x.TrangThai == "Trống")
                .ToList();
            cboMay.DisplayMember = "TenMay";
            cboMay.ValueMember = "MaMay";

            cboTaiKhoan.DataSource = db.TaiKhoanNguoiChois.ToList();
            cboTaiKhoan.DisplayMember = "TenDangNhap";
            cboTaiKhoan.ValueMember = "MaTK";

            UpdateRealTimeDisplay();
        }


        private void dgvSuDungMay_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                var row = dgvSuDungMay.Rows[e.RowIndex];
                int maMay = (int)row.Cells["MaMay"].Value;
                int maTK = (int)row.Cells["MaTK"].Value;

                // Để chọn được máy đang sử dụng (vốn không có trong DataSource của cboMay hiện tại)
                // Chúng ta cần tạm thời load lại full máy hoặc gán trực tiếp
                var fullMayList = db.MAYTINHs.ToList();
                cboMay.DataSource = fullMayList;

                cboMay.SelectedValue = maMay;
                cboTaiKhoan.SelectedValue = maTK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn máy: " + ex.Message);
            }
        }

        private void UpdateRealTimeDisplay()
        {
            try
            {
                var list = db.SuDungMays
                    .Where(x => x.ThoiGianKetThuc == null)
                    .ToList();

                var data = list.Select(x =>
                {
                    var may = db.MAYTINHs.Find(x.MaMay);
                    var tk = db.TaiKhoanNguoiChois.Find(x.MaTK);
                    TimeSpan duration = DateTime.Now - x.ThoiGianBatDau;

                    return new SuDungMayView
                    {
                        Ma_Phien = x.MaSD,
                        May = may?.TenMay,
                        User = tk?.TenDangNhap,
                        Bat_Dau = x.ThoiGianBatDau.ToString("HH:mm:ss"),
                        Thoi_Gian = $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}",
                        Tam_Tinh = $"{((decimal)duration.TotalHours * (may?.GiaTheoGio ?? 0)):N0} VNĐ"
                    };
                }).ToList();

                suDungMaySource.DataSource = data;
            }
            catch { }
        }


        private void BtnBatDau_Click(object sender, EventArgs e)
        {
            if (cboMay.SelectedValue == null || cboTaiKhoan.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn máy và tài khoản!");
                return;
            }

            int maMay = (int)cboMay.SelectedValue;
            int maTK = (int)cboTaiKhoan.SelectedValue;

            var may = db.MAYTINHs.Find(maMay);
            if (may.TrangThai != "Trống")
            {
                MessageBox.Show("Máy này đang được sử dụng!");
                return;
            }

            db.SuDungMays.Add(new SuDungMay
            {
                MaMay = maMay,
                MaTK = maTK,
                ThoiGianBatDau = DateTime.Now,
                TongTien = 0
            });

            may.TrangThai = "Đang sử dụng";
            db.SaveChanges();
            try { AppEvents.RaiseDataChanged(DataChangeType.AddPlayTime); } catch { }

            MessageBox.Show($"Đã bắt đầu phiên cho máy {may.TenMay}");
            RefreshData();
        }

        private void BtnKetThuc_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra chọn dòng
            if (dgvSuDungMay.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một phiên đang sử dụng trong danh sách!");
                return;
            }

            int maSD = (int)dgvSuDungMay.CurrentRow.Cells["Ma_Phien"].Value;

            // Tìm phiên chơi
            var phien = db.SuDungMays.Find(maSD);
            if (phien == null) return;

            var tk = db.TaiKhoanNguoiChois.Find(phien.MaTK);
            var may = db.MAYTINHs.Find(phien.MaMay);

            // 2. Tính toán tiền giờ
            double totalHours = (DateTime.Now - phien.ThoiGianBatDau).TotalHours;
            if (totalHours < 0.01) totalHours = 0.01;

            decimal bill = (decimal)totalHours * (may?.GiaTheoGio ?? 0);

            // (Tùy chọn) Cộng thêm tiền dịch vụ nếu có
            decimal tienDichVu = db.SuDungDichVus.Where(s => s.MaSD == maSD).Sum(s => (decimal?)s.ThanhTien) ?? 0m;
            bill += tienDichVu;

            // Trừ tiền tài khoản
            if (tk != null)
            {
                if (tk.SoDu < bill)
                {
                    // Có thể cảnh báo hoặc cho nợ tùy bạn
                    // MessageBox.Show($"Tài khoản không đủ tiền!...");
                }
                tk.SoDu -= bill;
            }

            // 3. Cập nhật Database
            phien.ThoiGianKetThuc = DateTime.Now;
            phien.TongTien = bill;
            if (may != null) may.TrangThai = "Trống";

            db.SaveChanges();
            try { AppEvents.RaiseDataChanged(DataChangeType.AddPlayTime); } catch { }

            // 4. --- [QUAN TRỌNG] GỬI LỆNH ĐÁ CLIENT RA NGOÀI ---
            try
            {
                // Tìm FormMain (nơi chứa Socket Server)
                FormMain mainForm = Application.OpenForms["FormMain"] as FormMain;

                if (mainForm != null && may != null)
                {
                    // Gọi hàm đá máy mà ta đã viết ở FormMain
                    // Lưu ý: may.MaMay phải chuyển sang string
                    mainForm.ForceLogoutClient(may.MaMay.ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi gửi lệnh đến máy trạm: " + ex.Message);
            }
            // ----------------------------------------------------

            MessageBox.Show($"Kết thúc thành công!\nMáy: {may?.TenMay}\nTổng tiền: {bill:N0} VNĐ");

            // 5. Load lại bảng
            // RefreshData(); // Bỏ comment nếu hàm này của bạn tên là RefreshData
            // Hoặc gọi lại hàm Load
            // LoadDanhSachPhien();
        }

        // ... Các hàm Style giữ nguyên như cũ ...



        private void BtnInHoaDon_Click(object sender, EventArgs e)
        {
            if (PrinterSettings.InstalledPrinters.Count == 0)
            {
                MessageBox.Show(
                    "Máy chưa cài máy in.\n\n" +
                    "👉 Hãy cài 'Microsoft Print to PDF' để xem trước hóa đơn.",
                    "Không thể in hóa đơn",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            try
            {
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += PrintHoaDon_PrintPage;

                using (PrintPreviewDialog preview = new PrintPreviewDialog())
                {
                    preview.Document = pd;
                    preview.Width = 900;
                    preview.Height = 600;
                    preview.ShowDialog();
                }
            }
            catch (InvalidPrinterException)
            {
                MessageBox.Show(
                    "Lỗi máy in.\nVui lòng kiểm tra lại printer.",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }
        private void PrintHoaDon_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            int y = 20;

            Font titleFont = new Font("Segoe UI", 14, FontStyle.Bold);
            Font normalFont = new Font("Segoe UI", 10);
            Font boldFont = new Font("Segoe UI", 10, FontStyle.Bold);

            g.DrawString("HÓA ĐƠN THANH TOÁN", titleFont, Brushes.Black, 200, y);
            y += 40;

            g.DrawString($"Tài khoản : {_hoaDonTK?.TenDangNhap}", normalFont, Brushes.Black, 20, y); y += 20;
            g.DrawString($"Máy       : {_hoaDonMay?.TenMay}", normalFont, Brushes.Black, 20, y); y += 20;
            g.DrawString($"Bắt đầu   : {_hoaDonPhien.ThoiGianBatDau:HH:mm:ss}", normalFont, Brushes.Black, 20, y); y += 20;
            g.DrawString($"Kết thúc  : {(_hoaDonPhien.ThoiGianKetThuc ?? DateTime.Now):HH:mm:ss}", normalFont, Brushes.Black, 20, y); y += 20;
            g.DrawString($"Thời gian : {_hoaDonThoiGian:hh\\:mm\\:ss}", normalFont, Brushes.Black, 20, y); y += 30;

            g.DrawLine(Pens.Black, 20, y, 380, y);
            y += 10;

            g.DrawString("CHI TIẾT", boldFont, Brushes.Black, 20, y); y += 20;

            g.DrawString($"Tiền giờ : {_hoaDonTongTien - _hoaDonTienDichVu:N0} VNĐ", normalFont, Brushes.Black, 20, y); y += 20;
            g.DrawString($"Dịch vụ  : {_hoaDonTienDichVu:N0} VNĐ", normalFont, Brushes.Black, 20, y); y += 20;

            g.DrawLine(Pens.Black, 20, y, 380, y);
            y += 10;

            g.DrawString($"TỔNG TIỀN : {_hoaDonTongTien:N0} VNĐ", boldFont, Brushes.Black, 20, y);
        }


        private void BtnXuatCsv_Click(object sender, EventArgs e)
        {
            if (dgvSuDungMay.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu để xuất!");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "CSV Files (*.csv)|*.csv",
                FileName = "SuDungMay_" + DateTime.Now.ToString("yyyyMMdd_HHmm") + ".csv"
            };

            if (sfd.ShowDialog() != DialogResult.OK) return;

            try
            {
                using (var sw = new System.IO.StreamWriter(sfd.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Header
                    var headers = dgvSuDungMay.Columns
                        .Cast<DataGridViewColumn>()
                        .Where(c => c.Visible)
                        .Select(c => c.HeaderText);
                    sw.WriteLine(string.Join(",", headers));

                    // Data
                    foreach (DataGridViewRow row in dgvSuDungMay.Rows)
                    {
                        var cells = row.Cells
                            .Cast<DataGridViewCell>()
                            .Where(c => c.Visible)
                            .Select(c => c.Value?.ToString()?.Replace(",", " "));
                        sw.WriteLine(string.Join(",", cells));
                    }
                }

                MessageBox.Show("Xuất CSV thành công!", "Thông báo");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi xuất CSV: " + ex.Message);
            }
        }

        private void ApplyStyles()
        {
            StyleComboBox(cboMay, new Point(20, 25), 180);
            StyleComboBox(cboTaiKhoan, new Point(220, 25), 180);
            StyleButton(btnKetThuc, "⏹ KẾT THÚC", Color.FromArgb(220, 53, 69), new Point(580, 20));
            StyleButton(btnInHoaDon, "📄 IN HÓA ĐƠN", Color.FromArgb(0, 120, 215), new Point(20, 75));
            StyleButton(btnXuatCsv, "📊 XUẤT CSV", Color.FromArgb(108, 117, 125), new Point(180, 75));
        }

        private void StyleComboBox(ComboBox cbo, Point loc, int width)
        {
            cbo.Location = loc;
            cbo.Width = width;
            cbo.BackColor = Color.FromArgb(45, 45, 55);
            cbo.ForeColor = Color.White;
            cbo.FlatStyle = FlatStyle.Flat;
            cbo.Font = new Font("Segoe UI", 11F);
        }

        private void StyleButton(Button btn, string text, Color color, Point loc)
        {
            btn.Text = text;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Location = loc;
            btn.Size = new Size(150, 45);
            btn.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        }

        private void dgvSuDungMay_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}