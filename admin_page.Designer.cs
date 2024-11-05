namespace baitaplon
{
    partial class admin_page
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.btnPhieuXuatKho = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtPublisher = new System.Windows.Forms.TextBox();
            this.txtCategory = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.dataGridViewBooks = new System.Windows.Forms.DataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.searchTimer = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBooks)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Controls.Add(this.tabPage7);
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabControl1.Location = new System.Drawing.Point(13, 12);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1256, 706);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.btnPhieuXuatKho);
            this.tabPage5.Controls.Add(this.panel1);
            this.tabPage5.Controls.Add(this.dataGridViewBooks);
            this.tabPage5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(1248, 673);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Quản lý sách";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // btnPhieuXuatKho
            // 
            this.btnPhieuXuatKho.Location = new System.Drawing.Point(661, 391);
            this.btnPhieuXuatKho.Name = "btnPhieuXuatKho";
            this.btnPhieuXuatKho.Size = new System.Drawing.Size(165, 54);
            this.btnPhieuXuatKho.TabIndex = 4;
            this.btnPhieuXuatKho.Text = "Tạo phiếu xuất kho";
            this.btnPhieuXuatKho.UseVisualStyleBackColor = true;
            this.btnPhieuXuatKho.Click += new System.EventHandler(this.btnPhieuXuatKho_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbLocation);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtPrice);
            this.panel1.Controls.Add(this.txtPublisher);
            this.panel1.Controls.Add(this.txtCategory);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Location = new System.Drawing.Point(17, 387);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(529, 269);
            this.panel1.TabIndex = 3;
            // 
            // cmbLocation
            // 
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Items.AddRange(new object[] {
            "Tất Cả",
            "Tại Kệ",
            "Tại Kho"});
            this.cmbLocation.Location = new System.Drawing.Point(278, 223);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(193, 28);
            this.cmbLocation.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 223);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(151, 20);
            this.label5.TabIndex = 6;
            this.label5.Text = "Tìm kiếm theo vị trí";
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(278, 170);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(193, 27);
            this.txtPrice.TabIndex = 5;
            this.txtPrice.TextChanged += new System.EventHandler(this.SearchCriteriaChanged);
            // 
            // txtPublisher
            // 
            this.txtPublisher.Location = new System.Drawing.Point(278, 122);
            this.txtPublisher.Name = "txtPublisher";
            this.txtPublisher.Size = new System.Drawing.Size(193, 27);
            this.txtPublisher.TabIndex = 5;
            this.txtPublisher.TextChanged += new System.EventHandler(this.SearchCriteriaChanged);
            // 
            // txtCategory
            // 
            this.txtCategory.Location = new System.Drawing.Point(278, 71);
            this.txtCategory.Name = "txtCategory";
            this.txtCategory.Size = new System.Drawing.Size(193, 27);
            this.txtCategory.TabIndex = 5;
            this.txtCategory.TextChanged += new System.EventHandler(this.SearchCriteriaChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(141, 20);
            this.label4.TabIndex = 4;
            this.label4.Text = "Tìm kiếm theo giá";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 129);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Tìm kiếm theo NXB";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(173, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Tìm kiếm theo thể loại";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Tìm kiếm theo tên";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(278, 14);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(193, 27);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.SearchCriteriaChanged);
            // 
            // dataGridViewBooks
            // 
            this.dataGridViewBooks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBooks.Location = new System.Drawing.Point(17, 16);
            this.dataGridViewBooks.Name = "dataGridViewBooks";
            this.dataGridViewBooks.RowHeadersWidth = 51;
            this.dataGridViewBooks.RowTemplate.Height = 24;
            this.dataGridViewBooks.Size = new System.Drawing.Size(1216, 338);
            this.dataGridViewBooks.TabIndex = 0;
            this.dataGridViewBooks.SelectionChanged += new System.EventHandler(this.dataGridViewBooks_SelectionChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(1248, 673);
            this.tabPage3.TabIndex = 6;
            this.tabPage3.Text = "Sách đã bán";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(1248, 673);
            this.tabPage4.TabIndex = 7;
            this.tabPage4.Text = "Sách cần mua";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage6
            // 
            this.tabPage6.Location = new System.Drawing.Point(4, 29);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1248, 673);
            this.tabPage6.TabIndex = 8;
            this.tabPage6.Text = "Quản lý nhân viên";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // tabPage7
            // 
            this.tabPage7.Location = new System.Drawing.Point(4, 29);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(1248, 673);
            this.tabPage7.TabIndex = 9;
            this.tabPage7.Text = "Quản lý thu chi";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(61, 4);
            // 
            // searchTimer
            // 
            this.searchTimer.Interval = 500;
            // 
            // admin_page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1281, 730);
            this.Controls.Add(this.tabControl1);
            this.Name = "admin_page";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trang quản trị";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing_1);
            this.tabControl1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBooks)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TabPage tabPage7;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.DataGridView dataGridViewBooks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Timer searchTimer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtPublisher;
        private System.Windows.Forms.TextBox txtCategory;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbLocation;
        private System.Windows.Forms.Button btnPhieuXuatKho;
    }
}

