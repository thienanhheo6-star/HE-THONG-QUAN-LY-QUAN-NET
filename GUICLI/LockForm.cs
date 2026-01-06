using System;
using System.Windows.Forms;
using DAL.Models;

namespace GUICLI
{
    public class LockForm : Form
    {
        private Label lblInfo;
        private TextBox txtPass;
        private Button btnUnlock;
        private TaiKhoanNguoiChoi account;
        private bool unlocked = false;

        public LockForm(TaiKhoanNguoiChoi account)
        {
            this.account = account;
            BuildUI();
        }

        // Build UI programmatically to avoid designer InitializeComponent parsing
        private void BuildUI()
        {
            this.lblInfo = new Label() { Text = "Nhập mật khẩu để mở khóa:", Left = 10, Top = 10, Width = 250 };
            this.txtPass = new TextBox() { Left = 10, Top = 40, Width = 260, UseSystemPasswordChar = true };
            this.btnUnlock = new Button() { Text = "Mở khóa", Left = 10, Top = 75, Width = 120 };
            this.btnUnlock.Click += BtnUnlock_Click;

            this.ClientSize = new System.Drawing.Size(290, 115);
            this.Controls.AddRange(new Control[] { lblInfo, txtPass, btnUnlock });
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "Khóa máy";
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
        }

        private void BtnUnlock_Click(object sender, EventArgs e)
        {
            if (account == null)
            {
                MessageBox.Show("Không có tài khoản để kiểm tra.", "Lỗi");
                return;
            }

            if (txtPass.Text == account.MatKhau)
            {
                unlocked = true;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Mật khẩu sai.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Clear();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Prevent closing the lock form unless it was unlocked
            if (!unlocked)
            {
                e.Cancel = true;
                return;
            }

            base.OnFormClosing(e);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // LockForm
            // 
            this.ClientSize = new System.Drawing.Size(316, 199);
            this.Name = "LockForm";
            this.ResumeLayout(false);

        }
    }
}
