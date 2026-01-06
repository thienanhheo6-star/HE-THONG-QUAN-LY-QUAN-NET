namespace GUICLI
{
    partial class FormCLI
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblUser = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.lblTotal = new System.Windows.Forms.Label();
            this.txtTotalTime = new System.Windows.Forms.TextBox();
            this.lblUsed = new System.Windows.Forms.Label();
            this.txtUsedTime = new System.Windows.Forms.TextBox();
            this.lblRemain = new System.Windows.Forms.Label();
            this.txtRemainTime = new System.Windows.Forms.TextBox();
            this.lblPlayCost = new System.Windows.Forms.Label();
            this.txtPlayCost = new System.Windows.Forms.TextBox();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnMessage = new System.Windows.Forms.Button();
            this.btnService = new System.Windows.Forms.Button();
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnPassword = new System.Windows.Forms.Button();
            this.btnLock = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlInfo.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(45)))));
            this.pnlHeader.Controls.Add(this.lblUser);
            this.pnlHeader.Controls.Add(this.lblStatus);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(300, 40);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblUser
            // 
            this.lblUser.ForeColor = System.Drawing.Color.White;
            this.lblUser.Location = new System.Drawing.Point(10, 10);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(100, 23);
            this.lblUser.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.ForeColor = System.Drawing.Color.Lime;
            this.lblStatus.Location = new System.Drawing.Point(200, 10);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(100, 23);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "Đã kết nối";
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.lblTotal);
            this.pnlInfo.Controls.Add(this.txtTotalTime);
            this.pnlInfo.Controls.Add(this.lblUsed);
            this.pnlInfo.Controls.Add(this.txtUsedTime);
            this.pnlInfo.Controls.Add(this.lblRemain);
            this.pnlInfo.Controls.Add(this.txtRemainTime);
            this.pnlInfo.Controls.Add(this.lblPlayCost);
            this.pnlInfo.Controls.Add(this.txtPlayCost);
            this.pnlInfo.Location = new System.Drawing.Point(10, 50);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(280, 180);
            this.pnlInfo.TabIndex = 1;
            // 
            // lblTotal
            // 
            this.lblTotal.Location = new System.Drawing.Point(6, 3);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(100, 23);
            this.lblTotal.TabIndex = 0;
            this.lblTotal.Text = "Tổng thời gian:";
            // 
            // txtTotalTime
            // 
            this.txtTotalTime.Location = new System.Drawing.Point(130, 0);
            this.txtTotalTime.Name = "txtTotalTime";
            this.txtTotalTime.ReadOnly = true;
            this.txtTotalTime.Size = new System.Drawing.Size(100, 22);
            this.txtTotalTime.TabIndex = 1;
            // 
            // lblUsed
            // 
            this.lblUsed.Location = new System.Drawing.Point(6, 34);
            this.lblUsed.Name = "lblUsed";
            this.lblUsed.Size = new System.Drawing.Size(124, 23);
            this.lblUsed.TabIndex = 2;
            this.lblUsed.Text = "Thời gian sử dụng:";
            // 
            // txtUsedTime
            // 
            this.txtUsedTime.Location = new System.Drawing.Point(130, 30);
            this.txtUsedTime.Name = "txtUsedTime";
            this.txtUsedTime.ReadOnly = true;
            this.txtUsedTime.Size = new System.Drawing.Size(100, 22);
            this.txtUsedTime.TabIndex = 3;
            // 
            // lblRemain
            // 
            this.lblRemain.Location = new System.Drawing.Point(3, 63);
            this.lblRemain.Name = "lblRemain";
            this.lblRemain.Size = new System.Drawing.Size(100, 23);
            this.lblRemain.TabIndex = 4;
            this.lblRemain.Text = "Thời gian còn lại:";
            // 
            // txtRemainTime
            // 
            this.txtRemainTime.Location = new System.Drawing.Point(130, 60);
            this.txtRemainTime.Name = "txtRemainTime";
            this.txtRemainTime.ReadOnly = true;
            this.txtRemainTime.Size = new System.Drawing.Size(100, 22);
            this.txtRemainTime.TabIndex = 5;
            // 
            // lblPlayCost
            // 
            this.lblPlayCost.Location = new System.Drawing.Point(3, 93);
            this.lblPlayCost.Name = "lblPlayCost";
            this.lblPlayCost.Size = new System.Drawing.Size(100, 23);
            this.lblPlayCost.TabIndex = 6;
            this.lblPlayCost.Text = "Chi phí giờ chơi:";
            // 
            // txtPlayCost
            // 
            this.txtPlayCost.Location = new System.Drawing.Point(130, 90);
            this.txtPlayCost.Name = "txtPlayCost";
            this.txtPlayCost.ReadOnly = true;
            this.txtPlayCost.Size = new System.Drawing.Size(100, 22);
            this.txtPlayCost.TabIndex = 7;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnMessage);
            this.pnlButtons.Controls.Add(this.btnService);
            this.pnlButtons.Controls.Add(this.btnLogout);
            this.pnlButtons.Controls.Add(this.btnPassword);
            this.pnlButtons.Controls.Add(this.btnLock);
            this.pnlButtons.Location = new System.Drawing.Point(10, 250);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(280, 200);
            this.pnlButtons.TabIndex = 2;
            // 
            // btnMessage
            // 
            this.btnMessage.Location = new System.Drawing.Point(0, 0);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(130, 40);
            this.btnMessage.TabIndex = 0;
            this.btnMessage.Text = "Tin nhắn";
            this.btnMessage.Click += new System.EventHandler(this.btnMessage_Click);
            // 
            // btnService
            // 
            this.btnService.Location = new System.Drawing.Point(140, 0);
            this.btnService.Name = "btnService";
            this.btnService.Size = new System.Drawing.Size(130, 40);
            this.btnService.TabIndex = 1;
            this.btnService.Text = "Dịch vụ";
            this.btnService.Click += new System.EventHandler(this.btnService_Click);
            // 
            // btnLogout
            // 
            this.btnLogout.Location = new System.Drawing.Point(0, 50);
            this.btnLogout.Name = "btnLogout";
            this.btnLogout.Size = new System.Drawing.Size(130, 40);
            this.btnLogout.TabIndex = 2;
            this.btnLogout.Text = "Đăng xuất";
            this.btnLogout.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnPassword
            // 
            this.btnPassword.Location = new System.Drawing.Point(140, 50);
            this.btnPassword.Name = "btnPassword";
            this.btnPassword.Size = new System.Drawing.Size(130, 40);
            this.btnPassword.TabIndex = 3;
            this.btnPassword.Text = "Mật khẩu";
            this.btnPassword.Click += new System.EventHandler(this.btnPassword_Click);
            // 
            // btnLock
            // 
            this.btnLock.Location = new System.Drawing.Point(0, 96);
            this.btnLock.Name = "btnLock";
            this.btnLock.Size = new System.Drawing.Size(130, 40);
            this.btnLock.TabIndex = 4;
            this.btnLock.Text = "Khóa máy";
            this.btnLock.Click += new System.EventHandler(this.btnLock_Click);
            // 
            // FormCLI
            // 
            this.ClientSize = new System.Drawing.Size(300, 500);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.pnlButtons);
            this.Name = "FormCLI";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Client Net";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Label lblStatus;

        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblUsed;
        private System.Windows.Forms.Label lblRemain;
        private System.Windows.Forms.Label lblPlayCost;

        private System.Windows.Forms.TextBox txtTotalTime;
        private System.Windows.Forms.TextBox txtUsedTime;
        private System.Windows.Forms.TextBox txtRemainTime;
        private System.Windows.Forms.TextBox txtPlayCost;

        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnMessage;
        private System.Windows.Forms.Button btnService;
        private System.Windows.Forms.Button btnLogout;
        private System.Windows.Forms.Button btnPassword;
        private System.Windows.Forms.Button btnLock;
    }
}
