using DAL.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace QLQuanNet
{
    public partial class FormDichVu : Form
    {
        private readonly Model1 db;

        // Nếu form được dùng trong ngữ cảnh một phiên sử dụng, set MaSD này từ caller
        public int CurrentMaSD { get; set; } = 0;

        public FormDichVu()
        {
            InitializeComponent();
            db = new Model1();
        }

        private void FormDichVu_Load_1(object sender, EventArgs e)
        {
            LoadDanhSachMonAn();
            LoadMenuHinhAnh();
        }
        private string LayDuongDanThuMucAnh()
        {
            // 1. Lấy đường dẫn file .exe đang chạy (bin\Debug)
            string duongDanBin = Application.StartupPath;

            // 2. Đi lùi ra 2 cấp để về thư mục gốc Project (QLQuanNet)
            string thuMucProject = Directory.GetParent(duongDanBin).Parent.FullName;

            // 3. --- SỬA Ở ĐÂY: Thêm thư mục "DAL" vào đường dẫn ---
            // Đường dẫn đúng theo hình bạn gửi: QLQuanNet -> DAL -> Models -> Images
            string duongDanAnh = Path.Combine(thuMucProject, "DAL", "Models", "Images");

            // Kiểm tra xem có đúng không, nếu không thấy thì thử tìm ở cấp Solution (phòng hờ)
            if (!Directory.Exists(duongDanAnh))
            {
                // Lùi ra thêm 1 cấp nữa (ra thư mục Solution) và tìm vào DAL
                string thuMucSolution = Directory.GetParent(thuMucProject).FullName;
                string duongDanAnh2 = Path.Combine(thuMucSolution, "DAL", "Models", "Images");

                if (Directory.Exists(duongDanAnh2))
                {
                    return duongDanAnh2;
                }
            }

            return duongDanAnh;
        }
        private void HienThiLenGiaoDien(List<DICHVU> listData)
        {
            // Xóa danh sách cũ đi
            flowLayoutPanel1.Controls.Clear();

            foreach (var item in listData)
            {
                 UC_MonAn uc = new UC_MonAn();

                // Gán dữ liệu (Code cũ của bạn)
                uc.Id = item.MaDV;
                uc.TenMon = item.TenDV;
                uc.Gia = (double)(item.DonGia);

                // Xử lý ảnh
                string tenAnh = item.HinhAnh;
                if (!string.IsNullOrEmpty(tenAnh))
                {
                    string path = Path.Combine(Application.StartupPath, "Images", tenAnh);
                    uc.SetImage(path);
                }

                // Gán sự kiện click
                uc.OnSelect += Uc_OnSelect;

                // Thêm vào giao diện
                flowLayoutPanel1.Controls.Add(uc);
            }
        }
        private void LoadDanhSachMonAn()
        {
            // Lấy tất cả dữ liệu từ DB
            var listAll = db.DICHVUs.ToList();

            // Gọi hàm hiển thị
            HienThiLenGiaoDien(listAll);
        }
        private void Uc_OnSelect(object sender, EventArgs e)
        {
            UC_MonAn itemChon = (UC_MonAn)sender;
            bool daTonTai = false;

            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                // Kiểm tra ID món ăn (lấy từ Tag)
                if (row.Tag != null && (int)row.Tag == itemChon.Id)
                {
                    // === CẬP NHẬT DÒNG ĐÃ CÓ ===
                    // Cột số 1 là Số Lượng (Lúc trước là 2)
                    int soLuongCu = int.Parse(row.Cells[1].Value.ToString());
                    int soLuongMoi = soLuongCu + 1;

                    row.Cells[1].Value = soLuongMoi;

                    // Cột số 3 là Đơn giá (Lúc trước là 4)
                    double donGia = double.Parse(row.Cells[3].Value.ToString().Replace(",", ""));

                    // Cột số 5 là Thành tiền (Lúc trước là 6)
                    row.Cells[5].Value = (soLuongMoi * donGia).ToString("#,##0");

                    daTonTai = true;
                    break;
                }
            }

            // === THÊM DÒNG MỚI NẾU CHƯA CÓ ===
            if (!daTonTai)
            {
                int index = dgvHoaDon.Rows.Add();
                DataGridViewRow row = dgvHoaDon.Rows[index];

                // Lưu ID vào Tag để dùng sau này
                row.Tag = itemChon.Id;

                // Điền dữ liệu vào các cột mới
                row.Cells[0].Value = itemChon.TenMon;       // Cột 0: Tên hàng
                                                           
                row.Cells[1].Value = 1;                     // Cột 1: SL
                row.Cells[2].Value = "-";                   // Cột 2: Nút Trừ
                row.Cells[3].Value = itemChon.Gia.ToString("#,##0"); // Cột 3: Đơn giá
                row.Cells[4].Value = "+";                   // Cột 4: Nút Cộng
                row.Cells[5].Value = itemChon.Gia.ToString("#,##0"); // Cột 5: Thành tiền
                row.Cells[6].Value = "X";                   // Cột 6: Nút Xóa
            }

            TinhTongTien();
        }
        private void TinhTongTien()
        {
            double tong = 0;
            // Duyệt qua từng dòng trong bảng để cộng dồn tiền
            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                // Kiểm tra cột Thành tiền (Cột số 5)
                if (row.Cells[5].Value != null)
                {
                    // Xóa dấu phẩy đi để tính toán (Ví dụ: "10,000" -> 10000)
                    string tienString = row.Cells[5].Value.ToString().Replace(",", "").Replace(".", "");
                    double tien = 0;
                    if (double.TryParse(tienString, out tien))
                    {
                        tong += tien;
                    }
                }
            }

            // --- CẬP NHẬT VÀO TEXTBOX ---
            // Hiển thị số tiền ra TextBox, định dạng có dấu phẩy ngăn cách
            txtTongTien.Text = tong.ToString("#,##0");
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Bỏ qua nếu click vào tiêu đề
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvHoaDon.Rows[e.RowIndex];

            // --- NÚT CỘNG (+) NẰM Ở CỘT SỐ 4 ---
            if (e.ColumnIndex == 4)
            {
                // Lấy SL ở cột 1
                int sl = int.Parse(row.Cells[1].Value.ToString());
                sl++; // Tăng lên
                row.Cells[1].Value = sl;

                // Tính lại tiền (Lấy giá ở cột 3, gán vào cột 5)
                double donGia = double.Parse(row.Cells[3].Value.ToString().Replace(",", ""));
                row.Cells[5].Value = (sl * donGia).ToString("#,##0");

                TinhTongTien();
            }
            // --- NÚT TRỪ (-) NẰM Ở CỘT SỐ 2 ---
            else if (e.ColumnIndex == 2)
            {
                int sl = int.Parse(row.Cells[1].Value.ToString());
                if (sl > 1) // Chỉ giảm khi > 1
                {
                    sl--;
                    row.Cells[1].Value = sl;

                    double donGia = double.Parse(row.Cells[3].Value.ToString().Replace(",", ""));
                    row.Cells[5].Value = (sl * donGia).ToString("#,##0");

                    TinhTongTien();
                }
            }
            // --- NÚT XÓA (X) NẰM Ở CỘT SỐ 6 ---
            else if (e.ColumnIndex == 6)
            {
                if (MessageBox.Show("Xóa món này?", "Xác nhận", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dgvHoaDon.Rows.RemoveAt(e.RowIndex);
                    TinhTongTien();
                }
            }
        }
        

        private void Button1_Click(object sender, EventArgs e)
        {
            decimal tongTienHienTai = 0;
            decimal.TryParse(txtTongTien.Text.Replace(",", "").Replace(".", ""), out tongTienHienTai);

            if (tongTienHienTai <= 0)
            {
                MessageBox.Show("Chưa chọn món nào!");
                return;
            }

            if (MessageBox.Show("Xác nhận thanh toán?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using (var db = new DAL.Models.Model1())
                {
                    try
                    {
                        // 2. DUYỆT QUA DANH SÁCH MÓN ĐỂ LƯU
                        foreach (DataGridViewRow row in dgvHoaDon.Rows) // dgvOrder là bảng danh sách món đã chọn bên phải
                        {
                            if (row.Cells[0].Value != null)
                            {
                                DAL.Models.SuDungDichVu dv = new DAL.Models.SuDungDichVu();

                                // Lưu các thông tin cơ bản
                                // 1. Lấy giá trị từ lưới (Grid) ra biến tạm (không gán vào dv vội)
                                int sl = int.Parse(row.Cells[1].Value.ToString());
                                // Giá hiển thị có định dạng #,##0 -> xóa dấu phân cách trước khi parse
                                decimal gia = decimal.Parse(row.Cells[3].Value.ToString().Replace(",", "").Replace(".", ""));

                                // 2. Gán vào đối tượng Database
                                // Gán MaDV từ Tag của dòng (nếu đã lưu khi tạo hàng)
                                dv.MaDV = row.Tag != null ? (int)row.Tag : 0;
                                // Gán MaSD từ phiên hiện tại (nếu form được mở trong ngữ cảnh một phiên)
                                dv.MaSD = this.CurrentMaSD;

                                dv.SoLuong = sl;

                                // 3. Tính thành tiền và lưu vào Database
                                dv.ThanhTien = sl * gia;

                                db.SuDungDichVus.Add(dv); // Chỉ thêm vào bảng Dịch Vụ
                            }
                        }

                        db.SaveChanges(); // Lưu xuống Database

                        MessageBox.Show("Đã thanh toán !");

                        // Nếu đang chạy trong client và có FormCLI mở, gửi lệnh thông báo tới server
                        try
                        {
                            var cliForm = Application.OpenForms["FormCLI"] as GUICLI.FormCLI;
                            if (cliForm != null && this.CurrentMaSD > 0)
                            {
                                cliForm.SendCommandToServer($"SERVICE_USED|{this.CurrentMaSD}");
                            }
                        }
                        catch { }

                        // Reset giao diện
                        dgvHoaDon.Rows.Clear();
                        txtTongTien.Text = "0";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi: " + ex.Message);
                    }
                }
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            // 1. Lấy từ khóa người dùng nhập (xóa khoảng trắng thừa, chuyển thường)
            string tuKhoa = txtTimKiem.Text.Trim().ToLower();

            // 2. Nếu ô tìm kiếm trống -> Load lại tất cả
            if (string.IsNullOrEmpty(tuKhoa))
            {
                LoadDanhSachMonAn();
                return;
            }

            // 3. Tìm kiếm trong Database (Lọc theo Tên dịch vụ chứa từ khóa)
            var listKetQua = db.DICHVUs
                               .Where(p => p.TenDV.ToLower().Contains(tuKhoa))
                               .ToList();

            // 4. Kiểm tra kết quả
            if (listKetQua.Count > 0)
            {
                // Nếu có kết quả -> Hiển thị ra
                HienThiLenGiaoDien(listKetQua);
            }
            else
            {
                // Nếu không tìm thấy -> Thông báo và xóa trắng danh sách
                MessageBox.Show("Không tìm thấy món nào có tên: " + txtTimKiem.Text);
                flowLayoutPanel1.Controls.Clear();
            }
        }

        private void txtTimKiem_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra xem phím vừa nhấn có phải là Enter không
            if (e.KeyCode == Keys.Enter)
            {
                // 1. Thực hiện bấm nút Tìm kiếm (tương đương click chuột)
                button2.PerformClick();
                // 2. Ngăn tiếng "ding" (âm thanh hệ thống) kêu lên khó chịu
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        private void LoadMenuHinhAnh()
        {
            try
            {
                // 1. Xóa sạch danh sách cũ
                // (Lưu ý: Bạn kiểm tra lại tên panel chứa ảnh của bạn là flowLayoutPanel1 hay flpTop4 nhé)
                flowLayoutPanel1.Controls.Clear();

                // 2. Lấy TẤT CẢ món ăn (XÓA BỎ lệnh .Take(4) đi)
                var listAll = db.DICHVUs.ToList();

                foreach (var item in listAll)
                {
                    // Tạo UserControl mới
                    UC_MonAn uc = new UC_MonAn();

                    // Đổ dữ liệu
                    uc.Id = item.MaDV;
                    uc.TenMon = item.TenDV;
                    uc.Gia = (double)(item.DonGia);

                    // Xử lý ảnh (Dùng hàm lấy đường dẫn chuẩn Models\Images)
                    string tenAnh = item.HinhAnh; // Hoặc item.Images tùy tên cột DB của bạn
                    if (!string.IsNullOrEmpty(tenAnh))
                    {
                        string thuMucAnh = LayDuongDanThuMucAnh();
                        string path = Path.Combine(thuMucAnh, tenAnh.Trim());
                        uc.SetImage(path);
                    }

                    // Gán sự kiện click
                    uc.OnSelect += Uc_OnSelect;

                    // Thêm vào giao diện
                    flowLayoutPanel1.Controls.Add(uc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load ảnh: " + ex.Message);
            }
        }
        private void Pic_Click(object sender, EventArgs e)
{
    // 1. Lấy tấm ảnh vừa được nhấn
    PictureBox pic = (PictureBox)sender;

    // 2. Lôi thông tin món ăn đã giấu trong Tag ra
    // (Phải ép kiểu về DICHVU vì lúc nãy mình gán Tag = item)
    var item = (DICHVU)pic.Tag;

    if (item == null) return;

    // 3. Code thêm vào lưới (Copy logic từ Uc_OnSelect và sửa lại chút xíu)
    bool daTonTai = false;

    foreach (DataGridViewRow row in dgvHoaDon.Rows)
    {
        // Kiểm tra ID
        if (row.Tag != null && (int)row.Tag == item.MaDV)
        {
            // Đã có -> Tăng số lượng
            int soLuongCu = int.Parse(row.Cells[1].Value.ToString());
            int soLuongMoi = soLuongCu + 1;
            row.Cells[1].Value = soLuongMoi;

            // Cập nhật thành tiền
            double donGia = double.Parse(row.Cells[3].Value.ToString().Replace(",", ""));
            row.Cells[5].Value = (soLuongMoi * donGia).ToString("#,##0");

            daTonTai = true;
            break;
        }
    }

    // Chưa có -> Thêm mới
    if (!daTonTai)
    {
        int index = dgvHoaDon.Rows.Add();
        DataGridViewRow row = dgvHoaDon.Rows[index];

        row.Tag = item.MaDV; // Lưu ID

        // Điền dữ liệu
        row.Cells[0].Value = item.TenDV;
        row.Cells[1].Value = 1;
        row.Cells[2].Value = "-";

                double gia = (double)(item.DonGia);
        row.Cells[3].Value = gia.ToString("#,##0");
        
        row.Cells[4].Value = "+";
        row.Cells[5].Value = gia.ToString("#,##0");
        row.Cells[6].Value = "X";
    }

    // 4. Tính lại tổng tiền cuối cùng
    TinhTongTien();
}
    }

}
