using System.Windows.Forms;

namespace QLQuanNet
{
    partial class FormSuDungMay
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlControls;
        private System.Windows.Forms.ComboBox cboMay;
        private System.Windows.Forms.ComboBox cboTaiKhoan;
        private System.Windows.Forms.Button btnKetThuc;
        private System.Windows.Forms.Button btnInHoaDon;
        private System.Windows.Forms.Button btnXuatCsv;
        private System.Windows.Forms.DataGridView dgvSuDungMay;
       

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlControls = new System.Windows.Forms.Panel();
            this.cboMay = new System.Windows.Forms.ComboBox();
            this.cboTaiKhoan = new System.Windows.Forms.ComboBox();
            this.btnKetThuc = new System.Windows.Forms.Button();
            this.btnInHoaDon = new System.Windows.Forms.Button();
            this.btnXuatCsv = new System.Windows.Forms.Button();
            this.dgvSuDungMay = new System.Windows.Forms.DataGridView();
            this.pnlHeader.SuspendLayout();
            this.pnlControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuDungMay)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(800, 60);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(26)))));
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(445, 37);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "QUẢN LÝ PHIÊN CHƠI REAL-TIME";
            // 
            // pnlControls
            // 
            this.pnlControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(28)))), ((int)(((byte)(36)))));
            this.pnlControls.Controls.Add(this.cboMay);
            this.pnlControls.Controls.Add(this.cboTaiKhoan);
            this.pnlControls.Controls.Add(this.btnKetThuc);
            this.pnlControls.Controls.Add(this.btnInHoaDon);
            this.pnlControls.Controls.Add(this.btnXuatCsv);
            this.pnlControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlControls.Location = new System.Drawing.Point(0, 60);
            this.pnlControls.Name = "pnlControls";
            this.pnlControls.Size = new System.Drawing.Size(800, 130);
            this.pnlControls.TabIndex = 1;
            // 
            // cboMay
            // 
            this.cboMay.Location = new System.Drawing.Point(40, 16);
            this.cboMay.Name = "cboMay";
            this.cboMay.Size = new System.Drawing.Size(121, 24);
            this.cboMay.TabIndex = 0;
            // 
            // cboTaiKhoan
            // 
            this.cboTaiKhoan.Location = new System.Drawing.Point(214, 16);
            this.cboTaiKhoan.Name = "cboTaiKhoan";
            this.cboTaiKhoan.Size = new System.Drawing.Size(121, 24);
            this.cboTaiKhoan.TabIndex = 1;
            // 
            // btnKetThuc
            // 
            this.btnKetThuc.Location = new System.Drawing.Point(390, 17);
            this.btnKetThuc.Name = "btnKetThuc";
            this.btnKetThuc.Size = new System.Drawing.Size(75, 23);
            this.btnKetThuc.TabIndex = 3;
            this.btnKetThuc.Text = "KẾT THÚC";
            this.btnKetThuc.Click += new System.EventHandler(this.BtnKetThuc_Click);
            // 
            // btnInHoaDon
            // 
            this.btnInHoaDon.Location = new System.Drawing.Point(57, 88);
            this.btnInHoaDon.Name = "btnInHoaDon";
            this.btnInHoaDon.Size = new System.Drawing.Size(75, 23);
            this.btnInHoaDon.TabIndex = 4;
            this.btnInHoaDon.Text = "IN HÓA ĐƠN";
            this.btnInHoaDon.Click += new System.EventHandler(this.BtnInHoaDon_Click);
            // 
            // btnXuatCsv
            // 
            this.btnXuatCsv.Location = new System.Drawing.Point(196, 88);
            this.btnXuatCsv.Name = "btnXuatCsv";
            this.btnXuatCsv.Size = new System.Drawing.Size(75, 23);
            this.btnXuatCsv.TabIndex = 5;
            this.btnXuatCsv.Text = "XUẤT CSV";
            this.btnXuatCsv.Click += new System.EventHandler(this.BtnXuatCsv_Click);
            // 
            // dgvSuDungMay
            // 
            this.dgvSuDungMay.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSuDungMay.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(26)))));
            this.dgvSuDungMay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvSuDungMay.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSuDungMay.ColumnHeadersHeight = 40;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(35)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSuDungMay.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvSuDungMay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuDungMay.EnableHeadersVisualStyles = false;
            this.dgvSuDungMay.Location = new System.Drawing.Point(0, 190);
            this.dgvSuDungMay.Name = "dgvSuDungMay";
            this.dgvSuDungMay.RowHeadersWidth = 51;
            this.dgvSuDungMay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSuDungMay.Size = new System.Drawing.Size(800, 360);
            this.dgvSuDungMay.TabIndex = 0;
            this.dgvSuDungMay.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSuDungMay_CellContentClick);
            // 
            // FormSuDungMay
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(26)))));
            this.ClientSize = new System.Drawing.Size(800, 550);
            this.Controls.Add(this.dgvSuDungMay);
            this.Controls.Add(this.pnlControls);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FormSuDungMay";
            this.Text = "Hệ thống điều khiển máy trạm";
            this.Load += new System.EventHandler(this.FormSuDungMay_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlControls.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuDungMay)).EndInit();
            this.ResumeLayout(false);

        }

       

      
    }
}