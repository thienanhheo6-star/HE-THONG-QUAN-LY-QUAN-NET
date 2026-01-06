using DAL.Models;
using GUICLI;
using QLQuanNet;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;

namespace GUICLI // <-- Đảm bảo tên này khớp với Project của bạn
{
    public partial class FormCLI : Form
    {
        // ===== BIẾN MẠNG (ĐƯỢC TRUYỀN TỪ LOGIN SANG) =====
        private TcpClient client;
        private StreamReader reader;
        private StreamWriter writer;
        private Thread receiveThread;
        private bool isConnected = true;
        private string userName;

        // Heartbeat timer
        private System.Timers.Timer heartbeatTimer;

        // ===== BIẾN QUẢN LÝ =====
        private bool daBao5Phut = false;
        private bool daBao3Phut = false;
        private string MY_MACHINE_ID = "";
        private Model1 db = new Model1();
        private System.Windows.Forms.Timer timer;
        private SuDungMay currentSession;
        private MAYTINH may;
        private TaiKhoanNguoiChoi account;
        public FormChatClient formChatDangMo = null; // Form chat con

        // --- 1. CONSTRUCTOR MỚI: NHẬN KẾT NỐI TỪ LOGIN ---
        public FormCLI(TcpClient socketDaKetNoi, string tenUser, string maySo)
        {
            InitializeComponent();

            // Nhận "chìa khóa" từ FormLogin
            this.client = socketDaKetNoi;
            this.userName = tenUser;
            this.MY_MACHINE_ID = maySo; // <--- LƯU SỐ MÁY VÀO ĐÂY

            this.Text = $"Client: {tenUser} - [Máy {maySo}]";

            // Thiết lập luồng đọc/ghi (Tiếp tục dùng kết nối cũ)
            try
            {
                NetworkStream stream = client.GetStream();
                this.reader = new StreamReader(stream);
                this.writer = new StreamWriter(stream) { AutoFlush = true };

                // Bắt đầu luồng lắng nghe của riêng FormCLI
                receiveThread = new Thread(ReceiveMessage);
                receiveThread.IsBackground = true;
                receiveThread.Start();

                // Start heartbeat timer (gửi PING mỗi 5 giây)
                heartbeatTimer = new System.Timers.Timer(5000);
                heartbeatTimer.Elapsed += (s, ev) => {
                    try { SendPing(); } catch { }
                };
                heartbeatTimer.AutoReset = true;
                heartbeatTimer.Start();
            }
            catch
            {
                MessageBox.Show("Lỗi tiếp nhận kết nối mạng!");
            }
        }

        // Constructor mặc định (để tránh lỗi Designer)
        public FormCLI() { InitializeComponent(); }

