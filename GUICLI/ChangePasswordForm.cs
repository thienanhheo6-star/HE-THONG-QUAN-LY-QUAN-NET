using System;
using System.Windows.Forms;
using DAL.Models;

namespace GUICLI
{
    public class ChangePasswordForm : Form
    {
        private Label lblNew;
        private Label lblConfirm;
        private TextBox txtNew;
        private TextBox txtConfirm;
        private Button btnOk;
        private Button btnCancel;

        public string NewPassword { get; private set; }

        public ChangePasswordForm(TaiKhoanNguoiChoi account)
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.lblNew = new Label() { Text = "Mật khẩu mới:", Left = 10, Top = 10, Width = 100 };
            this.txtNew = new TextBox() { Left = 120, Top = 10, Width = 180, UseSystemPasswordChar = true };
            this.lblConfirm = new Label() { Text = "Xác nhận:", Left = 10, Top = 40, Width = 100 };
            this.txtConfirm = new TextBox() { Left = 120, Top = 40, Width = 180, UseSystemPasswordChar = true };
            this.btnOk = new Button() { Text = "OK", Left = 120, Top = 75, Width = 80 };
            this.btnCancel = new Button() { Text = "Hủy", Left = 220, Top = 75, Width = 80 };

            this.btnOk.Click += BtnOk_Click;
            this.btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.ClientSize = new System.Drawing.Size(320, 120);
            this.Controls.AddRange(new Control[] { lblNew, txtNew, lblConfirm, txtConfirm, btnOk, btnCancel });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Đổi mật khẩu";
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var a = txtNew.Text.Trim();
            var b = txtConfirm.Text.Trim();
            if (string.IsNullOrEmpty(a))
            {
                MessageBox.Show("Mật khẩu không được rỗng", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (a != b)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NewPassword = a;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
