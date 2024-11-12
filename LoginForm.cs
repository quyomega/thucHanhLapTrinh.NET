using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class LoginForm : Form
    {
        public int UserId { get; private set; } // Biến lưu `user_id` của người dùng sau khi đăng nhập thành công
        private ketnoi kn = new ketnoi();

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

            string query = "SELECT user_id, role FROM users WHERE username=@username AND password=@password";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", username),
                new SqlParameter("@password", HashPassword(password))
            };

            try
            {
                DataTable dt = kn.GetDataTable(query, parameters);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    UserId = (int)row["user_id"]; // Lấy `user_id` từ kết quả truy vấn
                    string role = row["role"].ToString();

                    // Mở form dựa trên quyền
                    if (role.Trim() == "admin")
                    {
                        admin_page adminForm = new admin_page(this);
                        adminForm.Show();
                    }
                    else
                    {
                        // Gọi user_page với tham số form và username
                        user_page userForm = new user_page(this, username, UserId);  // Truyền form và username vào constructor
                        userForm.Show();
                    }
                    this.Hide(); // Ẩn LoginForm
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi đăng nhập: " + ex.Message);
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

        private void pc_hienthi_Click(object sender, EventArgs e)
        {
            // Hiển thị mật khẩu và chuyển đổi hiển thị PictureBox
            txtPassword.UseSystemPasswordChar = false;
            pc_hienthi.Visible = false; // Ẩn pc_hienthi
            pc_an.Visible = true; // Hiển thị pc_an
        }

        private void pc_an_Click(object sender, EventArgs e)
        {
            // Ẩn mật khẩu và chuyển đổi hiển thị PictureBox
            txtPassword.UseSystemPasswordChar = true;
            pc_an.Visible = false; // Ẩn pc_an
            pc_hienthi.Visible = true; // Hiển thị pc_hienthi
        }
    }
}
