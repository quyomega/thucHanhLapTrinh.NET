using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class user_page : Form
    {
        private LoginForm loginForm; // Tham chiếu đến LoginForm
        private string username;

        // Constructor
        public user_page(LoginForm form, string username)
        {
            InitializeComponent();
            this.loginForm = form; // Gán tham chiếu
            this.username = username;
            LoadUserInfo();
        }

        // Hàm tải thông tin người dùng từ cơ sở dữ liệu
        private void LoadUserInfo()
        {
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            string query = "SELECT username, email, phone, address, TenNhanVien FROM users WHERE username = @username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", this.username);

                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtUsername.Text = reader["username"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        txtPhone.Text = reader["phone"].ToString();
                        txtAddress.Text = reader["address"].ToString();
                        txtTenNhanVien.Text = reader["TenNhanVien"].ToString();

                        // Kiểm tra dữ liệu có được lấy đúng không
                        MessageBox.Show("Thông tin người dùng: " +
                            "\nUsername: " + txtUsername.Text +
                            "\nEmail: " + txtEmail.Text +
                            "\nPhone: " + txtPhone.Text +
                            "\nAddress: " + txtAddress.Text +
                            "\nTenNhanVien: " + txtTenNhanVien.Text);
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin người dùng.");
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }

        // Sự kiện khi form user_page được tải
        private void user_page_Load(object sender, EventArgs e)
        {
            LoadUserInfo();
        }

        // Sự kiện đóng form
        private void Form2_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }
        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            string query = "UPDATE users SET email = @email, phone = @phone, address = @address, TenNhanVien = @TenNhanVien WHERE username = @username";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@username", this.username);
                    cmd.Parameters.AddWithValue("@email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@TenNhanVien", txtTenNhanVien.Text);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Cập nhật thông tin thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Không thể cập nhật thông tin. Vui lòng thử lại.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi cập nhật: " + ex.Message);
                }
            }
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            loginForm.Show(); // Hiển thị lại form Login
            this.Close(); // Đóng form user_page
        }
    }
}
