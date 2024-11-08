using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
            LoadBooks();
            SearchBooks();
             txtName.TextChanged += txtSearch_TextChanged;
            txtCategory.TextChanged += SearchCriteriaChanged;
            txtPublisher.TextChanged += SearchCriteriaChanged;
            txtPrice.TextChanged += SearchCriteriaChanged;

            cmbLocation.SelectedIndexChanged += SearchCriteriaChanged;
            // Sự kiện Tick cho Timer tìm kiếm
            searchTimer.Tick += searchTimer_Tick;
            dataGridViewBooks.SelectionChanged += dataGridViewBooks_SelectionChanged;
        }
        private void searchTimer_Tick(object sender, EventArgs e)
        {
            // Dừng Timer để không thực hiện tìm kiếm nhiều lần
            searchTimer.Stop();
            // Gọi hàm tìm kiếm
            SearchBooks();
        }
        private void LoadBooks()
        {
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable booksTable = new DataTable();
                adapter.Fill(booksTable);

                // Đặt DataSource cho DataGridView
                dataGridViewBooks.DataSource = booksTable;

                // Đổi tên cột theo ý muốn
                dataGridViewBooks.Columns["book_id"].HeaderText = "Mã Sách";
                dataGridViewBooks.Columns["book_name"].HeaderText = "Tên Sách";
                dataGridViewBooks.Columns["category"].HeaderText = "Thể Loại";
                dataGridViewBooks.Columns["publishing"].HeaderText = "Nhà Xuất Bản";
                dataGridViewBooks.Columns["price"].HeaderText = "Giá";
                dataGridViewBooks.Columns["quantityShelf"].HeaderText = "SL Trên Kệ";
                dataGridViewBooks.Columns["quantityStore"].HeaderText = "SL Trong Kho";

                dataGridViewBooks.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewBooks.Font, FontStyle.Bold);

                dataGridViewBooks.Columns["book_id"].Width = 99;
                dataGridViewBooks.Columns["book_name"].Width = 140;
                dataGridViewBooks.Columns["category"].Width = 120;
                dataGridViewBooks.Columns["publishing"].Width = 150;
                dataGridViewBooks.Columns["price"].Width = 90;
                dataGridViewBooks.Columns["quantityShelf"].Width = 120;
                dataGridViewBooks.Columns["quantityStore"].Width = 140;
            }
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
        private void SearchCriteriaChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
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
        private void SearchBooks()
        {
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books WHERE 1=1";

                // Thêm các điều kiện tìm kiếm nếu có
                List<SqlParameter> parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(txtName.Text))
                {
                    query += " AND book_name LIKE @name";
                    parameters.Add(new SqlParameter("@name", "%" + txtName.Text + "%"));
                }
                if (!string.IsNullOrEmpty(txtCategory.Text))
                {
                    query += " AND category LIKE @category";
                    parameters.Add(new SqlParameter("@category", "%" + txtCategory.Text + "%"));
                }
                if (!string.IsNullOrEmpty(txtPublisher.Text))
                {
                    query += " AND publishing LIKE @publisher";
                    parameters.Add(new SqlParameter("@publisher", "%" + txtPublisher.Text + "%"));
                }
                if (decimal.TryParse(txtPrice.Text, out decimal price))
                {
                    query += " AND price LIKE @price";
                    parameters.Add(new SqlParameter("@price", price));
                }

                string selectedLocation = cmbLocation.SelectedItem?.ToString();
                if (selectedLocation == "Tại Kệ")
                {
                    query += " AND quantityShelf > 0";
                }
                else if (selectedLocation == "Tại Kho")
                {
                    query += " AND quantityStore > 0";
                }
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                adapter.SelectCommand.Parameters.AddRange(parameters.ToArray());

                DataTable booksTable = new DataTable();
                adapter.Fill(booksTable);
                dataGridViewBooks.DataSource = booksTable;
            }
        }
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            loginForm.Show(); // Hiển thị lại form Login
            this.Close(); // Đóng form user_page
        }

        private void dataGridViewBooks_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