        // --- 2. HÀM LẮNG NGHE (CHỈ CÓ 1 HÀM DUY NHẤT) ---
        private void ReceiveMessage()
        {
            try
            {
                while (isConnected)
                {
                    // 1. Đọc tin nhắn
                    string line = reader.ReadLine();
                    if (line == null) break;

                    // 2. Tách lệnh (Thêm Trim để xóa khoảng trắng thừa nếu có)
                    string[] parts = line.Split('|');
                    string command = parts[0].Trim();
                    string content = (parts.Length > 1) ? parts[1].Trim() : "";

                    // 3. KIỂM TRA: ĐÂY LÀ CHAT HAY LÀ LỆNH?

                    // === NẾU LÀ CHAT ===
                    if (command == "CHAT")
                    {
                        // Chỉ hiện tin nhắn, không làm gì khác
                        if (formChatDangMo != null && !formChatDangMo.IsDisposed)
                        {
                            formChatDangMo.Invoke(new Action(() => formChatDangMo.NhanTinTuServer(content)));
                        }
                        else
                        {
                            this.Invoke(new Action(() => MessageBox.Show("Admin nhắn: " + content)));
                        }
                    }

                    // === NẾU LÀ LỆNH ĐÁ MÁY (SỬA CHỖ NÀY) ===
                    else if (command == "FORCE_LOGOUT")
                    {
                        // Kiểm tra xem Server đá đúng máy mình không?
                        // MY_MACHINE_ID là biến lưu số máy (Ví dụ: "2")
                        if (content == MY_MACHINE_ID)
                        {
                            this.Invoke(new Action(() => {
                                // Hiện thông báo (Tùy chọn, có thể bỏ dòng này nếu muốn tắt luôn)
                                MessageBox.Show("Bị máy chủ đá rồi.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                // GỌI HÀM THOÁT NGAY LẬP TỨC
                                btnLogout_Click(null, null);
                            }));
                        }
                    }
                }
            }
            catch
            {
                isConnected = false;
            }
        }
        public void SendChatMessage(string message)
        {
            if (isConnected && writer != null)
            {
                writer.WriteLine($"CHAT|{message}");
            }
        }

        private void SendPing()
        {
            try
            {
                if (isConnected && writer != null)
                {
                    writer.WriteLine($"PING|{MY_MACHINE_ID}");
                }
            }
            catch { }
        }

        // Gửi command thô tới server (dùng bởi các form con như FormDichVu)
        public void SendCommandToServer(string command)
        {
            try
            {
                if (isConnected && writer != null)
                {
                    writer.WriteLine(command);
                }
            }
            catch { }
        }

        // --- 4. LOAD & TÍNH TIỀN ---
        private void Form1_Load(object sender, EventArgs e)
        {
            lblStatus.Text = "Đã kết nối";
            if (lblUser != null) lblUser.Text = userName;

            // Tìm thông tin tài khoản
            account = db.TaiKhoanNguoiChois.FirstOrDefault(t => t.TenDangNhap == userName);

            // Tìm phiên chơi
            if (account != null)
            {
                currentSession = db.SuDungMays.FirstOrDefault(s => s.MaTK == account.MaTK && s.ThoiGianKetThuc == null);
                if (currentSession != null)
                {
                    may = db.MAYTINHs.FirstOrDefault(m => m.MaMay == currentSession.MaMay);
                }
            }

            UpdateOnce();

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // --- 1. KIỂM TRA DỮ LIỆU (DEBUG) ---
            // Nếu thiếu dữ liệu, in lên tiêu đề để biết ngay
            if (currentSession == null) { this.Text = "Lỗi: Thiếu CurrentSession"; return; }
            if (account == null) { this.Text = "Lỗi: Thiếu Account"; return; }
            if (may == null) { this.Text = "Lỗi: Thiếu Thông tin máy"; return; }

            // --- 2. TÍNH TOÁN ---
            var duration = DateTime.Now - currentSession.ThoiGianBatDau;

            // Hiển thị text box (giữ nguyên code của bạn)
            if (txtUsedTime != null) txtUsedTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", duration.Hours, duration.Minutes, duration.Seconds);

            decimal playCost = ((decimal)duration.TotalHours) * may.GiaTheoGio;
            if (txtPlayCost != null) txtPlayCost.Text = string.Format("{0:N0} VNĐ", playCost);

            var serviceCost = db.SuDungDichVus.Where(s => s.MaSD == currentSession.MaSD).Sum(s => (decimal?)s.ThanhTien) ?? 0m;

            // Tính thời gian còn lại
            decimal tongChiPhi = playCost + serviceCost;
            decimal tienConLai = account.SoDu - tongChiPhi;

            // Ép kiểu rõ ràng để tránh lỗi chia số
            double giaMoiGio = (double)may.GiaTheoGio;
            if (giaMoiGio <= 0) giaMoiGio = 1; // Tránh chia cho 0

            double gioConLai = (double)tienConLai / giaMoiGio;
            TimeSpan timeRemain = TimeSpan.FromHours(gioConLai);
            double phutConLai = timeRemain.TotalMinutes;

            if (txtRemainTime != null)
                txtRemainTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)timeRemain.TotalHours, timeRemain.Minutes, timeRemain.Seconds);

            // --- 3. IN RA THANH TIÊU ĐỀ ĐỂ THEO DÕI (QUAN TRỌNG) ---
            // Nhìn lên góc trên cùng cửa sổ để xem máy tính đang nghĩ gì
            this.Text = string.Format("Client");

            // --- 4. LOGIC CẢNH BÁO ---

