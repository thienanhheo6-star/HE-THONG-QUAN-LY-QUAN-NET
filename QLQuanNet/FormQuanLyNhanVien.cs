using System;
using System.Linq;
using System.Windows.Forms;
using DAL.Models;

namespace QLQuanNet
{
    public partial class FormQuanLyNhanVien : Form
    {
        private EmployeeService employeeService;
        private BindingSource employeeBindingSource = new BindingSource();
        private Timer hireDateTimer;
        private bool isViewingEmployee = false;
        public FormQuanLyNhanVien()
        {
            InitializeComponent();

            employeeService = new EmployeeService();
            employeeBindingSource = new BindingSource();

            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = employeeBindingSource;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

            // ===== DateTimePicker khóa nhưng chạy realtime =====
            dateTimePickerHire.Enabled = false;

            hireDateTimer = new Timer();
            hireDateTimer.Interval = 1000;
            hireDateTimer.Tick += HireDateTimer_Tick;
            hireDateTimer.Start();
        }

        private void FormQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            // Không cho sửa
            dateTimePickerHire.Enabled = false;

            dateTimePickerHire.Format = DateTimePickerFormat.Custom;
            dateTimePickerHire.CustomFormat = "dd/MM/yyyy HH:mm:ss";

            // Timer realtime
            hireDateTimer = new Timer();
            hireDateTimer.Interval = 1000; // 1 giây
            hireDateTimer.Tick += HireDateTimer_Tick;
            hireDateTimer.Start();

            LoadData();
        }
        private void HireDateTimer_Tick(object sender, EventArgs e)
        {
            if (!isViewingEmployee)
            {
                dateTimePickerHire.Value = DateTime.Now;
            }
        }



        // ================= LOAD DATA =================
        private void LoadData()
        {
            using (var db = new Model1())
            {
                employeeBindingSource.DataSource = db.NhanViens.ToList();
            }
        }


        // ================= THÊM =================
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            isViewingEmployee = false;       
            hireDateTimer.Start();
            if (!decimal.TryParse(textBoxSalary.Text, out decimal salary))
            {
                MessageBox.Show("Lương không hợp lệ!");
                return;
            }
            const decimal MAX_SALARY = 100_000_000m;
            if (salary <= 0 || salary > MAX_SALARY)
            {
                MessageBox.Show($"Lương phải nằm trong khoảng 1 - {MAX_SALARY:N0} ₫");
                return;
            }

            using (var db = new Model1())
            {
                var nv = new NhanVien
                {
                    TenNhanVien = textBoxName.Text,
                    DienThoai = textBoxPhone.Text,
                    Luong = salary,
                    ChucVu = textBoxPosition.Text,
                    NgayVaoLam = DateTime.Now
                };


                db.NhanViens.Add(nv);
                db.SaveChanges();
            }

            LoadData();
            ClearInput();
            MessageBox.Show("Thêm nhân viên thành công!");
        }

        private void TextBoxSalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        // ================= XÓA =================
        private void ButtonDelete_Click(object sender, EventArgs e)
        {
            if (employeeBindingSource.Current == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên!");
                return;
            }

            var nv = (NhanVien)employeeBindingSource.Current;

            if (MessageBox.Show("Xóa nhân viên này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.No) return;

            using (var db = new Model1())
            {
                var entity = db.NhanViens.Find(nv.MaNhanVien);
                if (entity != null)
                {
                    db.NhanViens.Remove(entity);
                    db.SaveChanges();
                }
            }

            LoadData();
        }


        // ================= CLICK → ĐỔ DATA =================
        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (employeeBindingSource.Current == null) return;

            isViewingEmployee = true;        // 🔒 Đóng băng thời gian
            hireDateTimer.Stop();

            var nv = (NhanVien)employeeBindingSource.Current;

            textBoxName.Text = nv.TenNhanVien;
            textBoxPhone.Text = nv.DienThoai;
            textBoxSalary.Text = nv.Luong.ToString();
            textBoxPosition.Text = nv.ChucVu;
            dateTimePickerHire.Value = nv.NgayVaoLam ?? DateTime.Now;
        }


        private void RefreshGroupBoxInfo()
        {
            foreach (Control ctrl in groupBoxInfo.Controls)
            {
                // Reset TextBox
                if (ctrl is TextBox tb)
                {
                    tb.Clear();
                }
                // Reset DateTimePicker
                else if (ctrl is DateTimePicker dtp)
                {
                    dtp.Value = DateTime.Now;
                }
            }

            // Reset trạng thái logic
            isViewingEmployee = false;
            hireDateTimer.Start();
        }

        private void ButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshGroupBoxInfo();
            dataGridView1.ClearSelection(); // bỏ chọn dòng đang xem
        }


        // ================= CLEAR =================
        private void ClearInput()
        {
            isViewingEmployee = false;
            hireDateTimer.Start();

            textBoxName.Clear();
            textBoxPhone.Clear();
            textBoxSalary.Clear();
            textBoxPosition.Clear();
            dateTimePickerHire.Value = DateTime.Now;
        }

    }
}
