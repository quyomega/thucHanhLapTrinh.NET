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
    public partial class ChangePassword : Form
    {
        private int userId;
        private Connect kn = new Connect();

        public ChangePassword(int userId)
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
                string hashedNewPassword = HashPassword(tb_mkMoi.Text);
                // Kiểm tra nếu mật khẩu mới giống mật khẩu cũ
                if (hashedNewPassword == hashedOldPassword)
                {
                    MessageBox.Show("Mật khẩu mới không được giống mật khẩu cũ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

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

        private void pc_an_Click(object sender, EventArgs e)
        {
            tb_mkCu.UseSystemPasswordChar = true;
            pc_an.Visible = false;
            pc_hienthi.Visible = true; 
        }

        private void pc_hienthi_Click(object sender, EventArgs e)
        {
            tb_mkCu.UseSystemPasswordChar = false;
            pc_hienthi.Visible = false;
            pc_an.Visible = true;
        }

        private void DoiMK_Load(object sender, EventArgs e)
        {

        }

        private void pc_an2_Click(object sender, EventArgs e)
        {
            tb_mkMoi.UseSystemPasswordChar = true;
            pc_an2.Visible = false;
            pc_hienthi2.Visible = true;
        }

        private void pc_hienthi2_Click(object sender, EventArgs e)
        {
            tb_mkMoi.UseSystemPasswordChar = false;
            pc_hienthi2.Visible = false;
            pc_an2.Visible = true;
        }

        private void pc_an3_Click(object sender, EventArgs e)
        {
            tb_nhaplaiMK.UseSystemPasswordChar = true;
            pc_an3.Visible = false;
            pc_hienthi3.Visible = true;
        }

        private void pc_hienthi3_Click(object sender, EventArgs e)
        {
            tb_nhaplaiMK.UseSystemPasswordChar = false;
            pc_hienthi3.Visible = false;
            pc_an3.Visible = true;
        }
    }
}
