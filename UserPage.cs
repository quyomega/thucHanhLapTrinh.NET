using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace baitaplon
{
    public partial class UserPage : Form
    {
        private Connect kn = new Connect();
        private LoginForm loginForm; 
        private string username;
        private int userId;
        public UserPage(LoginForm form, string username, int userId)
        {
            InitializeComponent();
            this.loginForm = form; 
            this.username = username;
            this.userId = userId;
            LoadUserInfo();
            LoadBooks();
            LoadToDoList();
            SearchBooks();
            SetupEventHandlers();
        }
        private void SetupEventHandlers()
        {
            txtName.TextChanged += SearchCriteriaChanged;
            txtCategory.TextChanged += SearchCriteriaChanged;
            txtPublisher.TextChanged += SearchCriteriaChanged;
            txtPrice.TextChanged += SearchCriteriaChanged;
            cmbLocation.SelectedIndexChanged += SearchCriteriaChanged;
            searchTimer.Tick += searchTimer_Tick;
            dataGridViewBooks.SelectionChanged += dataGridViewBooks_SelectionChanged;

            dgvToDoList.SelectionChanged += dgvToDoList_SelectionChanged;

        }
        private void SearchCriteriaChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }
        private void searchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            SearchBooks();
        }
//Hàm tải trang
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
        private void LoadBooks()
        {
            string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books";
            
            DataTable booksTable = kn.GetDataTable(query);
            dataGridViewBooks.DataSource = booksTable;

            SetBookColumnHeaders();
        }
        private void LoadToDoList()
        {
            try
            {
                string query = "SELECT maPhieu, book_id, user_id, thoiGianTaoPhieu, thoiGianThucHien, soLuong, trangThai, hinhThuc " +
                               "FROM PhieuXuatKho " +
                               "WHERE user_id = @userId ";
                //AND trangThai = 'Chưa làm'
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", this.userId)
                };

                DataTable todolistTable = kn.GetDataTable(query, parameters);
                dgvToDoList.DataSource = todolistTable;

                ToDoListColumnHeaders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách: " + ex.Message);
            }
        }

        //căn chỉnh dgv
        private void SetBookColumnHeaders()
        {
            dataGridViewBooks.Columns["book_id"].HeaderText = "Mã Sách";
            dataGridViewBooks.Columns["book_name"].HeaderText = "Tên Sách";
            dataGridViewBooks.Columns["category"].HeaderText = "Thể Loại";
            dataGridViewBooks.Columns["publishing"].HeaderText = "Nhà Xuất Bản";
            dataGridViewBooks.Columns["price"].HeaderText = "Giá";
            dataGridViewBooks.Columns["quantityShelf"].HeaderText = "SL Trên Kệ";
            dataGridViewBooks.Columns["quantityStore"].HeaderText = "SL Trong Kho";

            dataGridViewBooks.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewBooks.Font, FontStyle.Bold);
            dataGridViewBooks.RowHeadersVisible = false;
            dataGridViewBooks.AllowUserToResizeColumns = false; 
            dataGridViewBooks.AllowUserToResizeRows = false;

            dataGridViewBooks.Columns["book_id"].Width = 100;
            dataGridViewBooks.Columns["book_name"].Width = 145;
            dataGridViewBooks.Columns["category"].Width = 120;
            dataGridViewBooks.Columns["publishing"].Width = 150;
            dataGridViewBooks.Columns["price"].Width = 70;
            dataGridViewBooks.Columns["quantityShelf"].Width = 130;
            dataGridViewBooks.Columns["quantityStore"].Width = 140;
        }
        private void ToDoListColumnHeaders()
        {
            dgvToDoList.Columns["maPhieu"].HeaderText = "Mã Phiếu";
            dgvToDoList.Columns["book_id"].HeaderText = "Mã Sách";
            dgvToDoList.Columns["user_id"].Visible = false;
            dgvToDoList.Columns["thoiGianTaoPhieu"].HeaderText = "Lúc Tạo Phiếu";
            dgvToDoList.Columns["thoiGianThucHien"].Visible = false;
            dgvToDoList.Columns["soLuong"].HeaderText = "Số Lượng";
            dgvToDoList.Columns["trangThai"].HeaderText = "Trạng Thái";
            dgvToDoList.Columns["hinhThuc"].HeaderText = "Hình Thức";

            dgvToDoList.ColumnHeadersDefaultCellStyle.Font = new Font(dgvToDoList.Font, FontStyle.Bold);
            dgvToDoList.RowHeadersVisible = false;
            dgvToDoList.AllowUserToResizeColumns = false;
            dgvToDoList.AllowUserToResizeRows = false;

            dgvToDoList.Columns["maPhieu"].Width = 170;
            dgvToDoList.Columns["book_id"].Width = 140;
            dgvToDoList.Columns["thoiGianTaoPhieu"].Width = 170;
            dgvToDoList.Columns["soLuong"].Width = 110;
            dgvToDoList.Columns["trangThai"].Width = 120;
            dgvToDoList.Columns["hinhThuc"].Width = 147;

        }

