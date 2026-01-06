using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
// using DAL.Models; // <-- BỎ CHÚ THÍCH DÒNG NÀY NẾU CẦN DÙNG MODEL

namespace QLQuanNet
{
    public partial class FormMain : Form
    {
        private int currentUserMaTK = -1;
        private Form activeForm = null;

        // --- 1. KHAI BÁO BIẾN SERVER (PHẦN MỚI) ---
        private TcpListener server;
        private Thread listenerThread;
        private bool isRunning = false;
        public List<TcpClient> clients = new List<TcpClient>(); // Danh sách máy trạm
        // Map từ TcpClient -> MaMay (để biết máy nào đang kết nối qua socket này)
        private System.Collections.Concurrent.ConcurrentDictionary<TcpClient, int> clientMachineMap = new System.Collections.Concurrent.ConcurrentDictionary<TcpClient, int>();
        // Track last heartbeat time per client
        private System.Collections.Concurrent.ConcurrentDictionary<TcpClient, DateTime> clientLastSeen = new System.Collections.Concurrent.ConcurrentDictionary<TcpClient, DateTime>();
        private Thread janitorThread;

        // Biến giữ Form nhắn tin (để đẩy tin nhắn qua hiển thị)
        public FormNhanTin formChatDangMo = null;

        public FormMain(int maTK = -1)
        {
            InitializeComponent();
            currentUserMaTK = maTK;

            // Gán sự kiện cho các nút (GIỮ NGUYÊN)
            btnDashboard.Click += MnuDashboard_Click;
            btnMap.Click += MnuMayTinh_Click;
            btnSession.Click += MnuSuDungMay_Click;
            btnStaff.Click += MnuNhanVien_Click;
            btnReport.Click += MnuBaoCao_Click;
            btnAccount.Click += QuảnLýTàiKhoảnToolStripMenuItem_Click;
            btnSettings.Click += MnuSettings_Click;
            btnExit.Click += MnuThoat_Click;
            if (btnDV != null) btnDV.Click += btnDV_Click;

            // Mặc định hiện Dashboard
            OpenChildForm(new FormDashboard());
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false; // Cho phép can thiệp giao diện từ luồng khác
            StartServer(); // KHỞI ĐỘNG BỘ NÃO SERVER
        }

        // --- 2. HÀM KHỞI ĐỘNG SERVER (PHẦN MỚI) ---
        private void StartServer()
        {
            isRunning = true;
            listenerThread = new Thread(() =>
            {
                try
                {
                    server = new TcpListener(IPAddress.Any, 9999);
                    server.Start();
                    isRunning = true;
                    while (isRunning)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        clients.Add(client);
                        // initialize last-seen for heartbeat
                        clientLastSeen[client] = DateTime.Now;

                        // Tạo luồng riêng xử lý từng máy trạm
                        Thread t = new Thread(() => HandleClient(client));
                        t.IsBackground = true;
                        t.Start();
                    }
                }
                catch { }
            });
            listenerThread.IsBackground = true;
            listenerThread.Start();

            // Start janitor thread to detect dead clients (no heartbeat)
            janitorThread = new Thread(() => {
                int thresholdSeconds = 15; // consider dead after 15s without ping
                while (isRunning)
                {
                    try
                    {
                        var now = DateTime.Now;
                        foreach (var c in clients.ToList())
                        {
                            try
                            {
                                if (clientLastSeen.TryGetValue(c, out DateTime last))
                                {
                                    if ((now - last).TotalSeconds > thresholdSeconds)
                                    {
                                        try { c.Close(); } catch { }
                                    }
                                }
                            }
                            catch { }
                        }
                    }
                    catch { }
                    Thread.Sleep(5000);
                }
            }) { IsBackground = true };
            janitorThread.Start();
        }

