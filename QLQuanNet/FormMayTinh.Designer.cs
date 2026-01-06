using System.Drawing;
using System.Windows.Forms;

namespace QLQuanNet
{
    partial class FormMayTinh
    {
        private System.ComponentModel.IContainer components = null;

        private Panel pnlHeader;
        private Label lblHeader;
        private FlowLayoutPanel flowPanelComputers;
        private Panel pnlControl;
        private TextBox txtTenMay;
        private TextBox txtGia;
        private ComboBox cboTrangThai;
        private Button btnThem;
        private Button btnSua;
        private Button btnBuocXoa;
        private Label lblL1;
        private Label lblL2;
        private Label lblL3;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.flowPanelComputers = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlControl = new System.Windows.Forms.Panel();
            this.btnBuocXoa = new System.Windows.Forms.Button();
            this.btnSua = new System.Windows.Forms.Button();
            this.btnThem = new System.Windows.Forms.Button();
            this.cboTrangThai = new System.Windows.Forms.ComboBox();
            this.lblL3 = new System.Windows.Forms.Label();
            this.txtGia = new System.Windows.Forms.TextBox();
            this.lblL2 = new System.Windows.Forms.Label();
            this.txtTenMay = new System.Windows.Forms.TextBox();
            this.lblL1 = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.pnlControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(30)))), ((int)(((byte)(45)))));
            this.pnlHeader.Controls.Add(this.lblHeader);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1227, 65);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(25, 15);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(375, 37);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "QUẢN LÝ SƠ ĐỒ MÁY TRẠM";
            // 
            // flowPanelComputers
            // 
            this.flowPanelComputers.AutoScroll = true;
            this.flowPanelComputers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.flowPanelComputers.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelComputers.Location = new System.Drawing.Point(0, 65);
            this.flowPanelComputers.Name = "flowPanelComputers";
            this.flowPanelComputers.Padding = new System.Windows.Forms.Padding(20);
            this.flowPanelComputers.Size = new System.Drawing.Size(1227, 563);
            this.flowPanelComputers.TabIndex = 0;
            // 
            // pnlControl
            // 
            this.pnlControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(24)))), ((int)(((byte)(32)))));
            this.pnlControl.Controls.Add(this.btnBuocXoa);
            this.pnlControl.Controls.Add(this.btnSua);
            this.pnlControl.Controls.Add(this.btnThem);
            this.pnlControl.Controls.Add(this.cboTrangThai);
            this.pnlControl.Controls.Add(this.lblL3);
            this.pnlControl.Controls.Add(this.txtGia);
            this.pnlControl.Controls.Add(this.lblL2);
            this.pnlControl.Controls.Add(this.txtTenMay);
            this.pnlControl.Controls.Add(this.lblL1);
            this.pnlControl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlControl.Location = new System.Drawing.Point(0, 628);
            this.pnlControl.Name = "pnlControl";
            this.pnlControl.Size = new System.Drawing.Size(1227, 130);
            this.pnlControl.TabIndex = 1;
            // 
            // btnBuocXoa
            // 
            this.btnBuocXoa.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(48)))), ((int)(((byte)(49)))));
            this.btnBuocXoa.FlatAppearance.BorderSize = 0;
            this.btnBuocXoa.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuocXoa.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnBuocXoa.ForeColor = System.Drawing.Color.White;
            this.btnBuocXoa.Location = new System.Drawing.Point(828, 45);
            this.btnBuocXoa.Name = "btnBuocXoa";
            this.btnBuocXoa.Size = new System.Drawing.Size(130, 45);
            this.btnBuocXoa.TabIndex = 0;
            this.btnBuocXoa.Text = "XÓA MÁY";
            this.btnBuocXoa.UseVisualStyleBackColor = false;
            // 
            // btnSua
            // 
            this.btnSua.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(139)))), ((int)(((byte)(195)))));
            this.btnSua.FlatAppearance.BorderSize = 0;
            this.btnSua.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSua.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSua.ForeColor = System.Drawing.Color.White;
            this.btnSua.Location = new System.Drawing.Point(692, 45);
            this.btnSua.Name = "btnSua";
            this.btnSua.Size = new System.Drawing.Size(130, 45);
            this.btnSua.TabIndex = 1;
            this.btnSua.Text = "CẬP NHẬT";
            this.btnSua.UseVisualStyleBackColor = false;
            // 
            // btnThem
            // 
            this.btnThem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(166)))), ((int)(((byte)(91)))));
            this.btnThem.FlatAppearance.BorderSize = 0;
            this.btnThem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnThem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnThem.ForeColor = System.Drawing.Color.White;
            this.btnThem.Location = new System.Drawing.Point(556, 45);
            this.btnThem.Name = "btnThem";
            this.btnThem.Size = new System.Drawing.Size(130, 45);
            this.btnThem.TabIndex = 2;
            this.btnThem.Text = "THÊM MỚI";
            this.btnThem.UseVisualStyleBackColor = false;
            // 
            // cboTrangThai
            // 
            this.cboTrangThai.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            this.cboTrangThai.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrangThai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTrangThai.ForeColor = System.Drawing.Color.White;
            this.cboTrangThai.Location = new System.Drawing.Point(390, 55);
            this.cboTrangThai.Name = "cboTrangThai";
            this.cboTrangThai.Size = new System.Drawing.Size(160, 24);
            this.cboTrangThai.TabIndex = 3;
            // 
            // lblL3
            // 
            this.lblL3.AutoSize = true;
            this.lblL3.ForeColor = System.Drawing.Color.Gray;
            this.lblL3.Location = new System.Drawing.Point(390, 25);
            this.lblL3.Name = "lblL3";
            this.lblL3.Size = new System.Drawing.Size(70, 16);
            this.lblL3.TabIndex = 4;
            this.lblL3.Text = "Trạng thái:";
            // 
            // txtGia
            // 
            this.txtGia.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            this.txtGia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGia.ForeColor = System.Drawing.Color.White;
            this.txtGia.Location = new System.Drawing.Point(230, 55);
            this.txtGia.Name = "txtGia";
            this.txtGia.Size = new System.Drawing.Size(140, 22);
            this.txtGia.TabIndex = 5;
            // 
            // lblL2
            // 
            this.lblL2.AutoSize = true;
            this.lblL2.ForeColor = System.Drawing.Color.Gray;
            this.lblL2.Location = new System.Drawing.Point(230, 25);
            this.lblL2.Name = "lblL2";
            this.lblL2.Size = new System.Drawing.Size(78, 16);
            this.lblL2.TabIndex = 6;
            this.lblL2.Text = "Giá tiền/giờ:";
            // 
            // txtTenMay
            // 
            this.txtTenMay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(55)))));
            this.txtTenMay.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTenMay.ForeColor = System.Drawing.Color.White;
            this.txtTenMay.Location = new System.Drawing.Point(30, 55);
            this.txtTenMay.Name = "txtTenMay";
            this.txtTenMay.Size = new System.Drawing.Size(180, 22);
            this.txtTenMay.TabIndex = 7;
            // 
            // lblL1
            // 
            this.lblL1.AutoSize = true;
            this.lblL1.ForeColor = System.Drawing.Color.Gray;
            this.lblL1.Location = new System.Drawing.Point(30, 25);
            this.lblL1.Name = "lblL1";
            this.lblL1.Size = new System.Drawing.Size(63, 16);
            this.lblL1.TabIndex = 8;
            this.lblL1.Text = "Tên máy:";
            // 
            // FormMayTinh
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(40)))));
            this.ClientSize = new System.Drawing.Size(1227, 758);
            this.Controls.Add(this.flowPanelComputers);
            this.Controls.Add(this.pnlControl);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FormMayTinh";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hệ thống quản lý máy";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlControl.ResumeLayout(false);
            this.pnlControl.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}