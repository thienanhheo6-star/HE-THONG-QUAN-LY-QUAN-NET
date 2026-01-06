using DAL.Models;
using GUICLI;
using System;
using System.Linq;
using System.Windows.Forms;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace QLQuanNet
{
    public partial class FormLogin : Form
    {
        int ID_MAY_TRAM = 1;
        private Model1 db = new Model1();
        TcpClient client;
        StreamReader reader;
        StreamWriter writer;
        Thread receiveThread;
        bool isConnected = false;
        public FormLogin()
        {
            InitializeComponent();
            this.Text = "Client - Máy số " + ID_MAY_TRAM;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim(); // (Nếu bạn dùng txtPassword thì sửa lại tên)

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Vui lòng nhập tài khoản và mật khẩu!");
                return;
            }

            try
            {
                if (isConnected && writer != null)
                {
                    // Gửi lệnh: USER_LOGIN | User | Pass | Số Máy
                    string lenhGui = $"USER_LOGIN|{user}|{pass}|{ID_MAY_TRAM}";
                    writer.WriteLine(lenhGui);

                    // Khóa nút lại chờ phản hồi
                    btnLogin.Enabled = false;
                    this.Text = "Đang kiểm tra...";
                }
                else
                {
                    // Nếu mất mạng thì thử kết nối lại
                    ConnectToServer();
                    MessageBox.Show("Mất kết nối! Vui lòng bấm Đăng nhập lại.");
                }
            }
            catch
            {
                MessageBox.Show("Lỗi gửi dữ liệu!");
            }
        }
        private void ReceiveMessage()
        {
            try
            {
                while (isConnected)
                {
                    string line = reader.ReadLine();
                    if (line == null) break;

                    string[] parts = line.Split('|');
                    string command = parts[0];
                    string content = parts.Length > 1 ? parts[1] : "";

                    // === A. ĐĂNG NHẬP THÀNH CÔNG ===
                    if (command == "LOGIN_SUCCESS")
                    {
                        // Server gửi về: LOGIN_SUCCESS | [Số Máy]
                        // Ví dụ: LOGIN_SUCCESS|2

                        // 1. Lấy số máy Server cấp cho
                        string soMayServerCap = "1"; // Giá trị mặc định
                        if (parts.Length > 1)
                        {
                            soMayServerCap = parts[1];
                        }

                        // 2. Dừng việc lắng nghe ở FormLogin lại
                        isConnected = false;

                        this.Invoke(new Action(() => {
                            this.Hide();

                            // 3. --- SỬA DÒNG NÀY (Thêm biến soMayServerCap vào cuối) ---
                            FormCLI f = new FormCLI(this.client, txtUser.Text, soMayServerCap);
                            f.Show();
                        }));

                        break;
                    }

                    // === B. ĐĂNG NHẬP THẤT BẠI ===
                    else if (command == "LOGIN_FAIL")
                    {
                        this.Invoke(new Action(() => {
                            MessageBox.Show(content, "Lỗi đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            btnLogin.Enabled = true;
                            this.Text = "Đăng nhập - Máy số " + ID_MAY_TRAM;
                            // Reset mật khẩu
                            if (txtPass != null) { txtPass.Clear(); txtPass.Focus(); }
                        }));
                    }
                }
            }
            catch
            {
                // Nếu lỗi thì thôi, không làm gì cả (hoặc hiện thông báo mất mạng)
                isConnected = false;
                this.Invoke(new Action(() => {
                    btnLogin.Enabled = true;
                    this.Text = "Mất kết nối Server";
                }));
            }
        }
        private void ConnectToServer()
        {
            try
            {
                // Nếu chưa kết nối thì mới kết nối
                if (client == null || !client.Connected)
                {
                    client = new TcpClient();
                    // Lưu ý: Thay "127.0.0.1" bằng IP máy Server nếu chạy khác máy
                    client.Connect("127.0.0.1", 9999);

                    NetworkStream stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream) { AutoFlush = true };

                    isConnected = true;

                    // Bắt đầu lắng nghe phản hồi từ Server
                    receiveThread = new Thread(ReceiveMessage);
                    receiveThread.IsBackground = true;
                    receiveThread.Start();
                }
            }
            catch (Exception ex)
            {
                isConnected = false;
                MessageBox.Show("Không tìm thấy Server! Hãy bật FormMain trước.\nLỗi: " + ex.Message);
            }
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            ConnectToServer();
            // Khi client khởi động, kiểm tra và dọn dẹp các phiên "treo" do ứng dụng bị kill (Task Manager)
            try
            {
                CleanupOrphanSessions(ID_MAY_TRAM);
            }
            catch { }
        }

        private void CleanupOrphanSessions(int machineId)
        {
            try
            {
                var orphan = db.SuDungMays.Where(s => s.MaMay == machineId && s.ThoiGianKetThuc == null).ToList();
                foreach (var s in orphan)
                {
                    var acc = db.TaiKhoanNguoiChois.Find(s.MaTK);
                    var m = db.MAYTINHs.Find(s.MaMay);
                    s.ThoiGianKetThuc = DateTime.Now;

                    double totalHours = (s.ThoiGianKetThuc.Value - s.ThoiGianBatDau).TotalHours;
                    if (totalHours < 0.01) totalHours = 0.01;
                    decimal playCost = (decimal)totalHours * (m?.GiaTheoGio ?? 0);
                    decimal serviceCost = db.SuDungDichVus.Where(x => x.MaSD == s.MaSD).Sum(x => (decimal?)x.ThanhTien) ?? 0m;
                    decimal bill = playCost + serviceCost;

                    if (acc != null) acc.SoDu -= bill;
                    s.TongTien = bill;
                    if (m != null) m.TrangThai = "Trống";
                }
                if (orphan.Count > 0) db.SaveChanges();
            }
            catch { }
        }
    }
}