        // --- 3. BỘ NÃO XỬ LÝ LOGIC (PHẦN MỚI - QUAN TRỌNG) ---
        private void HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            try
            {
                while (isRunning && client.Connected)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    string[] parts = line.Split('|');
                    string command = parts[0];

                    // Heartbeat from client
                    if (command == "PING")
                    {
                        try
                        {
                            clientLastSeen[client] = DateTime.Now;
                        }
                        catch { }
                        continue;
                    }

                    // === XỬ LÝ ĐĂNG NHẬP & CHẶN MÁY ===
                    if (command == "USER_LOGIN")
                    {
                        // Cấu trúc nhận: USER_LOGIN|User|Pass|SoMay
                        string username = parts[1];
                        string password = parts[2];
                        int soMay = 0;
                        if (parts.Length > 3) int.TryParse(parts[3], out soMay);

                        XuLyDangNhap(client, writer, username, password, soMay);
                    }

                    // === XỬ LÝ CHAT ===
                    else if (command == "CHAT")
                    {
                        string content = parts.Length > 1 ? parts[1] : "";

                        // Bắt buộc dùng Invoke vì đây là luồng riêng, muốn mở Form phải nhờ luồng chính
                        this.Invoke(new Action(() => {

                            // 1. KIỂM TRA: Nếu Form chưa mở hoặc đã bị tắt
                            if (formChatDangMo == null || formChatDangMo.IsDisposed)
                            {
                                // TỰ ĐỘNG BẬT FORM LÊN
                                formChatDangMo = new FormNhanTin(this);
                                formChatDangMo.Show();
                            }

                            // 2. Đảm bảo Form hiện lên trên cùng (nếu đang bị che hoặc thu nhỏ)
                            if (formChatDangMo.WindowState == FormWindowState.Minimized)
                            {
                                formChatDangMo.WindowState = FormWindowState.Normal;
                            }
                            formChatDangMo.BringToFront(); // Lôi lên trên cùng

                            // 3. Hiển thị nội dung tin nhắn vào ô chat
                            // (Thêm tiền tố "Client:" để dễ phân biệt)
                            formChatDangMo.HienThiTinNhan(content);

                            // (Tùy chọn) Có thể bật thêm tiếng kêu "Ting" để Admin chú ý
                            System.Media.SystemSounds.Exclamation.Play();
                        }));
                    }

                    // === XỬ LÝ KHI CLIENT THÔNG BÁO ĐÃ SỬ DỤNG DỊCH VỤ ===
                    else if (command == "SERVICE_USED")
                    {
                        // Cú pháp: SERVICE_USED|MaSD
                        // Khi nhận, server cập nhật giao diện dashboard
                        try
                        {
                            this.Invoke(new Action(() => {
                                try { AppEvents.RaiseDataChanged(DataChangeType.ServiceUsed); } catch { }
                            }));
                        }
                        catch { }
                    }
                }
            }
            catch (Exception ex)
            {
                // Suppress expected socket interruption errors that occur when
                // the server is stopping (WSACancelBlockingCall / SocketError.Interrupted).
                try
                {
                    var baseEx = ex.GetBaseException();
                    bool isSocketInterrupted = false;
                    if (baseEx is System.Net.Sockets.SocketException sockEx)
                    {
                        if (sockEx.SocketErrorCode == System.Net.Sockets.SocketError.Interrupted)
                            isSocketInterrupted = true;
                    }
                    else if (baseEx.Message != null && baseEx.Message.IndexOf("WSACancelBlockingCall", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        isSocketInterrupted = true;
                    }

                    if (isSocketInterrupted || !isRunning)
                    {
                        // expected during shutdown; ignore
                    }
                    else
                    {
                        MessageBox.Show("Lỗi Server: " + baseEx.Message);
                    }
                }
                catch { }
            }
            finally
            {
                // Nếu socket này từng được gán máy, thì kết thúc phiên trong DB
                try
                {
                    if (clientMachineMap.TryRemove(client, out int maMay))
                    {
                        try
                        {
                            using (var db = new DAL.Models.Model1())
                            {
                                var phien = db.SuDungMays.FirstOrDefault(s => s.MaMay == maMay && s.ThoiGianKetThuc == null);
                                if (phien != null)
                                {
                                    phien.ThoiGianKetThuc = DateTime.Now;

                                    double totalHours = (phien.ThoiGianKetThuc.Value - phien.ThoiGianBatDau).TotalHours;
                                    if (totalHours < 0.01) totalHours = 0.01;

                                    var may = db.MAYTINHs.Find(phien.MaMay);
                                    decimal playCost = (decimal)totalHours * (may?.GiaTheoGio ?? 0);
                                    decimal serviceCost = db.SuDungDichVus.Where(x => x.MaSD == phien.MaSD).Sum(x => (decimal?)x.ThanhTien) ?? 0m;
                                    decimal bill = playCost + serviceCost;

                                    var acc = db.TaiKhoanNguoiChois.Find(phien.MaTK);
                                    if (acc != null) acc.SoDu -= bill;

                                    phien.TongTien = bill;
                                    if (may != null) may.TrangThai = "Trống";

                                    db.SaveChanges();
                                    try { AppEvents.RaiseDataChanged(DataChangeType.AddPlayTime); } catch { }
                                }
                            }
                        }
                        catch { }
                    }
                }
                catch { }
                // remove last-seen entry
                try { clientLastSeen.TryRemove(client, out _); } catch { }

                clients.Remove(client);
                try { client.Close(); } catch { }
            }
        }

        // --- 4. HÀM LOGIN & GHI DATABASE (PHẦN MỚI) ---
        private void XuLyDangNhap(TcpClient client, StreamWriter writer, string user, string pass, int maMay)
        {
            try
            {
                using (var db = new DAL.Models.Model1())
                {
                    int maMayChinhThuc = maMay;
                    // 1. KIỂM TRA: MÁY YÊU CẦU CÓ BẬN KHÔNG?
                    var mayBan = db.SuDungMays.FirstOrDefault(s => s.MaMay == maMay && s.ThoiGianKetThuc == null);

                    if (mayBan != null)
                    {
                        // ==> NẾU BẬN: TỰ ĐỘNG TÌM MÁY KHÁC ĐANG TRỐNG
                        // Tìm máy đầu tiên có trạng thái "Trống" hoặc không có trong bảng SuDungMay
                        var mayTrong = db.MAYTINHs.FirstOrDefault(m => m.TrangThai == "Trống");

                        if (mayTrong != null)
                        {
                            maMayChinhThuc = mayTrong.MaMay; // Đổi sang máy trống vừa tìm được
                        }
                        else
                        {
                            // Nếu tìm hết cả quán mà không còn máy nào trống
                            writer.WriteLine("LOGIN_FAIL|Hết máy trống! Vui lòng quay lại sau.");
                            return;
                        }
                    }

                    // 2. Kiểm tra tài khoản có đang treo nick không?
                    var accTreo = db.SuDungMays.FirstOrDefault(s => s.TaiKhoanNguoiChoi.TenDangNhap == user && s.ThoiGianKetThuc == null);
                    if (accTreo != null)
                    {
                        writer.WriteLine($"LOGIN_FAIL|Tài khoản đang chơi ở máy {accTreo.MaMay}!");
                        return;
                    }

                    // 3. Kiểm tra mật khẩu
                    var taiKhoan = db.TaiKhoanNguoiChois.FirstOrDefault(u => u.TenDangNhap == user && u.MatKhau == pass);
                    if (taiKhoan != null)
                    {
                        // Validate that the chosen machine exists (prevent FK insert errors)
                        var mayTinh = db.MAYTINHs.Find(maMayChinhThuc);
                        if (mayTinh == null)
                        {
                            // Try to fallback to any available "Trống" machine
                            var fallback = db.MAYTINHs.FirstOrDefault(m => m.TrangThai == "Trống");
                            if (fallback != null)
                            {
                                maMayChinhThuc = fallback.MaMay;
                                mayTinh = fallback;
                            }
                            else
                            {
                                writer.WriteLine("LOGIN_FAIL|Máy không tồn tại hoặc không còn máy trống.");
                                return;
                            }
                        }

                        // A. Ghi vào Sổ (Dùng maMayChinhThuc)
                        DAL.Models.SuDungMay phienMoi = new DAL.Models.SuDungMay();
                        phienMoi.MaTK = taiKhoan.MaTK;
                        phienMoi.MaMay = maMayChinhThuc; // <--- Dùng ID máy mới
                        phienMoi.ThoiGianBatDau = DateTime.Now;
                        phienMoi.TongTien = 0;
                        db.SuDungMays.Add(phienMoi);

                        // B. Đổi trạng thái máy
                        if (mayTinh != null) mayTinh.TrangThai = "Đang sử dụng";

                        db.SaveChanges();

                        // C. GỬI PHẢN HỒI KÈM SỐ MÁY ĐƯỢC CẤP
                        // Server báo: "OK, tôi cấp cho bạn máy số [maMayChinhThuc]"
                        writer.WriteLine($"LOGIN_SUCCESS|{maMayChinhThuc}");

                        // D. Gán mapping TcpClient -> MaMay để dọn dẹp khi socket rớt
                        try { clientMachineMap[client] = maMayChinhThuc; } catch { }

                        // D. Update giao diện Server
                        this.Invoke(new Action(() => { LoadDanhSachMay(); }));
                    }
                    else
                    {
                        writer.WriteLine("LOGIN_FAIL|Sai mật khẩu!");
                    }
                }
            }
            catch (Exception ex)
            {
                writer.WriteLine("LOGIN_FAIL|Lỗi Server: " + ex.GetBaseException().Message);
            }
        }
        public void LoadDanhSachMay()
        {

        }




        // --- 5. HÀM GỬI TIN NHẮN (CHO FORM CHAT DÙNG KÉ) ---
        public void GuiTinNhanChoTatCa(string tinNhan)
        {
            foreach (var client in clients)
            {
                try
                {
                    if (client.Connected)
                    {
                        StreamWriter w = new StreamWriter(client.GetStream()) { AutoFlush = true };
                        w.WriteLine("CHAT|" + tinNhan);
                    }
                }
                catch { }
            }
        }

        // --- CÁC HÀM XỬ LÝ GIAO DIỆN CŨ (GIỮ NGUYÊN) ---
        private void OpenChildForm(Form childForm)
        {
            activeForm?.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(childForm);
            pnlContent.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
        }

        // Nút mở Chat: Sửa lại để truyền FormMain sang
        private void MnuNhanTin_Click(object sender, EventArgs e)
        {
            if (formChatDangMo == null || formChatDangMo.IsDisposed)
            {
                formChatDangMo = new FormNhanTin(this); // <-- TRUYỀN 'this' SANG
                formChatDangMo.Show();
            }
            else
            {
                formChatDangMo.BringToFront();
                if (formChatDangMo.WindowState == FormWindowState.Minimized)
                    formChatDangMo.WindowState = FormWindowState.Normal;
            }
        }

        private void MnuDashboard_Click(object sender, EventArgs e) => OpenChildForm(new FormDashboard());
        private void MnuMayTinh_Click(object sender, EventArgs e) => OpenChildForm(new FormMayTinh());
        private void MnuSuDungMay_Click(object sender, EventArgs e) => OpenChildForm(new FormSuDungMay());
        private void MnuNhanVien_Click(object sender, EventArgs e) => OpenChildForm(new FormQuanLyNhanVien());
        private void MnuBaoCao_Click(object sender, EventArgs e) => OpenChildForm(new FormBaoCaoDoanhThu());
        private void QuảnLýTàiKhoảnToolStripMenuItem_Click(object sender, EventArgs e) => OpenChildForm(new FormTaiKhoan());
        private void MnuSettings_Click(object sender, EventArgs e) => OpenChildForm(new FormSettings());
        private void btnDV_Click(object sender, EventArgs e) => OpenChildForm(new Quanlidichvu());

        private void MnuThoat_Click(object sender, EventArgs e)
        {
            // 1. Đóng các phiên đang mở trong DB (tính tiền, trả máy)
            try
            {
                using (var db = new DAL.Models.Model1())
                {
                    var opens = db.SuDungMays.Where(s => s.ThoiGianKetThuc == null).ToList();
                    foreach (var s in opens)
                    {
                        var m = db.MAYTINHs.Find(s.MaMay);
                        var services = db.SuDungDichVus.Where(x => x.MaSD == s.MaSD).ToList();
                        s.ThoiGianKetThuc = DateTime.Now;
                        double totalHours = (s.ThoiGianKetThuc.Value - s.ThoiGianBatDau).TotalHours;
                        if (totalHours < 0.01) totalHours = 0.01;
                        decimal playCost = (decimal)totalHours * (m?.GiaTheoGio ?? 0);
                        decimal serviceCost = services.Sum(x => (decimal?)x.ThanhTien) ?? 0m;
                        decimal bill = playCost + serviceCost;

                        var acc = db.TaiKhoanNguoiChois.Find(s.MaTK);
                        if (acc != null) acc.SoDu -= bill;

                        s.TongTien = bill;
                        if (m != null) m.TrangThai = "Trống";

                        // Gửi lệnh đá máy tới client tương ứng (nếu đang kết nối)
                        try { ForceLogoutClient(s.MaMay.ToString()); } catch { }
                    }
                    if (opens.Count > 0) db.SaveChanges();
                }
            }
            catch { }

            // 2. Dừng server và đóng các kết nối
            try
            {
                isRunning = false;
                try { server?.Stop(); } catch { }
                foreach (var c in clients.ToList())
                {
                    try { c.Close(); } catch { }
                }
                clients.Clear();
            }
            catch { }

            // 3. Đóng FormMain
            this.Close();
        }
        public void ForceLogoutClient(string maMayCanDa)
        {
            // Duyệt qua tất cả các máy đang kết nối
            foreach (var client in clients)
            {
                try
                {
                    if (client.Connected)
                    {
                        System.IO.StreamWriter w = new System.IO.StreamWriter(client.GetStream()) { AutoFlush = true };

                        // --- DÒNG QUAN TRỌNG NHẤT ---
                        // Phải gửi trần trụi lệnh này, không được thêm chữ CHAT| vào đằng trước
                        w.WriteLine($"FORCE_LOGOUT|{maMayCanDa}");
                    }
                }
                catch { }
            }
        }


        // Các hàm sự kiện thừa (để trống)
        private void pnlTopBar_Paint(object sender, PaintEventArgs e) { }
        private void btnSession_Click(object sender, EventArgs e) { }
        private void lblLogo_Click(object sender, EventArgs e) { }
        private void pnlContent_Paint(object sender, PaintEventArgs e) { }
    }
}