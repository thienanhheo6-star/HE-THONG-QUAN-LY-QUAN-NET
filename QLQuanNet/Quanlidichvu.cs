using DAL.Models;
using System.Data.Entity.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QLQuanNet
{
    public partial class Quanlidichvu : Form
    {
        readonly Model1 db = new Model1();
        string duongDanAnhMoi = "";
        public Quanlidichvu()
        {
            InitializeComponent();
        }

        private void Quanlidichvu_Load(object sender, EventArgs e)
        {

        }

        private void picAnhHienTai_Click(object sender, EventArgs e)
        {

        }

        private void dgvDanhSach_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void Button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            {
                // Kiểm tra nếu click vào tiêu đề cột thì bỏ qua
                if (e.RowIndex < 0) return;

                try
                {
                    // 1. Lấy ID của dòng đang chọn (Cột đầu tiên - Cells[0])
                    // Nếu ID của bạn nằm cột khác thì sửa số 0
                    int id = int.Parse(dgvDanhSach.Rows[e.RowIndex].Cells[0].Value.ToString());

                    // 2. Tìm món ăn trong Database
                    var item = db.DICHVUs.Find(id);

                    if (item != null)
                    {
                        // --- Đổ dữ liệu chữ ---
                        txtTenDV.Text = item.TenDV;

                        // Hiển thị giá tiền (format đẹp nếu cần)
                        txtGia.Text = item.DonGia.ToString();

                        // Lưu ID vào Tag nút Sửa (để tí nữa biết đang sửa món nào)
                        btnSua.Tag = item.MaDV;

                        // --- XỬ LÝ HÌNH ẢNH (QUAN TRỌNG) ---

                        string tenAnh = "";
                        // Lấy tên file và cắt bỏ khoảng trắng thừa
                        if (item.HinhAnh != null) tenAnh = item.HinhAnh.Trim();

                        if (!string.IsNullOrEmpty(tenAnh))
                        {
                            // Gọi hàm lấy đường dẫn Models\Images ở trên
                            string thuMucAnh = LayDuongDanThuMucAnh();
                            string duongDanDayDu = Path.Combine(thuMucAnh, tenAnh);

                            // Kiểm tra file có tồn tại không
                            if (File.Exists(duongDanDayDu))
                            {
                                // Load ảnh lên (Dùng FileStream để tránh lỗi khóa file)
                                using (FileStream fs = new FileStream(duongDanDayDu, FileMode.Open, FileAccess.Read))
                                {
                                    picAnhHienTai.Image = Image.FromStream(fs);
                                }
                                picAnhHienTai.SizeMode = PictureBoxSizeMode.Zoom;
                            }
                            else
                            {
                                // Có tên trong DB nhưng không thấy file -> Xóa trắng
                                picAnhHienTai.Image = null;
                            }
                        }
                        else
                        {
                            // Không có tên ảnh -> Xóa trắng
                            picAnhHienTai.Image = null;
                        }

                        // Reset biến đường dẫn tạm (để tránh nhầm lẫn khi bấm nút Sửa ngay sau đó)
                        duongDanAnhMoi = "";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi khi chọn dòng: " + ex.Message);
                }
            }
        }

        private void Quanlidichvu_Load_1(object sender, EventArgs e)
        {
            LoadData();
            duongDanAnhMoi = "";
        }
        private void LoadData()
        {
            dgvDanhSach.DataSource = db.DICHVUs
                .AsEnumerable()
                .Select(d => new {
                    d.MaDV,
                    d.TenDV,
                    d.DonGia,
                    d.HinhAnh,
                })
                .ToList();
            if (dgvDanhSach.Columns["MaDV"] != null)
            {
                dgvDanhSach.Columns["MaDV"].Visible = false; // Ẩn cột Mã
            }

            // 3. Đặt tên đẹp cho các cột còn lại
            dgvDanhSach.Columns["TenDV"].HeaderText = "Tên Món";
            dgvDanhSach.Columns["DonGia"].HeaderText = "Đơn Giá";
            if (dgvDanhSach.Columns["HinhAnh"] != null)
                dgvDanhSach.Columns["HinhAnh"].HeaderText = "Hình Ảnh";

            // Chỉnh lại kích thước cho đẹp (Tùy chọn)
            dgvDanhSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog
            {
                // Set thư mục mặc định là Models\Images
                InitialDirectory = LayDuongDanThuMucAnh(),

                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp"
            };

            if (open.ShowDialog() == DialogResult.OK)
            {
                picAnhHienTai.Image = Image.FromFile(open.FileName);
                picAnhHienTai.SizeMode = PictureBoxSizeMode.Zoom;
                duongDanAnhMoi = open.FileName;
            }
        }
        
        private void Button1_Click_1(object sender, EventArgs e)
        {
            // 1. Tạo đối tượng Dịch vụ mới
            DICHVU dv = new DICHVU
            {
                TenDV = txtTenDV.Text
            };

            // Xử lý giá tiền (dùng decimal như mình đã sửa lỗi cho bạn)
            decimal gia = 0;
            decimal.TryParse(txtGia.Text, out gia);
            dv.DonGia = gia;

            // --- 2. XỬ LÝ LƯU HÌNH ẢNH (QUAN TRỌNG) ---
            if (duongDanAnhMoi != "")
                if (duongDanAnhMoi != "")
                {
                    // GỌI HÀM MỚI ĐỂ LẤY ĐÍCH ĐẾN
                    string thuMucDich = LayDuongDanThuMucAnh();

                    string tenFileGoc = Path.GetFileName(duongDanAnhMoi);
                    string duongDanFileDich = Path.Combine(thuMucDich, tenFileGoc);

                    // Copy file vào thư mục Models\Images
                    // (Chỉ copy nếu nguồn và đích khác nhau)
                    if (duongDanAnhMoi != duongDanFileDich)
                    {
                        File.Copy(duongDanAnhMoi, duongDanFileDich, true);
                    }

                    dv.HinhAnh = tenFileGoc;
                }

            // 3. Lưu vào Database
            db.DICHVUs.Add(dv);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var sb = new StringBuilder();
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        sb.AppendLine($"{ve.PropertyName}: {ve.ErrorMessage}");
                    }
                }
                MessageBox.Show("Validation error:\n" + sb.ToString());
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                return;
            }

            MessageBox.Show("Thêm món mới thành công!");
            LoadData(); // Tải lại bảng

            // Reset biến đường dẫn
            duongDanAnhMoi = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (btnSua.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn món cần sửa!");
                return;
            }

            int idCanSua = (int)btnSua.Tag;
            var dv = db.DICHVUs.Find(idCanSua);

            if (dv != null)
            {
                dv.TenDV = txtTenDV.Text;
                decimal gia = 0;
                decimal.TryParse(txtGia.Text, out gia);
                dv.DonGia = gia;

                // Nếu người dùng có chọn ảnh mới thì cập nhật, không thì giữ nguyên ảnh cũ
                if (!string.IsNullOrEmpty(duongDanAnhMoi))
                {
                    string thuMucDuAn = Path.Combine(Application.StartupPath, "Images");

                    // Lấy tên gốc
                    string tenFileGoc = Path.GetFileName(duongDanAnhMoi);
                    string duongDanDich = Path.Combine(thuMucDuAn, tenFileGoc);

                    // Copy file (ghi đè nếu trùng)
                    if (duongDanAnhMoi != duongDanDich)
                    {
                        File.Copy(duongDanAnhMoi, duongDanDich, true);
                    }

                    // Cập nhật tên ảnh vào đối tượng dịch vụ
                    dv.HinhAnh = tenFileGoc;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var sb = new StringBuilder();
                    foreach (var eve in ex.EntityValidationErrors)
                        foreach (var ve in eve.ValidationErrors)
                            sb.AppendLine($"{ve.PropertyName}: {ve.ErrorMessage}");
                    MessageBox.Show("Validation error:\n" + sb.ToString());
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi lưu: " + ex.Message);
                    return;
                }
                MessageBox.Show("Cập nhật thành công!");
                LoadData();
                LamMoiO();
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (btnSua.Tag == null)
            {
                MessageBox.Show("Vui lòng chọn món cần xóa!");
                return;
            }

            if (MessageBox.Show("Bạn chắc chắn muốn xóa?", "Cảnh báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                int idCanXoa = (int)btnSua.Tag;
                var dv = db.DICHVUs.Find(idCanXoa);

                if (dv != null)
                {
                    try
                    {
                        db.DICHVUs.Remove(dv);
                        db.SaveChanges();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var sb = new StringBuilder();
                        foreach (var eve in ex.EntityValidationErrors)
                            foreach (var ve in eve.ValidationErrors)
                                sb.AppendLine($"{ve.PropertyName}: {ve.ErrorMessage}");
                        MessageBox.Show("Validation error:\n" + sb.ToString());
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xóa: " + ex.Message);
                        return;
                    }
                    LoadData();
                    LamMoiO();
                    MessageBox.Show("Đã xóa!");
                }
            }
        }
        private void LamMoiO()
        {
            txtTenDV.Text = "";
            txtGia.Text = "";
            picAnhHienTai.Image = null;
            btnSua.Tag = null;
            duongDanAnhMoi = "";
        }

       

        private void dgvDanhSach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            try
            {
                // 1. Lấy ID của dòng đang chọn (Cột đầu tiên - Cells[0])
                // Nếu ID của bạn nằm cột khác thì sửa số 0
                int id = int.Parse(dgvDanhSach.Rows[e.RowIndex].Cells[0].Value.ToString());

                // 2. Tìm món ăn trong Database
                var item = db.DICHVUs.Find(id);

                if (item != null)
                {
                    // --- Đổ dữ liệu chữ ---
                    txtTenDV.Text = item.TenDV;

                    // Hiển thị giá tiền (format đẹp nếu cần)
                    txtGia.Text = item.DonGia.ToString();

                    // Lưu ID vào Tag nút Sửa (để tí nữa biết đang sửa món nào)
                    btnSua.Tag = item.MaDV;

                    // --- XỬ LÝ HÌNH ẢNH (QUAN TRỌNG) ---

                    string tenAnh = "";
                    // Lấy tên file và cắt bỏ khoảng trắng thừa
                    if (item.HinhAnh != null) tenAnh = item.HinhAnh.Trim();

                    if (!string.IsNullOrEmpty(tenAnh))
                    {
                        // Gọi hàm lấy đường dẫn Models\Images ở trên
                        string thuMucAnh = LayDuongDanThuMucAnh();
                        string duongDanDayDu = Path.Combine(thuMucAnh, tenAnh);

                        // Kiểm tra file có tồn tại không
                        if (File.Exists(duongDanDayDu))
                        {
                            // Load ảnh lên (Dùng FileStream để tránh lỗi khóa file)
                            using (FileStream fs = new FileStream(duongDanDayDu, FileMode.Open, FileAccess.Read))
                            {
                                picAnhHienTai.Image = Image.FromStream(fs);
                            }
                            picAnhHienTai.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else
                        {
                            // Có tên trong DB nhưng không thấy file -> Xóa trắng
                            picAnhHienTai.Image = null;
                        }
                    }
                    else
                    {
                        // Không có tên ảnh -> Xóa trắng
                        picAnhHienTai.Image = null;
                    }

                    // Reset biến đường dẫn tạm (để tránh nhầm lẫn khi bấm nút Sửa ngay sau đó)
                    duongDanAnhMoi = "";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi khi chọn dòng: " + ex.Message);
            }
        }

        private void picAnhHienTai_Click_1(object sender, EventArgs e)
        {

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

        private void BtnLM_Click(object sender, EventArgs e)
        {
            LamMoiO();   
            LoadData();
        }
    }
}
