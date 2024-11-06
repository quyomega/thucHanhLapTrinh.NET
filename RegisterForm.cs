using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class RegisterForm : Form
    {
        private LoginForm _loginForm; // Tham chiếu đến LoginForm

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

            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Kiểm tra xem tên đăng nhập đã tồn tại chưa
                if (IsUsernameExists(conn, username))
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }

                // Kiểm tra xem email đã tồn tại chưa
                if (IsEmailExists(conn, email))
                {
                    MessageBox.Show("Email đã tồn tại!");
                    return;
                }

                // Kiểm tra xem số điện thoại đã tồn tại chưa
                if (IsPhoneExists(conn, phone))
                {
                    MessageBox.Show("Số điện thoại đã tồn tại!");
                    return;
                }

                // Kiểm tra xem địa chỉ đã tồn tại chưa
                if (IsAddressExists(conn, address))
                {
                    MessageBox.Show("Địa chỉ đã tồn tại thật rồi!");
                    return;
                }

                // Nếu không có sự trùng lặp, thực hiện chèn dữ liệu
                string query = "INSERT INTO users (username, password, email, phone, address, role) VALUES (@Username, @Password, @Email, @Phone, @Address, 'user')";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    cmd.Parameters.AddWithValue("@Password", HashPassword(password)); 
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    cmd.Parameters.AddWithValue("@Address", address);

                    try
                    {
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Đăng ký thành công! Bạn có thể đăng nhập.");
                        this.Close(); // Đóng form đăng ký
                        _loginForm.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đăng ký không thành công: " + ex.Message);
                    }
                }
            }
        }

        private bool IsUsernameExists(SqlConnection conn, string username)
        {
            string query = "SELECT COUNT(*) FROM users WHERE username = @Username";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private bool IsEmailExists(SqlConnection conn, string email)
        {
            string query = "SELECT COUNT(*) FROM users WHERE email = @Email";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Email", email);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private bool IsPhoneExists(SqlConnection conn, string phone)
        {
            string query = "SELECT COUNT(*) FROM users WHERE phone = @Phone";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Phone", phone);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
        }

        private bool IsAddressExists(SqlConnection conn, string address)
        {
            string query = "SELECT COUNT(*) FROM users WHERE address = @Address";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Address", address);
                int count = (int)cmd.ExecuteScalar();
                return count > 0;
            }
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
