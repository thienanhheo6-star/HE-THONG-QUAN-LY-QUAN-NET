namespace QLQuanNet
{
    partial class UC_MonAn
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picHinhAnh = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblGia = new System.Windows.Forms.Label();
            this.lblTenMon = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHinhAnh)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // picHinhAnh
            // 
            this.picHinhAnh.Location = new System.Drawing.Point(0, 0);
            this.picHinhAnh.Name = "picHinhAnh";
            this.picHinhAnh.Size = new System.Drawing.Size(244, 215);
            this.picHinhAnh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHinhAnh.TabIndex = 0;
            this.picHinhAnh.TabStop = false;
            this.picHinhAnh.Click += new System.EventHandler(this.picHinhAnh_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Controls.Add(this.lblGia);
            this.panel1.Location = new System.Drawing.Point(168, 21);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(62, 38);
            this.panel1.TabIndex = 1;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // lblGia
            // 
            this.lblGia.AutoSize = true;
            this.lblGia.ForeColor = System.Drawing.Color.White;
            this.lblGia.Location = new System.Drawing.Point(11, 12);
            this.lblGia.Name = "lblGia";
            this.lblGia.Size = new System.Drawing.Size(42, 16);
            this.lblGia.TabIndex = 2;
            this.lblGia.Text = "25000";
            this.lblGia.Click += new System.EventHandler(this.lblGia_Click);
            // 
            // lblTenMon
            // 
            this.lblTenMon.AutoSize = true;
            this.lblTenMon.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.lblTenMon.ForeColor = System.Drawing.Color.White;
            this.lblTenMon.Location = new System.Drawing.Point(20, 33);
            this.lblTenMon.Name = "lblTenMon";
            this.lblTenMon.Size = new System.Drawing.Size(60, 16);
            this.lblTenMon.TabIndex = 2;
            this.lblTenMon.Text = "Tên món";
            this.lblTenMon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // UC_MonAn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTenMon);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.picHinhAnh);
            this.Name = "UC_MonAn";
            this.Size = new System.Drawing.Size(244, 215);
            this.Load += new System.EventHandler(this.UC_MonAn_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picHinhAnh)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picHinhAnh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblGia;
        private System.Windows.Forms.Label lblTenMon;
    }
}
