using System.Drawing;
using System.Windows.Forms;

namespace QLQuanNet
{
    partial class FormNhanTin
    {
        private System.ComponentModel.IContainer components = null;

        private ListBox lstMessages;
        private TextBox txtReply;
        private Button btnReply;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lstMessages = new System.Windows.Forms.ListBox();
            this.txtReply = new System.Windows.Forms.TextBox();
            this.btnReply = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstMessages
            // 
            this.lstMessages.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstMessages.FormattingEnabled = true;
            this.lstMessages.ItemHeight = 23;
            this.lstMessages.Location = new System.Drawing.Point(12, 52);
            this.lstMessages.Name = "lstMessages";
            this.lstMessages.Size = new System.Drawing.Size(576, 280);
            this.lstMessages.TabIndex = 1;
            // 
            // txtReply
            // 
            this.txtReply.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txtReply.Location = new System.Drawing.Point(12, 345);
            this.txtReply.Multiline = true;
            this.txtReply.Name = "txtReply";
            this.txtReply.Size = new System.Drawing.Size(460, 60);
            this.txtReply.TabIndex = 2;
            this.txtReply.TextChanged += new System.EventHandler(this.txtReply_TextChanged);
            // 
            // btnReply
            // 
            this.btnReply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.btnReply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReply.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnReply.ForeColor = System.Drawing.Color.White;
            this.btnReply.Location = new System.Drawing.Point(480, 345);
            this.btnReply.Name = "btnReply";
            this.btnReply.Size = new System.Drawing.Size(108, 60);
            this.btnReply.TabIndex = 3;
            this.btnReply.Text = "Trả lời";
            this.btnReply.UseVisualStyleBackColor = false;
            this.btnReply.Click += new System.EventHandler(this.BtnReply_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(600, 40);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "TIN NHẮN TỪ CLIENT";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormNhanTin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 420);
            this.Controls.Add(this.btnReply);
            this.Controls.Add(this.txtReply);
            this.Controls.Add(this.lstMessages);
            this.Controls.Add(this.lblTitle);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormNhanTin";
            this.Text = "Nhận & Trả lời tin nhắn";
            this.Load += new System.EventHandler(this.FormNhanTin_Load);
            this.Shown += new System.EventHandler(this.FormNhanTin_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