            // Mốc 5 Phút (Từ 3.1 đến 5.0)
            if (phutConLai <= 5.0 && phutConLai > 3.0)
            {
                if (!daBao5Phut) // Nếu chưa báo
                {
                    daBao5Phut = true; // Đánh dấu ngay lập tức
                    MessageBox.Show("Sắp hết giờ: Còn dưới 5 phút!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            // Mốc 3 Phút (Từ 0.1 đến 3.0)
            if (phutConLai <= 3.0 && phutConLai > 0)
            {
                if (!daBao3Phut) // Nếu chưa báo
                {
                    daBao3Phut = true; // Đánh dấu ngay lập tức
                    MessageBox.Show("KHẨN CẤP: Còn dưới 3 phút!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Mốc Hết giờ (<= 0)
            if (phutConLai <= 0)
            {
                timer.Stop();
                MessageBox.Show("Hết tiền! Đang đăng xuất...", "Hết giờ");
                btnLogout_Click(null, null);
            }
        }
        private void UpdateOnce()
        {
            if (account != null && may != null && may.GiaTheoGio > 0 && txtTotalTime != null)
            {
                double totalHours = (double)(account.SoDu / may.GiaTheoGio);
                TimeSpan ts = TimeSpan.FromHours(totalHours);
                txtTotalTime.Text = string.Format("{0:D2}:{1:D2}:{2:D2}", (int)ts.TotalHours, ts.Minutes, ts.Seconds);
            }
        }

        // --- CÁC NÚT BẤM ---
        private void btnMessage_Click(object sender, EventArgs e)
        {
            formChatDangMo = new FormChatClient(this);
            formChatDangMo.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            // 1. Dừng bộ đếm giờ (Timer) để tránh lỗi ngầm
            if (timer != null)
            {
                timer.Stop();
                timer.Dispose();
            }

            // 2. Tính tiền và Lưu Database
            try
            {
                if (currentSession != null)
                {
                    // Tính thời gian và tiền giống logic ở server
                    currentSession.ThoiGianKetThuc = DateTime.Now;

                    double totalHours = (currentSession.ThoiGianKetThuc.Value - currentSession.ThoiGianBatDau).TotalHours;
                    if (totalHours < 0.01) totalHours = 0.01;

                    decimal playCost = (decimal)totalHours * (may?.GiaTheoGio ?? 0);
                    decimal serviceCost = db.SuDungDichVus.Where(s => s.MaSD == currentSession.MaSD).Sum(s => (decimal?)s.ThanhTien) ?? 0m;
                    decimal bill = playCost + serviceCost;

                    // Trừ tiền tài khoản (nếu có)
                    if (account != null)
                    {
                        var acc = db.TaiKhoanNguoiChois.Find(account.MaTK);
                        if (acc != null)
                        {
                            acc.SoDu -= bill;
                        }
                    }

                    // Lưu tổng tiền lên phiên
                    currentSession.TongTien = bill;

                    if (may != null) may.TrangThai = "Trống";
                    db.SaveChanges();
                    try { QLQuanNet.AppEvents.RaiseDataChanged(QLQuanNet.DataChangeType.AddPlayTime); } catch { }
                }
            }
            catch { }

            // 3. Ngắt mạng
            isConnected = false;
            try { heartbeatTimer?.Stop(); heartbeatTimer?.Dispose(); } catch { }
            if (client != null) client.Close();

            // 4. --- LỆNH QUAN TRỌNG NHẤT: KHỞI ĐỘNG LẠI PHẦN MỀM ---
            // Lệnh này sẽ tắt hết mọi thứ và bật lại FormLogin từ đầu
            Application.Restart();
            Environment.Exit(0);
        }

        private void btnService_Click(object sender, EventArgs e)
        {
            // Code mở form dịch vụ của bạn
            using (var f = new QLQuanNet.FormDichVu())
            {
                // Nếu đang có phiên chơi thì truyền MaSD để ghi dịch vụ đúng phiên
                if (currentSession != null) f.CurrentMaSD = currentSession.MaSD;
                f.ShowDialog();
            }
        }

        private void btnPassword_Click(object sender, EventArgs e)
        {
            // Code đổi mật khẩu cũ của bạn
            if (account == null) return;
            using (var f = new ChangePasswordForm(account))
            {
                if (f.ShowDialog() == DialogResult.OK)
                {
                    account.MatKhau = f.NewPassword;
                    db.SaveChanges();
                    MessageBox.Show("Đổi mật khẩu thành công.");
                }
            }
        }

        private void btnLock_Click(object sender, EventArgs e)
        {
            if (account == null) return;
            using (var f = new LockForm(account)) { f.ShowDialog(); }
        }

        private void FormCLI_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Ensure we perform logout actions when the form is closed (click X)
            try
            {
                // If the logout button logic hasn't already run, call it to save session
                btnLogout_Click(null, null);
            }
            catch
            {
                try { isConnected = false; if (client != null) client.Close(); } catch { }
            }
        }
    }
}