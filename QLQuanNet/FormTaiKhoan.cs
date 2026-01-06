using DAL.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace QLQuanNet
{
    public partial class FormTaiKhoan : Form
    {
        private Model1  db;

        public FormTaiKhoan()
        {
            InitializeComponent();
            db = new Model1();
            LoadData();
        }

        private void pnlLeft_Paint(object sender, PaintEventArgs e)
        {

        }

        private void maskedTextBox3_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void pnlHeader_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormTaiKhoan_Load_1(object sender, EventArgs e)
        {
            LoadData();
            txtMaTK.BackColor = Color.LightGray;
            // --- CẤU HÌNH AN TOÀN ---
            // 1. Khóa ô Số dư để không ai sửa bậy
            txtSoDu.ReadOnly = true;
            txtSoDu.Text = "0";
            txtSoDu.BackColor = Color.LightGray;

            // 2. Khóa ô ID (Auto)
            txtMaTK.ReadOnly = true;
            txtMaTK.BackColor = Color.LightGray;
        }
        private void LoadData()
        {
            // Lấy dữ liệu từ DB đổ vào bảng (giả sử tên bảng là dgvTaiKhoan)
            dgvTaiKhoan.DataSource = db.TaiKhoanNguoiChois.Select(t => new
            {
                t.MaTK,
                t.TenDangNhap,
                // Không hiện mật khẩu ra bảng để bảo mật
                t.SoDu
            }).ToList();
        }

        private void dgvTaiKhoan_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lấy ID của dòng đang chọn
            int id = int.Parse(dgvTaiKhoan.Rows[e.RowIndex].Cells[0].Value.ToString());

            // Tìm trong DB
            var tk = db.TaiKhoanNguoiChois.Find(id);
            if (tk != null)
            {
                txtMaTK.Text = tk.MaTK.ToString();
                txtUser.Text = tk.TenDangNhap;

                // Hiển thị số dư (Chỉ xem, không sửa được do đã set ReadOnly)
                txtSoDu.Text = tk.SoDu.ToString("#,##0");

                // Mật khẩu thì để trống (để người dùng nhập mới nếu muốn đổi)
                txtPassword.Text = "";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kiểm tra nhập liệu
            if (string.IsNullOrEmpty(txtUser.Text) || string.IsNullOrEmpty(txtPassword.Text))
            {
                MessageBox.Show("Vui lòng nhập Tên đăng nhập và Mật khẩu!");
                return;
            }

            // Kiểm tra trùng tên đăng nhập
            if (db.TaiKhoanNguoiChois.Any(x => x.TenDangNhap == txtUser.Text))
            {
                MessageBox.Show("Tên đăng nhập này đã tồn tại! Vui lòng chọn tên khác.");
                return;
            }

            try
            {
                TaiKhoanNguoiChoi tk = new TaiKhoanNguoiChoi();
                tk.TenDangNhap = txtUser.Text;
                tk.MatKhau = txtPassword.Text; // (Nhớ mã hóa nếu cần)

                // --- XỬ LÝ SỐ DƯ BAN ĐẦU ---
                decimal tienNap = 0;
                // Thử lấy số tiền từ ô Thêm giờ chơi
                decimal.TryParse(txtThemGio.Text, out tienNap);

                tk.SoDu = tienNap; // Gán làm số dư ban đầu
                                   // ---------------------------

                db.TaiKhoanNguoiChois.Add(tk);
                db.SaveChanges();

                // Ghi lịch sử nạp tiền nếu có
                if (tienNap > 0)
                {
                    NapTien nap = new NapTien { MaTK = tk.MaTK, SoTien = tienNap, ThoiGian = DateTime.Now };
                    db.NapTiens.Add(nap);
                    db.SaveChanges();
                    try { AppEvents.RaiseDataChanged(DataChangeType.TopUp); } catch { }
                }

                MessageBox.Show("Thêm thành công! Số dư ban đầu: " + tienNap.ToString("#,##0"));
                LoadData();
                LamMoi();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTK.Text)) return;

            int id = int.Parse(txtMaTK.Text);
            var tk = db.TaiKhoanNguoiChois.Find(id);

            if (tk != null)
            {
                // 1. Cập nhật thông tin cơ bản
                tk.TenDangNhap = txtUser.Text;
                if (!string.IsNullOrEmpty(txtPassword.Text))
                {
                    tk.MatKhau = txtPassword.Text;
                }

                // 2. --- XỬ LÝ NẠP TIỀN (CỘNG DỒN) ---
                decimal tienNap = 0;
                // Lấy giá trị từ ô "Thêm giờ chơi"
                if (decimal.TryParse(txtThemGio.Text, out tienNap))
                {
                    // Nếu có nhập tiền (> 0) thì mới cộng
                    if (tienNap > 0)
                    {
                        // Logic: Số dư mới = Số dư cũ (nếu null thì là 0) + Tiền vừa nạp
                        tk.SoDu = (tk.SoDu) + tienNap;

                        // Ghi lịch sử nạp tiền
                        NapTien nap = new NapTien { MaTK = tk.MaTK, SoTien = tienNap, ThoiGian = DateTime.Now };
                        db.NapTiens.Add(nap);
                    }
                }

                // Lưu các thay đổi (cập nhật tài khoản và lưu bản ghi nạp tiền nếu có)
                db.SaveChanges();
                try { AppEvents.RaiseDataChanged(DataChangeType.TopUp); } catch { }
                MessageBox.Show("Cập nhật thành công! Đã cộng thêm tiền.");

                LoadData();
                LamMoi(); // Xóa trắng các ô để tránh bấm nhầm cộng tiếp
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaTK.Text))
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn có chắc muốn xóa tài khoản này?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int id = int.Parse(txtMaTK.Text);
                var tk = db.TaiKhoanNguoiChois.Find(id);

                if (tk != null)
                {
                    // Prevent deleting account if there are related NapTien records
                    bool hasTopUps = db.NapTiens.Any(n => n.MaTK == tk.MaTK);
                    if (hasTopUps)
                    {
                        MessageBox.Show("Không thể xóa tài khoản vì có lịch sử nạp tiền. Xóa các bản ghi phụ thuộc trước.");
                        return;
                    }

                    db.TaiKhoanNguoiChois.Remove(tk);
                    try
                    {
                        db.SaveChanges();
                        try { AppEvents.RaiseDataChanged(DataChangeType.Other); } catch { }
                        MessageBox.Show("Đã xóa tài khoản!");
                        LoadData();
                        LamMoi();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa tài khoản: " + ex.Message);
                    }
                }
            }
        }
        private void LamMoi()
        {
            txtMaTK.Text = "";
            txtUser.Text = "";
            txtPassword.Text = "";
            txtSoDu.Text = "0";

            // Reset ô nạp tiền
            txtThemGio.Text = "";
        }
    }
}
