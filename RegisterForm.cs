using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class RegisterForm : Form
    {
        private LoginForm _loginForm; // Tham chiếu đến LoginForm
        private ketnoi kn = new ketnoi();

        public RegisterForm(LoginForm loginForm) // Nhận tham chiếu từ LoginForm
        {
            InitializeComponent();
            _loginForm = loginForm ?? throw new ArgumentNullException(nameof(loginForm)); // Kiểm tra null
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

        // Phương thức kiểm tra email hợp lệ
        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;
            string confirmPassword = txtConfirmPassword.Text;
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string address = txtAddress.Text.Trim();

            // Kiểm tra các điều kiện nhập liệu
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword) ||
                string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(address))
            {
                MessageBox.Show("Vui lòng điền đầy đủ thông tin!");
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu và xác nhận mật khẩu không khớp!");
                return;
            }

            // Kiểm tra email hợp lệ
            if (!IsValidEmail(email))
            {
                MessageBox.Show("Địa chỉ email không hợp lệ!");
                return;
            }

            try
            {
                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (IsUsernameExists(username))
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }

                // Kiểm tra xem email đã tồn tại chưa
                if (IsEmailExists(email))
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }

                // Kiểm tra xem số điện thoại đã tồn tại chưa
                if (IsPhoneExists(phone))
                {
                    MessageBox.Show("Số điện thoại đã tồn tại!");
                    return;
                }

                // Kiểm tra xem địa chỉ đã tồn tại chưa
                if (IsAddressExists(address))
                {
                    MessageBox.Show("Địa chỉ đã tồn tại!");
                    return;
                }

                // Nếu không có sự trùng lặp, thực hiện chèn dữ liệu
                string query = "INSERT INTO users (username, password, email, phone, address, role) VALUES (@Username, @Password, @Email, @Phone, @Address, 'user')";
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", HashPassword(password)),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@Address", address)
                };

                kn.ExecuteQuery(query, parameters);
                MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập.");
                this.Close(); // Đóng form đăng ký
                _loginForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đăng ký không thành công: " + ex.Message);
            }
        }

        private bool IsUsernameExists(string username)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @Username";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", username)
            };

            object result = kn.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }

        private bool IsEmailExists(string email)
        {
            string query = "SELECT COUNT(*) FROM users WHERE email = @Email";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", email)
            };

            object result = kn.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;

        }

        private bool IsPhoneExists(string phone)
        {
            string query = "SELECT COUNT(*) FROM users WHERE phone = @Phone";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Phone", phone)
            };

            object result = kn.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }

        private bool IsAddressExists(string address)
        {
            string query = "SELECT COUNT(*) FROM users WHERE address = @Address";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Address", address)
            };

            object result = kn.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result) > 0;
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            ShowLoginForm();
        }
        private void ShowLoginForm()
        {
            _loginForm.Show(); 
            this.Close(); 
        }
    }
}
