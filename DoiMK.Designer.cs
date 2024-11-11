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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_mkCu = new System.Windows.Forms.TextBox();
            this.tb_mkMoi = new System.Windows.Forms.TextBox();
            this.tb_nhaplaiMK = new System.Windows.Forms.TextBox();
            this.XacNhan = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(342, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(307, 46);
            this.label1.TabIndex = 0;
            this.label1.Text = "ĐỔI MẬT KHẨU";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(82, 155);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 25);
            this.label2.TabIndex = 1;
            this.label2.Text = "MK Cũ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(82, 280);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "MK Mới";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(82, 399);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(136, 25);
            this.label4.TabIndex = 3;
            this.label4.Text = "Nhập Lại MK";
            // 
            // tb_mkCu
            // 
            this.tb_mkCu.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_mkCu.Location = new System.Drawing.Point(423, 142);
            this.tb_mkCu.Name = "tb_mkCu";
            this.tb_mkCu.Size = new System.Drawing.Size(436, 38);
            this.tb_mkCu.TabIndex = 4;
            this.tb_mkCu.UseSystemPasswordChar = true;
            // 
            // tb_mkMoi
            // 
            this.tb_mkMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_mkMoi.Location = new System.Drawing.Point(423, 267);
            this.tb_mkMoi.Name = "tb_mkMoi";
            this.tb_mkMoi.Size = new System.Drawing.Size(436, 38);
            this.tb_mkMoi.TabIndex = 5;
            this.tb_mkMoi.UseSystemPasswordChar = true;
            // 
            // tb_nhaplaiMK
            // 
            this.tb_nhaplaiMK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tb_nhaplaiMK.Location = new System.Drawing.Point(423, 386);
            this.tb_nhaplaiMK.Name = "tb_nhaplaiMK";
            this.tb_nhaplaiMK.Size = new System.Drawing.Size(436, 38);
            this.tb_nhaplaiMK.TabIndex = 6;
            this.tb_nhaplaiMK.UseSystemPasswordChar = true;
            // 
            // XacNhan
            // 
            this.XacNhan.Location = new System.Drawing.Point(391, 515);
            this.XacNhan.Name = "XacNhan";
            this.XacNhan.Size = new System.Drawing.Size(199, 69);
            this.XacNhan.TabIndex = 7;
            this.XacNhan.Text = "Xác Nhận";
            this.XacNhan.UseVisualStyleBackColor = true;
            this.XacNhan.Click += new System.EventHandler(this.XacNhan_Click);
            // 
            // DoiMK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1066, 634);
            this.Controls.Add(this.XacNhan);
            this.Controls.Add(this.tb_nhaplaiMK);
            this.Controls.Add(this.tb_mkMoi);
            this.Controls.Add(this.tb_mkCu);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "DoiMK";
            this.Text = "DoiMK";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_mkCu;
        private System.Windows.Forms.TextBox tb_mkMoi;
        private System.Windows.Forms.TextBox tb_nhaplaiMK;
        private System.Windows.Forms.Button XacNhan;
    }
}