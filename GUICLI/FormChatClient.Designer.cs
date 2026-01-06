using System.Drawing;
using System.Windows.Forms;

namespace GUICLI
{
    partial class FormChatClient
    {
        private System.ComponentModel.IContainer components = null;

        private ListBox lstChat;
        private TextBox txtMessage;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lstChat = new System.Windows.Forms.ListBox();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnGuiTin = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstChat
            // 
            this.lstChat.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstChat.FormattingEnabled = true;
            this.lstChat.ItemHeight = 23;
            this.lstChat.Location = new System.Drawing.Point(12, 50);
            this.lstChat.Name = "lstChat";
            this.lstChat.Size = new System.Drawing.Size(476, 234);
            this.lstChat.TabIndex = 1;
            // 
            // txtMessage
            // 
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtMessage.Location = new System.Drawing.Point(12, 295);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(371, 60);
            this.txtMessage.TabIndex = 2;
            this.txtMessage.TextChanged += new System.EventHandler(this.txtMessage_TextChanged);
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(500, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "CLIENT - GỬI HỖ TRỢ";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblTitle.Click += new System.EventHandler(this.lblTitle_Click);
            // 
            // btnGuiTin
            // 
            this.btnGuiTin.Location = new System.Drawing.Point(394, 298);
            this.btnGuiTin.Name = "btnGuiTin";
            this.btnGuiTin.Size = new System.Drawing.Size(93, 56);
            this.btnGuiTin.TabIndex = 3;
            this.btnGuiTin.Text = "Gữi đi";
            this.btnGuiTin.UseVisualStyleBackColor = true;
            this.btnGuiTin.Click += new System.EventHandler(this.btnGuiTin_Click);
            // 
            // FormChatClient
            // 
            this.ClientSize = new System.Drawing.Size(500, 370);
            this.Controls.Add(this.btnGuiTin);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.lstChat);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormChatClient";
            this.Text = "Client gửi tin";
            this.Load += new System.EventHandler(this.FormChatClient_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Button btnGuiTin;
    }
}
