using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DAL.Models;

namespace QLQuanNet
{
    public partial class FormMayTinh : Form
    {
        private MAYTINH _selectedMay = null;
        private Panel _selectedCard = null;

        public FormMayTinh()
        {
            InitializeComponent();
            InitUI();
            LoadData();
        }

        // ================= INIT =================
        private void InitUI()
        {
            cboTrangThai.Items.AddRange(new string[]
            {
                "Trống",
                "Đang sử dụng",
              
            });
            cboTrangThai.SelectedIndex = 0;

            btnThem.Click += btnThem_Click;
            btnSua.Click += btnSua_Click;
            btnBuocXoa.Click += btnBuocXoa_Click;
        }

        // ================= LOAD DATA =================
        private void LoadData()
        {
            flowPanelComputers.Controls.Clear();

            using (var db = new Model1())
            {
                var list = db.MAYTINHs.OrderBy(m => m.MaMay).ToList();
                foreach (var m in list)
                {
                    AddComputerCard(m);
                }
            }
        }

        // ================= ADD CARD =================
        private void AddComputerCard(MAYTINH m)
        {
            Panel card = new Panel
            {
                Size = new Size(150, 180),
                BackColor = Color.FromArgb(45, 45, 55),
                Margin = new Padding(15),
                Cursor = Cursors.Hand,
                Tag = m
            };

            Label lblIcon = new Label
            {
                Text = "💻",
                Font = new Font("Segoe UI", 36F),
                Dock = DockStyle.Top,
                Height = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = m.TrangThai == "Trống" ? Color.LimeGreen : Color.OrangeRed
            };

            Label lblName = new Label
            {
                Text = m.TenMay,
                Dock = DockStyle.Top,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblPrice = new Label
            {
                Text = $"{m.GiaTheoGio:N0} đ/h",
                Dock = DockStyle.Bottom,
                ForeColor = Color.LightGray,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblName);
            card.Controls.Add(lblIcon);
            card.Controls.Add(lblPrice);

            card.Click += Card_Click;
            foreach (Control c in card.Controls)
                c.Click += Card_Click;

            flowPanelComputers.Controls.Add(card);
        }

        // ================= CLICK SELECT =================
        private void Card_Click(object sender, EventArgs e)
        {
            Panel card = sender as Panel ?? (sender as Control)?.Parent as Panel;
            if (card == null) return;

            if (_selectedCard != null)
                _selectedCard.BackColor = Color.FromArgb(45, 45, 55);

            _selectedCard = card;
            _selectedCard.BackColor = Color.FromArgb(70, 70, 100);

            _selectedMay = card.Tag as MAYTINH;

            txtTenMay.Text = _selectedMay.TenMay;
            txtGia.Text = _selectedMay.GiaTheoGio.ToString();
            cboTrangThai.Text = _selectedMay.TrangThai;
        }

        // ================= ADD =================
        private void btnThem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenMay.Text)) return;

            using (var db = new Model1())
            {
                var m = new MAYTINH
                {
                    TenMay = txtTenMay.Text,
                    GiaTheoGio = decimal.Parse(txtGia.Text),
                    TrangThai = cboTrangThai.Text
                };
                db.MAYTINHs.Add(m);
                db.SaveChanges();
            }

            LoadData();
            ClearInput();
        }

        // ================= UPDATE =================
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (_selectedMay == null) return;

            using (var db = new Model1())
            {
                var m = db.MAYTINHs.Find(_selectedMay.MaMay);
                if (m == null) return;

                m.TenMay = txtTenMay.Text;
                m.GiaTheoGio = decimal.Parse(txtGia.Text);
                m.TrangThai = cboTrangThai.Text;

                db.SaveChanges();
            }

            LoadData();
            ClearInput();
        }

        // ================= DELETE =================
        private void btnBuocXoa_Click(object sender, EventArgs e)
        {
            if (_selectedMay == null) return;

            if (MessageBox.Show("Xóa máy này?", "Xác nhận",
                MessageBoxButtons.YesNo) == DialogResult.No) return;

            using (var db = new Model1())
            {
                var m = db.MAYTINHs.Find(_selectedMay.MaMay);
                if (m != null)
                {
                    // Prevent deleting a computer that is referenced by SuDungMay (FK constraint)
                    var inUse = db.SuDungMays.Any(s => s.MaMay == m.MaMay);
                    if (inUse)
                    {
                        MessageBox.Show("Không thể xóa máy này vì có dữ liệu sử dụng liên quan.\nHãy xóa hoặc chuyển các bản ghi liên quan trước.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    db.MAYTINHs.Remove(m);
                    db.SaveChanges();
                }
            }

            LoadData();
            ClearInput();
        }

        // ================= CLEAR =================
        private void ClearInput()
        {
            txtTenMay.Clear();
            txtGia.Clear();
            cboTrangThai.SelectedIndex = 0;
            _selectedMay = null;
            _selectedCard = null;
        }
    }
}
