namespace baitaplon
{
    partial class DoiMK
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
            this.tb_mkCu = new System.Windows.Forms.TextBox();
            this.tb_mkMoi = new System.Windows.Forms.TextBox();
            this.tb_nhaplaiMK = new System.Windows.Forms.TextBox();
            this.but_XacNhan = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // tb_mkCu
            // 
            this.tb_mkCu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_mkCu.Location = new System.Drawing.Point(401, 155);
            this.tb_mkCu.Name = "tb_mkCu";
            this.tb_mkCu.Size = new System.Drawing.Size(347, 38);
            this.tb_mkCu.TabIndex = 0;
            this.tb_mkCu.UseSystemPasswordChar = true;
            // 
            // tb_mkMoi
            // 
            this.tb_mkMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_mkMoi.Location = new System.Drawing.Point(401, 277);
            this.tb_mkMoi.Name = "tb_mkMoi";
            this.tb_mkMoi.Size = new System.Drawing.Size(347, 38);
            this.tb_mkMoi.TabIndex = 0;
            this.tb_mkMoi.UseSystemPasswordChar = true;
            // 
            // tb_nhaplaiMK
            // 
            this.tb_nhaplaiMK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_nhaplaiMK.Location = new System.Drawing.Point(401, 407);
            this.tb_nhaplaiMK.Name = "tb_nhaplaiMK";
            this.tb_nhaplaiMK.Size = new System.Drawing.Size(347, 38);
            this.tb_nhaplaiMK.TabIndex = 0;
            this.tb_nhaplaiMK.UseSystemPasswordChar = true;
            // 
            // but_XacNhan
            // 
            this.but_XacNhan.Location = new System.Drawing.Point(363, 513);
            this.but_XacNhan.Name = "but_XacNhan";
            this.but_XacNhan.Size = new System.Drawing.Size(150, 73);
            this.but_XacNhan.TabIndex = 1;
            this.but_XacNhan.Text = "Xác Nhận";
            this.but_XacNhan.UseVisualStyleBackColor = true;
            this.but_XacNhan.Click += new System.EventHandler(this.but_XacNhan_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(66, 164);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "MK cũ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(66, 290);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 25);
            this.label2.TabIndex = 3;
            this.label2.Text = "MK mới";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 420);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(129, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Nhập lại MK";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(331, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(307, 46);
            this.label4.TabIndex = 5;
            this.label4.Text = "ĐỔI MẬT KHẨU";
            // 
            // DoiMK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 612);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.but_XacNhan);
            this.Controls.Add(this.tb_nhaplaiMK);
            this.Controls.Add(this.tb_mkMoi);
            this.Controls.Add(this.tb_mkCu);
            this.Name = "DoiMK";
            this.Text = "DoiMK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_mkCu;
        private System.Windows.Forms.TextBox tb_mkMoi;
        private System.Windows.Forms.TextBox tb_nhaplaiMK;
        private System.Windows.Forms.Button but_XacNhan;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
    }
}