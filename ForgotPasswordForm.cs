using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class ForgotPasswordForm : Form
    {

        private ketnoi kn = new ketnoi();

        public ForgotPasswordForm()
        {
            InitializeComponent();
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim(); // Lấy email và loại bỏ khoảng trắng
            string query = "SELECT username FROM users WHERE email=@Email";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email)
            };

            try
            {
                DataTable dt = kn.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    string username = dt.Rows[0]["username"].ToString();

                    // Tạo mật khẩu mới
                    string newPassword = GenerateRandomPassword();

                    // Gửi email khôi phục mật khẩu
                    SendRecoveryEmail(email, username, newPassword);
                    MessageBox.Show("Một email khôi phục mật khẩu đã được gửi đến: " + email);

                    // Cập nhật mật khẩu mới vào cơ sở dữ liệu
                    UpdatePasswordInDatabase(username, newPassword);
                }
                else
                {
                    MessageBox.Show("Email không tồn tại trong hệ thống!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gửi email khôi phục: " + ex.Message);
            }
        }

        private string GenerateRandomPassword(int length = 10)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            Random random = new Random();
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(validChars.Length)];
            }
            return new string(chars);
        }

        private void SendRecoveryEmail(string email, string username, string newPassword)
        {
            string fromEmail = "hanglam1209@gmail.com"; // Thay thế bằng email của bạn
            string password = "qlou tusy pfxo adhr"; // Thay thế bằng mật khẩu của bạn
            string displayName = "Nhóm 9 - DHTI15A14";
            string subject = "Khôi phục mật khẩu";
            string body = $"Chào {username},\n\nBạn đã yêu cầu khôi phục mật khẩu. Mật khẩu mới của bạn là: {newPassword}\n\n" +
                          "Vui lòng thay đổi mật khẩu này ngay sau khi đăng nhập.\n\n" +
                          "Cảm ơn bạn!";

            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(fromEmail, displayName);
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = false;
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential(fromEmail, password);
                    smtp.EnableSsl = true; // Bật SSL
                    smtp.Send(mail);
                }
            }
        }

        private void UpdatePasswordInDatabase(string username, string newPassword)
        {
            string query = "UPDATE users SET password=@NewPassword WHERE username=@Username";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NewPassword", HashPassword(newPassword)), // Mã hóa mật khẩu trước khi lưu
                new SqlParameter("@Username", username)
            };

            try
            {
                kn.ExecuteQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật mật khẩu: " + ex.Message);
            }
        }

    }
}