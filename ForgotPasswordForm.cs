using System;
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
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT username FROM users WHERE email=@Email";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string username = reader["username"].ToString();
                        // Tạo mật khẩu mới
                        string newPassword = GenerateRandomPassword();
                        // Gửi email khôi phục mật khẩu
                        SendRecoveryEmail(email, username, newPassword);
                        MessageBox.Show("Một email khôi phục mật khẩu đã được gửi đến: " + email);

                        // Cập nhật mật khẩu mới vào cơ sở dữ liệu (nếu cần)
                        UpdatePasswordInDatabase(username, newPassword);
                    }
                    else
                    {
                        MessageBox.Show("Email không tồn tại trong hệ thống!");
                    }
                }
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
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE users SET password=@NewPassword WHERE username=@Username";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NewPassword", HashPassword(newPassword)); // Mã hóa mật khẩu trước khi lưu
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}