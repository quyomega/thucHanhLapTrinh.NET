namespace baitaplon
{
    partial class PhieuXuatKhoForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnLuuPhieu = new System.Windows.Forms.Button();
            this.txtMaPhieu = new System.Windows.Forms.TextBox();
            this.txtMaSach = new System.Windows.Forms.TextBox();
            this.txtSoLuong = new System.Windows.Forms.TextBox();
            this.dateTimePickerNgayXuat = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbNhanVien = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Mã phiếu";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 16);
            this.label2.TabIndex = 1;
            this.label2.Text = "Mã sách";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 16);
            this.label3.TabIndex = 2;
            this.label3.Text = "Số lượng";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 213);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(92, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Ngày xuất kho";
            // 
            // btnLuuPhieu
            // 
            this.btnLuuPhieu.Location = new System.Drawing.Point(276, 329);
            this.btnLuuPhieu.Name = "btnLuuPhieu";
            this.btnLuuPhieu.Size = new System.Drawing.Size(113, 36);
            this.btnLuuPhieu.TabIndex = 4;
            this.btnLuuPhieu.Text = "Tạo phiếu";
            this.btnLuuPhieu.UseVisualStyleBackColor = true;
            this.btnLuuPhieu.Click += new System.EventHandler(this.btnLuuPhieu_Click);
            // 
            // txtMaPhieu
            // 
            this.txtMaPhieu.Location = new System.Drawing.Point(189, 33);
            this.txtMaPhieu.Name = "txtMaPhieu";
            this.txtMaPhieu.Size = new System.Drawing.Size(200, 22);
            this.txtMaPhieu.TabIndex = 5;
            // 
            // txtMaSach
            // 
            this.txtMaSach.Location = new System.Drawing.Point(189, 88);
            this.txtMaSach.Name = "txtMaSach";
            this.txtMaSach.Size = new System.Drawing.Size(200, 22);
            this.txtMaSach.TabIndex = 6;
            // 
            // txtSoLuong
            // 
            this.txtSoLuong.Location = new System.Drawing.Point(189, 151);
            this.txtSoLuong.Name = "txtSoLuong";
            this.txtSoLuong.Size = new System.Drawing.Size(200, 22);
            this.txtSoLuong.TabIndex = 7;
            // 
            // dateTimePickerNgayXuat
            // 
            this.dateTimePickerNgayXuat.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerNgayXuat.Location = new System.Drawing.Point(189, 206);
            this.dateTimePickerNgayXuat.Name = "dateTimePickerNgayXuat";
            this.dateTimePickerNgayXuat.Size = new System.Drawing.Size(200, 22);
            this.dateTimePickerNgayXuat.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 273);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 16);
            this.label5.TabIndex = 9;
            this.label5.Text = "Nhân viên thực hiện";
            // 
            // cmbNhanVien
            // 
            this.cmbNhanVien.FormattingEnabled = true;
            this.cmbNhanVien.Location = new System.Drawing.Point(189, 273);
            this.cmbNhanVien.Name = "cmbNhanVien";
            this.cmbNhanVien.Size = new System.Drawing.Size(200, 24);
            this.cmbNhanVien.TabIndex = 10;
            // 
            // PhieuXuatKhoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(425, 391);
            this.Controls.Add(this.cmbNhanVien);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.dateTimePickerNgayXuat);
            this.Controls.Add(this.txtSoLuong);
            this.Controls.Add(this.txtMaSach);
            this.Controls.Add(this.txtMaPhieu);
            this.Controls.Add(this.btnLuuPhieu);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "PhieuXuatKhoForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PhieuXuatKho";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnLuuPhieu;
        private System.Windows.Forms.TextBox txtMaPhieu;
        private System.Windows.Forms.TextBox txtMaSach;
        private System.Windows.Forms.TextBox txtSoLuong;
        private System.Windows.Forms.DateTimePicker dateTimePickerNgayXuat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbNhanVien;
    }
}