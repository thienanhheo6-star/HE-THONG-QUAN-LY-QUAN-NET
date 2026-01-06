using DAL.Models;
using QLQuanNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace QLQuanNet
{
    public partial class FormNhanTin : Form
    {
        FormMain formCha;
        public FormNhanTin(FormMain main)
        {
            InitializeComponent();
            this.formCha = main;
        }
        private void FormNhanTin_Shown(object sender, EventArgs e)
        {
        }


        public void HienThiTinNhan(string noiDung)
        {
            if (this.IsDisposed) return;

            // Thêm vào ListBox
            if (lstMessages != null)
            {
                string thoiGian = DateTime.Now.ToString("HH:mm:ss");
                lstMessages.Items.Add($"[{thoiGian}] Client: {noiDung}");

                // Tự động cuộn xuống dòng mới nhất
                lstMessages.TopIndex = lstMessages.Items.Count - 1;
            }
        }

        // Gửi tin nhắn từ Server xuống TẤT CẢ Client (Broadcast)
        private void btnGui_Click(object sender, EventArgs e)
        {

        }
        private void BtnReply_Click(object sender, EventArgs e)
        {
            string msg = txtReply.Text.Trim();
            if (string.IsNullOrEmpty(msg)) return;

            // Nhờ FormMain gửi hộ
            if (formCha != null)
            {
                formCha.GuiTinNhanChoTatCa("Admin: " + msg);

                // Hiện lên khung chat của mình
                lstMessages.Items.Add("Admin: " + msg);
                txtReply.Clear();
                txtReply.Focus();
            }
            else
            {
                MessageBox.Show("Lỗi: Không tìm thấy Server Main!");
            }
        }
        
        private void FormNhanTin_Load(object sender, EventArgs e)
        {
            if (lstMessages != null)
            {
                lstMessages.Items.Add("Hệ thống: Đã kết nối với bộ xử lý trung tâm.");
                lstMessages.Items.Add("Sẵn sàng chat với Client...");
            }
        }

        private void txtReply_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
