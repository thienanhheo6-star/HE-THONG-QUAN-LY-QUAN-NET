using BUS;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace QLQuanNet
{
    public partial class FormSettings : Form
    {
        public static bool DarkModeEnabled { get; set; } = false;

        public FormSettings()
        {
            InitializeComponent();
        }

        private void FormSettings_Load(object sender, EventArgs e)
        {
            // Load settings (from AppSettings if available)
            BUS.AppSettings.Load();
            chkAutoLogout.Checked = BUS.AppSettings.AutoLogoutEnabled;
            numAutoLogoutMinutes.Value = BUS.AppSettings.AutoLogoutMinutes > 0 ? BUS.AppSettings.AutoLogoutMinutes : 30;
            chkAutoBackup.Checked = BUS.AppSettings.AutoBackupEnabled;
            numBackupIntervalHours.Value = BUS.AppSettings.BackupIntervalHours > 0 ? BUS.AppSettings.BackupIntervalHours : 24;
            DarkModeEnabled = BUS.AppSettings.DarkMode;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // 1. Dark mode
            AppSettings.DarkMode = DarkModeEnabled;

            // 2. Auto logout
            AppSettings.AutoLogoutEnabled = chkAutoLogout.Checked;
            AppSettings.AutoLogoutMinutes = (int)numAutoLogoutMinutes.Value;

            // 3. Auto backup
            AppSettings.AutoBackupEnabled = chkAutoBackup.Checked;
            AppSettings.BackupIntervalHours = (int)numBackupIntervalHours.Value;

            // Lưu trạng thái vào file cấu hình
            BUS.AppSettings.DarkMode = AppSettings.DarkMode;
            BUS.AppSettings.AutoLogoutEnabled = AppSettings.AutoLogoutEnabled;
            BUS.AppSettings.AutoLogoutMinutes = AppSettings.AutoLogoutMinutes;
            BUS.AppSettings.AutoBackupEnabled = AppSettings.AutoBackupEnabled;
            BUS.AppSettings.BackupIntervalHours = AppSettings.BackupIntervalHours;
            BUS.AppSettings.Save();

            MessageBox.Show(
                "Cài đặt đã được áp dụng thành công!",
                "Thông báo",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DarkModeEnabled = false;

            chkAutoLogout.Checked = false;
            numAutoLogoutMinutes.Value = 30;

            chkAutoBackup.Checked = false;
            numBackupIntervalHours.Value = 24;

            // Khôi phục các giá trị đã lưu trước đó (nếu có)
            BUS.AppSettings.Load();
            chkAutoLogout.Checked = BUS.AppSettings.AutoLogoutEnabled;
            numAutoLogoutMinutes.Value = BUS.AppSettings.AutoLogoutMinutes > 0 ? BUS.AppSettings.AutoLogoutMinutes : 30;
            chkAutoBackup.Checked = BUS.AppSettings.AutoBackupEnabled;
            numBackupIntervalHours.Value = BUS.AppSettings.BackupIntervalHours > 0 ? BUS.AppSettings.BackupIntervalHours : 24;
            DarkModeEnabled = BUS.AppSettings.DarkMode;
            MessageBox.Show("Cài đặt đã được khôi phục từ cấu hình đã lưu.");
        }
        private void ApplyThemeToControls(Control.ControlCollection controls, bool isDark)
        {
            foreach (Control c in controls)
            {
                if (isDark)
                {
                    c.BackColor = Color.FromArgb(45, 45, 48);
                    c.ForeColor = Color.White;
                }
                else
                {
                    c.BackColor = Color.White;
                    c.ForeColor = Color.Black;
                }

                if (c.HasChildren)
                    ApplyThemeToControls(c.Controls, isDark);
            }
        }

        private void ApplyDarkModeToAllForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                form.BackColor = Color.FromArgb(45, 45, 48);
                form.ForeColor = Color.White;
                ApplyThemeToControls(form.Controls, true);
            }
        }
        private void ApplyLightModeToAllForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                form.BackColor = Color.White;
                form.ForeColor = Color.Black;
                ApplyThemeToControls(form.Controls, false);
            }
        }

        private void btnBackupNow_Click(object sender, EventArgs e)
        {
            try
            {
                BackupService.CreateBackup(null); // ✅ QUAN TRỌNG
                MessageBox.Show("Sao lưu dữ liệu thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi sao lưu: " + ex.Message);
            }
        }


    }
}
