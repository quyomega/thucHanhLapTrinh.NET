using System;
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


        private void but_XacNhan_Click(object sender, EventArgs e)
        {
            // Kiểm tra tính hợp lệ của mật khẩu mới
            if (tb_mkMoi.Text != tb_nhaplaiMK.Text)
            {
                MessageBox.Show("Mật khẩu mới và nhập lại mật khẩu không khớp!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string connectionString = "Data Source=DESKTOP-V71IBDD;Initial Catalog=baitaplon;Integrated Security=True";
            string hashedOldPassword = HashPassword(tb_mkCu.Text);

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    // Kiểm tra mật khẩu cũ của người dùng dựa trên `userId` và `hashedOldPassword`
                    SqlCommand checkCmd = new SqlCommand("SELECT COUNT(1) FROM dbo.users WHERE user_id = @userId AND LTRIM(RTRIM(password)) = @oldPassword", conn);
                    checkCmd.Parameters.AddWithValue("@userId", this.userId);
                    checkCmd.Parameters.AddWithValue("@oldPassword", hashedOldPassword);

                    int userExists = (int)checkCmd.ExecuteScalar();

                    if (userExists == 1)
                    {
                        // Băm mật khẩu mới trước khi cập nhật
                        string hashedNewPassword = HashPassword(tb_mkMoi.Text);

                        // Cập nhật mật khẩu mới
                        SqlCommand updateCmd = new SqlCommand("UPDATE dbo.users SET password = @newPassword WHERE user_id = @userId", conn);
                        updateCmd.Parameters.AddWithValue("@userId", this.userId);
                        updateCmd.Parameters.AddWithValue("@newPassword", hashedNewPassword.PadRight(100)); // Bổ sung khoảng trắng để phù hợp với kiểu `nchar(100)`

                        updateCmd.ExecuteNonQuery();
                        MessageBox.Show("Đổi mật khẩu thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Mật khẩu cũ không đúng!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
