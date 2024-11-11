﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class DoiMK : Form
    {
        private int userId;
        private ketnoi kn = new ketnoi();

        public DoiMK(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password.Trim()));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString().Trim(); // Cắt bỏ khoảng trắng để đảm bảo độ dài chuỗi khớp
            }
        }

        private void XacNhan_Click(object sender, EventArgs e)
        {
            // Kiểm tra tính hợp lệ của mật khẩu mới
            if (tb_mkMoi.Text != tb_nhaplaiMK.Text)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Băm mật khẩu cũ mà người dùng nhập vào
            string hashedOldPassword = HashPassword(tb_mkCu.Text);

            // Kiểm tra mật khẩu cũ trong cơ sở dữ liệu
            string checkQuery = "SELECT COUNT(1) FROM dbo.users WHERE user_id = @userId AND LTRIM(RTRIM(password)) = @oldPassword";
            SqlParameter[] checkParams = new SqlParameter[]
            {
                new SqlParameter("@userId", this.userId),
                new SqlParameter("@oldPassword", hashedOldPassword)
            };

            DataTable dt = kn.GetDataTable(checkQuery, checkParams);
            if (dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0][0]) == 1)
            {
                // Nếu mật khẩu cũ đúng, băm mật khẩu mới và cập nhật
                string hashedNewPassword = HashPassword(tb_mkMoi.Text);
                string updateQuery = "UPDATE dbo.users SET password = @newPassword WHERE user_id = @userId";
                SqlParameter[] updateParams = new SqlParameter[]
                {
                    new SqlParameter("@userId", this.userId),
                    new SqlParameter("@newPassword", hashedNewPassword.PadRight(100)) // Thêm khoảng trắng cho `nchar(100)`
                };

                kn.ExecuteQuery(updateQuery, updateParams); // Thực thi truy vấn cập nhật
                MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close(); // Đóng form đổi mật khẩu sau khi thành công
            }
            else
            {
                MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
    }
}