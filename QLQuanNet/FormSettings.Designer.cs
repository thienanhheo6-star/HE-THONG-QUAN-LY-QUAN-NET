using System.Drawing;
using System.Windows.Forms;

namespace QLQuanNet
{
    partial class FormSettings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpSecurity = new System.Windows.Forms.GroupBox();
            this.lblAutoLogout = new System.Windows.Forms.Label();
            this.numAutoLogoutMinutes = new System.Windows.Forms.NumericUpDown();
            this.chkAutoLogout = new System.Windows.Forms.CheckBox();
            this.grpData = new System.Windows.Forms.GroupBox();
            this.btnBackupNow = new System.Windows.Forms.Button();
            this.lblBackupInterval = new System.Windows.Forms.Label();
            this.numBackupIntervalHours = new System.Windows.Forms.NumericUpDown();
            this.chkAutoBackup = new System.Windows.Forms.CheckBox();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.grpSecurity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAutoLogoutMinutes)).BeginInit();
            this.grpData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numBackupIntervalHours)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(567, 50);
            this.panelHeader.TabIndex = 5;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(201, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CÀI ĐẶT HỆ THỐNG";
            // 
            // grpSecurity
            // 
            this.grpSecurity.Controls.Add(this.lblAutoLogout);
            this.grpSecurity.Controls.Add(this.numAutoLogoutMinutes);
            this.grpSecurity.Controls.Add(this.chkAutoLogout);
            this.grpSecurity.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpSecurity.Location = new System.Drawing.Point(20, 56);
            this.grpSecurity.Name = "grpSecurity";
            this.grpSecurity.Size = new System.Drawing.Size(535, 70);
            this.grpSecurity.TabIndex = 3;
            this.grpSecurity.TabStop = false;
            this.grpSecurity.Text = "Bảo mật & Phiên làm việc";
            // 
            // lblAutoLogout
            // 
            this.lblAutoLogout.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAutoLogout.Location = new System.Drawing.Point(290, 32);
            this.lblAutoLogout.Name = "lblAutoLogout";
            this.lblAutoLogout.Size = new System.Drawing.Size(239, 23);
            this.lblAutoLogout.TabIndex = 0;
            this.lblAutoLogout.Text = "phút (nhằm bảo mật khi vắng mặt)";
            // 
            // numAutoLogoutMinutes
            // 
            this.numAutoLogoutMinutes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numAutoLogoutMinutes.Location = new System.Drawing.Point(224, 30);
            this.numAutoLogoutMinutes.Name = "numAutoLogoutMinutes";
            this.numAutoLogoutMinutes.Size = new System.Drawing.Size(60, 27);
            this.numAutoLogoutMinutes.TabIndex = 1;
            // 
            // chkAutoLogout
            // 
            this.chkAutoLogout.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAutoLogout.Location = new System.Drawing.Point(20, 30);
            this.chkAutoLogout.Name = "chkAutoLogout";
            this.chkAutoLogout.Size = new System.Drawing.Size(176, 27);
            this.chkAutoLogout.TabIndex = 2;
            this.chkAutoLogout.Text = "Đăng xuất tự động:";
            // 
            // grpData
            // 
            this.grpData.Controls.Add(this.btnBackupNow);
            this.grpData.Controls.Add(this.lblBackupInterval);
            this.grpData.Controls.Add(this.numBackupIntervalHours);
            this.grpData.Controls.Add(this.chkAutoBackup);
            this.grpData.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.grpData.Location = new System.Drawing.Point(20, 132);
            this.grpData.Name = "grpData";
            this.grpData.Size = new System.Drawing.Size(535, 100);
            this.grpData.TabIndex = 2;
            this.grpData.TabStop = false;
            this.grpData.Text = "Dữ liệu & Sao lưu";
            // 
            // btnBackupNow
            // 
            this.btnBackupNow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(126)))), ((int)(((byte)(20)))));
            this.btnBackupNow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackupNow.ForeColor = System.Drawing.Color.White;
            this.btnBackupNow.Location = new System.Drawing.Point(20, 60);
            this.btnBackupNow.Name = "btnBackupNow";
            this.btnBackupNow.Size = new System.Drawing.Size(130, 30);
            this.btnBackupNow.TabIndex = 0;
            this.btnBackupNow.Text = "Sao lưu ngay";
            this.btnBackupNow.UseVisualStyleBackColor = false;
            this.btnBackupNow.Click += new System.EventHandler(this.btnBackupNow_Click);
            // 
            // lblBackupInterval
            // 
            this.lblBackupInterval.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblBackupInterval.Location = new System.Drawing.Point(290, 30);
            this.lblBackupInterval.Name = "lblBackupInterval";
            this.lblBackupInterval.Size = new System.Drawing.Size(94, 23);
            this.lblBackupInterval.TabIndex = 1;
            this.lblBackupInterval.Text = "giờ một lần";
            // 
            // numBackupIntervalHours
            // 
            this.numBackupIntervalHours.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.numBackupIntervalHours.Location = new System.Drawing.Point(224, 30);
            this.numBackupIntervalHours.Name = "numBackupIntervalHours";
            this.numBackupIntervalHours.Size = new System.Drawing.Size(60, 27);
            this.numBackupIntervalHours.TabIndex = 2;
            // 
            // chkAutoBackup
            // 
            this.chkAutoBackup.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.chkAutoBackup.Location = new System.Drawing.Point(20, 30);
            this.chkAutoBackup.Name = "chkAutoBackup";
            this.chkAutoBackup.Size = new System.Drawing.Size(154, 27);
            this.chkAutoBackup.TabIndex = 3;
            this.chkAutoBackup.Text = "Sao lưu tự động:";
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(360, 330);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(150, 40);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "LƯU THAY ĐỔI";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(244, 330);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 40);
            this.btnReset.TabIndex = 0;
            this.btnReset.Text = "Đặt lại";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // FormSettings
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(242)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(567, 390);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.grpData);
            this.Controls.Add(this.grpSecurity);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cấu hình hệ thống";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.grpSecurity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numAutoLogoutMinutes)).EndInit();
            this.grpData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numBackupIntervalHours)).EndInit();
            this.ResumeLayout(false);

        }

        private Panel panelHeader;
        private Label lblTitle;
        private GroupBox grpSecurity;
        private GroupBox grpData;
        private CheckBox chkAutoLogout;
        private NumericUpDown numAutoLogoutMinutes;
        private CheckBox chkAutoBackup;
        private NumericUpDown numBackupIntervalHours;
        private Button btnApply;
        private Button btnReset;
        private Button btnBackupNow;
        private Label lblAutoLogout;
        private Label lblBackupInterval;
    }
}