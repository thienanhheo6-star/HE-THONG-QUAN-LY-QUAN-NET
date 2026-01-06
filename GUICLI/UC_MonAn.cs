using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace QLQuanNet
{
    public partial class UC_MonAn : UserControl
    {
        public event EventHandler OnSelect = null;
        public UC_MonAn()
        {
            InitializeComponent();
            this.picHinhAnh.Click += UC_MonAn_Click;
            this.lblTenMon.Click += UC_MonAn_Click;
            this.lblGia.Click += UC_MonAn_Click;
            this.Click += UC_MonAn_Click; // Click vào nền
        }
        public int Id { get; set; }
        public string TenMon
        {
            get { return lblTenMon.Text; }
            set { lblTenMon.Text = value; }
        }
        public double Gia
        {
            get { return double.Parse(lblGia.Text.Replace(",", "")); } // Xử lý format tiền nếu cần
            set { lblGia.Text = value.ToString("#,##0"); } // Format số đẹp (ví dụ: 10,000)
        }
        public string HinhAnhPath { get; set; }
        public void SetImage(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    picHinhAnh.Image = Image.FromFile(path);
                    picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                else
                {
                    // Ảnh mặc định nếu không tìm thấy file
                    // picHinhAnh.Image = Properties.Resources.default_food; 
                }
            }
            catch
            {
                // Bỏ qua lỗi ảnh
            }
        }
        public void SetImage(Image img)
        {
            picHinhAnh.Image = img;
            picHinhAnh.SizeMode = PictureBoxSizeMode.Zoom;
        }
        private void UC_MonAn_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picHinhAnh_Click(object sender, EventArgs e)
        {

        }
        private void UC_MonAn_Click(object sender, EventArgs e)
        {
            // Kích hoạt sự kiện OnSelect để FormDichVu nhận được
            OnSelect?.Invoke(this, e);
        }

        private void lblGia_Click(object sender, EventArgs e)
        {

        }
        public void LoadDuLieu(int id, string ten, double gia, string tenAnh)
        {
            // 1. Gán các thuộc tính cơ bản
            this.Id = id;
            this.TenMon = ten;
            this.Gia = gia;

            // 2. Xử lý hiển thị ảnh
            try
            {
                if (!string.IsNullOrEmpty(tenAnh))
                {
                    // Đường dẫn ảnh
                    string path = Path.Combine(Application.StartupPath, "Images", tenAnh);

                    if (File.Exists(path))
                    {
                        picHinhAnh.Image = Image.FromFile(path);
                        picHinhAnh.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
            }
            catch { }
        }
    }
}
