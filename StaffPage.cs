using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class UserPage : Form
    {
        private Connect kn = new Connect();
        private LoginForm loginForm; // Tham chiếu đến LoginForm
        private string username;
        private int userId;

        // Constructor
        public UserPage(LoginForm form, string username, int userId)
        {
            InitializeComponent();
            this.loginForm = form; // Gán tham chiếu
            this.username = username;
            this.userId = userId;

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
            string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books";
            try
            {
                DataTable booksTable = kn.GetDataTable(query);
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
                dataGridViewBooks.Columns["quantityShelf"].Width = 132;
                dataGridViewBooks.Columns["quantityStore"].Width = 132;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải sách: " + ex.Message);
            }

        }
        private void LoadUserInfo()
        {
            string query = "SELECT username, email, phone, address, TenNhanVien FROM users WHERE username = @username";
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@username", this.username)
            };

                try
                {
                    DataTable dt = kn.GetDataTable(query, parameters);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        txtUsername.Text = row["username"].ToString();
                        txtEmail.Text = row["email"].ToString();
                        txtPhone.Text = row["phone"].ToString();
                        txtAddress.Text = row["address"].ToString();
                        txtTenNhanVien.Text = row["TenNhanVien"].ToString();
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy thông tin người dùng.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
        }

        private bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return System.Text.RegularExpressions.Regex.IsMatch(email, emailPattern);
        }


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
            string email = txtEmail.Text.Trim();
            string phone = txtPhone.Text.Trim();
            string queryCheck = "SELECT COUNT(*) FROM users WHERE (email = @Email OR phone = @Phone) AND username != @Username";
            SqlParameter[] checkParams = new SqlParameter[]
            {
                new SqlParameter("@Email", email),
                new SqlParameter("@Phone", phone),
                new SqlParameter("@Username", this.username)
            };

            try
            {
                int count = (int)kn.ExecuteScalar(queryCheck, checkParams);
                if (count > 0)
                {
                    MessageBox.Show("Email hoặc số điện thoại đã tồn tại trong hệ thống. Vui lòng sử dụng email và số điện thoại khác.");
                    return;
                }

                string queryUpdate = "UPDATE users SET email = @Email, phone = @Phone, address = @Address, TenNhanVien = @TenNhanVien WHERE username = @Username";
                SqlParameter[] updateParams = new SqlParameter[]
                {
                    new SqlParameter("@Username", this.username),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@Address", txtAddress.Text),
                    new SqlParameter("@TenNhanVien", txtTenNhanVien.Text)
                };

                kn.ExecuteQuery(queryUpdate, updateParams);
                MessageBox.Show("Cập nhật thông tin thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi cập nhật thông tin: " + ex.Message);
            }
        }

        private void SearchBooks()
        {
            string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books WHERE 1=1";
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
                query += " AND price = @price";
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

            try
            {
                DataTable booksTable = kn.GetDataTable(query, parameters.ToArray());
                dataGridViewBooks.DataSource = booksTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm sách: " + ex.Message);
            }
        }

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            loginForm.Show(); 
            this.Close(); 
        }

        private void dataGridViewBooks_SelectionChanged(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnDoiMK_Click(object sender, EventArgs e)
        {
            ChangePassword doiMK = new ChangePassword(this.userId); 
            doiMK.ShowDialog();
        }

        private void btnTaoHoaDon_Click(object sender, EventArgs e)
        {
            string currentUsername = this.username;
            CreateInvoice taoHoaDonForm = new CreateInvoice(currentUsername);
            taoHoaDonForm.Show();
        }
    }
}
