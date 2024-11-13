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
        private Connect kn = new Connect();

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
            string email = txtEmail.Text.Trim();
            string query = "SELECT username FROM users WHERE email=@Email";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Email", email) };

            try
            {
                DataTable dt = kn.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    if (IsAccountLocked(email))
                    {
                        MessageBox.Show("Tài khoản này đã bị khóa do quá nhiều lần yêu cầu khôi phục mật khẩu. Vui lòng thử lại sau 24 giờ.");
                        return;
                    }

                    string username = dt.Rows[0]["username"].ToString();
                    string newPassword = GenerateRandomPassword();

                    SendRecoveryEmail(email, username, newPassword);
                    MessageBox.Show("Một email khôi phục mật khẩu đã được gửi đến: " + email);

                    UpdatePasswordInDatabase(username, newPassword);

                    UpdateRecoveryRequestCount(email);
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
        private bool IsAccountLocked(string email)
        {
            string query = "SELECT request_count, last_request_time FROM PasswordRecoveryRequests WHERE email=@Email";
            SqlParameter[] parameters = new SqlParameter[] { new SqlParameter("@Email", email) };
            DataTable dt = kn.GetDataTable(query, parameters);

            if (dt.Rows.Count > 0)
            {
                int requestCount = Convert.ToInt32(dt.Rows[0]["request_count"]);
                DateTime lastRequestTime = Convert.ToDateTime(dt.Rows[0]["last_request_time"]);

                if (requestCount >= 5 && (DateTime.Now - lastRequestTime).TotalHours < 24)
                {
                    return true; 
                }
            }

            return false; 
        }
        private void UpdateRecoveryRequestCount(string email)
        {
            string query = "IF EXISTS (SELECT 1 FROM PasswordRecoveryRequests WHERE email=@Email) " +
                           "UPDATE PasswordRecoveryRequests SET request_count = request_count + 1, last_request_time = @LastRequestTime WHERE email=@Email " +
                           "ELSE " +
                           "INSERT INTO PasswordRecoveryRequests (email, request_count, last_request_time) VALUES (@Email, 1, @LastRequestTime)";
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@Email", email),
        new SqlParameter("@LastRequestTime", DateTime.Now)
            };

            kn.ExecuteQuery(query, parameters);
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
            string fromEmail = "hanglam1209@gmail.com"; 
            string password = "qlou tusy pfxo adhr"; 
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
                    smtp.EnableSsl = true; 
                    smtp.Send(mail);
                }
            }
        }
        private void UpdatePasswordInDatabase(string username, string newPassword)
        {
            string query = "UPDATE users SET password=@NewPassword WHERE username=@Username";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@NewPassword", HashPassword(newPassword)), 
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