//Hàm tìm kiếm
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

        
//Con trỏ của dataDridView
        private void dataGridViewBooks_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBooks.SelectedRows.Count > 0)
            {

                int selectedRowIndex = dataGridViewBooks.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridViewBooks.Rows[selectedRowIndex];

                ttTenSach.Text = selectedRow.Cells["book_name"].Value.ToString();
                ttTheLoai.Text = selectedRow.Cells["category"].Value.ToString();
                ttNXB.Text = selectedRow.Cells["publishing"].Value.ToString();
                ttDonGia.Text = selectedRow.Cells["price"].Value.ToString();
                slTaiKe.Text = selectedRow.Cells["quantityShelf"].Value.ToString();
                slTaiKho.Text = selectedRow.Cells["quantityStore"].Value.ToString();

            }
        }
        private void dgvToDoList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvToDoList.SelectedRows.Count > 0)
            {

                int selectedRowIndex = dgvToDoList.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dgvToDoList.Rows[selectedRowIndex];

                ttMaPhieu.Text = selectedRow.Cells["maPhieu"].Value.ToString();
                ttMaSach.Text = selectedRow.Cells["book_id"].Value.ToString();
                dtpThoiGianTao.Text = selectedRow.Cells["thoiGianTaoPhieu"].Value.ToString();
                ttSoLuong.Text = selectedRow.Cells["soLuong"].Value.ToString();
                ttHinhThuc.Text = selectedRow.Cells["hinhThuc"].Value.ToString();

            }
        }
//Các nút
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

        private void btnThucHien_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thực hiện thao tác này không?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                string maPhieu = ttMaPhieu.Text;
                string maSach = ttMaSach.Text;
                int soLuong = int.Parse(ttSoLuong.Text);
                string hinhThuc = ttHinhThuc.Text;

                if (hinhThuc == "Xuất kho")
                {
                    try
                    {
                        string selectQuery = "SELECT quantityStore, quantityShelf FROM books WHERE book_id = @book_id";
                        SqlParameter[] selectParams = { new SqlParameter("@book_id", maSach) };

                        DataTable bookData = kn.GetDataTable(selectQuery, selectParams);

                        if (bookData.Rows.Count > 0)
                        {
                            int quantityStore = Convert.ToInt32(bookData.Rows[0]["quantityStore"]);
                            int quantityShelf = Convert.ToInt32(bookData.Rows[0]["quantityShelf"]);

                            if (quantityStore >= soLuong)
                            {
                                int newQuantityStore = quantityStore - soLuong;
                                int newQuantityShelf = quantityShelf + soLuong;

                                string updateBooksQuery = "UPDATE books SET quantityStore = @quantityStore, quantityShelf = @quantityShelf WHERE book_id = @book_id";
                                SqlParameter[] updateBooksParams = {
                                    new SqlParameter("@quantityStore", newQuantityStore),
                                    new SqlParameter("@quantityShelf", newQuantityShelf),
                                    new SqlParameter("@book_id", maSach)
                                };
                                kn.ExecuteQuery(updateBooksQuery, updateBooksParams);

                                string updatePhieuQuery = "UPDATE PhieuXuatKho SET trangThai = 'Đã làm' WHERE maPhieu = @maPhieu";
                                SqlParameter[] updatePhieuParams = { new SqlParameter("@maPhieu", maPhieu) };
                                kn.ExecuteQuery(updatePhieuQuery, updatePhieuParams);

                                MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadToDoList();
                            }
                            else
                            {
                                MessageBox.Show("Số lượng trong kho không đủ để xuất!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    try
                    {
                        string selectQuery = "SELECT quantityStore, quantityShelf FROM books WHERE book_id = @book_id";
                        SqlParameter[] selectParams = { new SqlParameter("@book_id", maSach) };

                        DataTable bookData = kn.GetDataTable(selectQuery, selectParams);

                        if (bookData.Rows.Count > 0)
                        {
                            int quantityStore = Convert.ToInt32(bookData.Rows[0]["quantityStore"]);
                            int quantityShelf = Convert.ToInt32(bookData.Rows[0]["quantityShelf"]);

                            if (quantityStore >= soLuong)
                            {
                                int newQuantityStore = quantityStore + soLuong;
                                int newQuantityShelf = quantityShelf - soLuong;

                                string updateBooksQuery = "UPDATE books SET quantityStore = @quantityStore, quantityShelf = @quantityShelf WHERE book_id = @book_id";
                                SqlParameter[] updateBooksParams = {
                                    new SqlParameter("@quantityStore", newQuantityStore),
                                    new SqlParameter("@quantityShelf", newQuantityShelf),
                                    new SqlParameter("@book_id", maSach)
                                };
                                kn.ExecuteQuery(updateBooksQuery, updateBooksParams);

                                SqlParameter[] updatePhieuParams = { new SqlParameter("@maPhieu", maPhieu) };

                                MessageBox.Show("Thao tác thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                LoadToDoList();
                            }
                            else
                            {
                                MessageBox.Show("Đã vượt quá số lượng trên kệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            loginForm.Show();
            this.Close();
        }
    }
}
