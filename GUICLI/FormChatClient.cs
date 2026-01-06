using GUICLI;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
namespace GUICLI
{
    public partial class FormChatClient : Form
    {
        private FormCLI parentForm;

        public FormChatClient(FormCLI parent)

        {
            InitializeComponent();
            this.parentForm = parent;
            this.txtMessage.KeyDown -= new KeyEventHandler(txtMessage_KeyDown);
            this.txtMessage.KeyDown += new KeyEventHandler(txtMessage_KeyDown);
            this.btnGuiTin.Click -= new EventHandler(btnGuiTin_Click);
            this.btnGuiTin.Click += new EventHandler(btnGuiTin_Click);
        }
        
        private void BtnSend_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Nút đã bấm được! Đang chuẩn bị gửi...");
            GuiTinDi();
        }
        private void FormChatClient_Load(object sender, EventArgs e)
        {
            if (lstChat != null)
            {
                lstChat.Items.Add("--------------------------------------------------");
                lstChat.Items.Add("✅ Hệ thống: Đã kết nối với Server thành công!");
                lstChat.Items.Add("ℹ Bạn có thể bắt đầu nhắn tin hỗ trợ.");
                lstChat.Items.Add("--------------------------------------------------");
            }
            this.ActiveControl = txtMessage;
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GuiTinDi();
                e.SuppressKeyPress = true; // Chặn tiếng "Bíp"
            }
        }
        private void GuiTinDi()
        {
            string msg = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(msg)) return;

            // 1. Gửi qua Form Cha (Server nhận)
            if (parentForm != null)
            {
                parentForm.SendChatMessage(msg);
            }
            else
            {
                MessageBox.Show("Lỗi: Mất kết nối với Form chính!");
                return;
            }

            // 2. Hiện lên màn hình của mình
            if (lstChat != null)
            {
                lstChat.Items.Add("Tôi: " + msg);
                lstChat.TopIndex = lstChat.Items.Count - 1; // Tự cuộn xuống
            }

            // 3. XÓA CHỮ VỪA NHẬP
            txtMessage.Clear();
            txtMessage.Focus();
        }
        
        public void NhanTinTuServer(string tinNhan)
        {
            this.Invoke(new Action(() => {
                if (lstChat != null)
                {
                    lstChat.Items.Add(tinNhan);
                    lstChat.TopIndex = lstChat.Items.Count - 1;
                }
            }));
        }

        // Nút thoát (nếu có)
        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnGuiTin_Click(object sender, EventArgs e)
        {
            GuiTinDi();
        }
    }
}
