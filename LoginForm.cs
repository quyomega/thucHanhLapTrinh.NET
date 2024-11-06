using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class LoginForm : Form
    {
        public LoginForm()
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

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT role FROM users WHERE username=@username AND password=@password";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", HashPassword(password));

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        string role = reader["role"].ToString();
                        // Mở form dựa trên quyền
                        if (role.Trim() == "admin")
                        {
                            admin_page adminForm = new admin_page(this);
                            adminForm.Show();
                        }
                        else
                        {
                            // Gọi user_page với tham số form và username
                            user_page userForm = new user_page(this, username);  // Truyền form và username vào constructor
                            userForm.Show();
                        }
                        this.Hide(); // Ẩn LoginForm
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                    }
                }
            }
        }



        private void label3_Click(object sender, EventArgs e)
        {
            ForgotPasswordForm forgotPasswordForm = new ForgotPasswordForm();
            forgotPasswordForm.ShowDialog();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            RegisterForm registerForm = new RegisterForm(this); // Chuyển tham chiếu đến LoginForm
            registerForm.Show();
        }
    }
}
