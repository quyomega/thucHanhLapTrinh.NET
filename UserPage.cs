using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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

            LoadNhapKhoData();  // Tải dữ liệu vào dgvNhapKho
            LoadXuatKhoData();  // Tải dữ liệu vào dgvXuatKho

            LoadNhapKhoData_DaLam(); 
            LoadXuatKhoData_DaLam();

            txtName.TextChanged += SearchCriteriaChanged;
            txtCategory.TextChanged += SearchCriteriaChanged;
            txtPublisher.TextChanged += SearchCriteriaChanged;
            txtPrice.TextChanged += SearchCriteriaChanged;

            cmbLocation.SelectedIndexChanged += SearchCriteriaChanged;
            // Sự kiện Tick cho Timer tìm kiếm
            searchTimer.Tick += searchTimer_Tick;
            searchTimer.Interval = 500;

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

        //Việc Cần làm
        private void LoadNhapKhoData()
        {
            DataTable allData = kn.GetDataTable("SELECT * FROM PhieuXuatKho");

            // Loại bỏ khoảng trắng ở đầu và cuối cho các cột cần thiết
            foreach (DataRow row in allData.Rows)
            {
                row["trangThai"] = row["trangThai"].ToString().Trim();
                row["hinhThuc"] = row["hinhThuc"].ToString().Trim();
            }

            // Lọc dữ liệu cho Nhập Kho theo user_id của người dùng hiện tại
            var nhapKhoRows = allData.AsEnumerable()
                .Where(r => r.Field<string>("trangThai") == "Chưa làm"
                            && r.Field<string>("hinhThuc") == "Nhập kho"
                            && r.Field<int>("user_id") == this.userId);

            DataTable nhapKhoTable;

            if (nhapKhoRows.Any())
            {
                nhapKhoTable = nhapKhoRows.CopyToDataTable();
            }
            else
            {
                // Tạo bảng trống với cùng cấu trúc nếu không có dữ liệu
                nhapKhoTable = allData.Clone(); // Clone để lấy cấu trúc cột mà không có dữ liệu
            }

            dgvNhapKho.DataSource = nhapKhoTable;
        }

        private void btnNhapKho_Click(object sender, EventArgs e)
        {
            if (dgvNhapKho.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvNhapKho.SelectedRows)
                {
                    int maPhieu = Convert.ToInt32(row.Cells["maPhieu"].Value);
                    int bookId = Convert.ToInt32(row.Cells["book_id"].Value);
                    int soLuong = Convert.ToInt32(row.Cells["soLuong"].Value);

                    string updatePhieuQuery = "UPDATE PhieuXuatKho SET trangThai = 'Đã làm', thoiGianThucHien = GETDATE() WHERE maPhieu = @maPhieu";
                    SqlParameter[] phieuParams = new SqlParameter[]
                    {
                        new SqlParameter("@maPhieu", maPhieu)
                    };
                    kn.ExecuteQuery(updatePhieuQuery, phieuParams);

                    string updateBooksQuery = "UPDATE books SET quantityStore = quantityStore + @soLuong WHERE book_id = @bookId";
                    SqlParameter[] booksParams = new SqlParameter[]
                    {
                        new SqlParameter("@soLuong", soLuong),
                        new SqlParameter("@bookId", bookId)
                    };
                    kn.ExecuteQuery(updateBooksQuery, booksParams);
                }
                LoadNhapKhoData(); // Tải lại dữ liệu để cập nhật trên dgvNhapKho
                MessageBox.Show("Bạn thực hiện nhập kho thành công.");
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu nhập kho để thực hiện.");
            }
        }

        private void LoadXuatKhoData()
        {
            DataTable allData = kn.GetDataTable("SELECT * FROM PhieuXuatKho");

            // Loại bỏ khoảng trắng ở đầu và cuối cho các cột cần thiết
            foreach (DataRow row in allData.Rows)
            {
                row["trangThai"] = row["trangThai"].ToString().Trim();
                row["hinhThuc"] = row["hinhThuc"].ToString().Trim();
            }

            // Lọc dữ liệu cho Xuất Kho theo user_id của người dùng hiện tại
            var xuatKhoRows = allData.AsEnumerable()
                .Where(r => r.Field<string>("trangThai") == "Chưa làm"
                            && r.Field<string>("hinhThuc") == "Xuất kho"
                            && r.Field<int>("user_id") == this.userId);

            DataTable xuatKhoTable;

            if (xuatKhoRows.Any())
            {
                xuatKhoTable = xuatKhoRows.CopyToDataTable();
            }
            else
            {
                // Tạo bảng trống với cùng cấu trúc nếu không có dữ liệu
                xuatKhoTable = allData.Clone(); // Clone để lấy cấu trúc cột mà không có dữ liệu
            }

            dgvXuatKho.DataSource = xuatKhoTable;
        }


        private void btnXuatKho_Click(object sender, EventArgs e)
        {
            if (dgvXuatKho.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvXuatKho.SelectedRows)
                {
                    int maPhieu = Convert.ToInt32(row.Cells["maPhieu"].Value);
                    int bookId = Convert.ToInt32(row.Cells["book_id"].Value);
                    int soLuong = Convert.ToInt32(row.Cells["soLuong"].Value);

                    string updatePhieuQuery = "UPDATE PhieuXuatKho SET trangThai = 'Đã làm', thoiGianThucHien = GETDATE() WHERE maPhieu = @maPhieu";
                    SqlParameter[] phieuParams = new SqlParameter[]
                    {
                        new SqlParameter("@maPhieu", maPhieu)
                    };
                    kn.ExecuteQuery(updatePhieuQuery, phieuParams);

                    string updateBooksQuery = "UPDATE books SET quantityShelf = quantityShelf + @soLuong, quantityStore = quantityStore - @soLuong WHERE book_id = @bookId";
                    SqlParameter[] booksParams = new SqlParameter[]
                    {
                        new SqlParameter("@soLuong", soLuong),
                        new SqlParameter("@bookId", bookId)
                    };
                    kn.ExecuteQuery(updateBooksQuery, booksParams);
                }
                LoadXuatKhoData(); // Tải lại dữ liệu để cập nhật trên dgvXuatKho
                MessageBox.Show("Bạn thực hiện xuất kho thành công.");
            }
            else
            {
                MessageBox.Show("Vui lòng chọn phiếu xuất kho để thực hiện.");
            }
        }

        //Việc Đã làm
        //Nhập Kho
        private void LoadNhapKhoData_DaLam()
        {
            try
            {
                DataTable allData = kn.GetDataTable("SELECT * FROM PhieuXuatKho");

                // Loại bỏ khoảng trắng ở đầu và cuối cho các cột cần thiết
                foreach (DataRow row in allData.Rows)
                {
                    row["trangThai"] = row["trangThai"].ToString().Trim();
                    row["hinhThuc"] = row["hinhThuc"].ToString().Trim();
                }

                // Lọc dữ liệu cho Nhập Kho (Đã làm) theo user_id của người dùng hiện tại
                var nhapKhoRows = allData.AsEnumerable()
                    .Where(r => r.Field<string>("trangThai") == "Đã làm"
                                && r.Field<string>("hinhThuc") == "Nhập kho"
                                && r.Field<int>("user_id") == this.userId);

                DataTable nhapKhoTable;

                if (nhapKhoRows.Any())
                {
                    nhapKhoTable = nhapKhoRows.CopyToDataTable();
                }
                else
                {
                    nhapKhoTable = allData.Clone();
                }

                dgvNhapKho2.DataSource = nhapKhoTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Nhập Kho (Đã làm): " + ex.Message);
            }
        }


        private void txtmaPhieu_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void txtmaSach_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void txtmaNV_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void dtimeTaoPhieu_ValueChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void dtimThucHien_ValueChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        //Xuất Kho
        private void LoadXuatKhoData_DaLam()
        {
            try
            {
                DataTable allData = kn.GetDataTable("SELECT * FROM PhieuXuatKho");

                // Loại bỏ khoảng trắng ở đầu và cuối cho các cột cần thiết
                foreach (DataRow row in allData.Rows)
                {
                    row["trangThai"] = row["trangThai"].ToString().Trim();
                    row["hinhThuc"] = row["hinhThuc"].ToString().Trim();
                }

                // Lọc dữ liệu cho Xuất Kho (Đã làm) theo user_id của người dùng hiện tại
                var xuatKhoRows = allData.AsEnumerable()
                    .Where(r => r.Field<string>("trangThai") == "Đã làm"
                                && r.Field<string>("hinhThuc") == "Xuất kho"
                                && r.Field<int>("user_id") == this.userId);

                DataTable xuatKhoTable;

                if (xuatKhoRows.Any())
                {
                    xuatKhoTable = xuatKhoRows.CopyToDataTable();
                }
                else
                {
                    xuatKhoTable = allData.Clone();
                }

                dgvXuatKho2.DataSource = xuatKhoTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu Xuất Kho (Đã làm): " + ex.Message);
            }
        }


        private void txtmaPhieu2_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void txtmaSach2_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void txtMaNV2_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void dtimeTaoPhieu2_ValueChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void dtimeThucHien2_ValueChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void txtSoLuong2_TextChanged(object sender, EventArgs e)
        {
            SearchCompletedTasks();
        }

        private void SearchCompletedTasks()
        {
            string query = "SELECT maPhieu, book_id, user_id, thoiGianTaoPhieu, thoiGianThucHien, soLuong, trangThai, hinhThuc FROM PhieuXuatKho WHERE trangThai = 'Đã làm'";
            List<SqlParameter> parameters = new List<SqlParameter>();

            if (!string.IsNullOrEmpty(txtmaPhieu2.Text))
            {
                query += " AND maPhieu = @maPhieu";
                parameters.Add(new SqlParameter("@maPhieu", txtmaPhieu2.Text));
            }
            if (!string.IsNullOrEmpty(txtmaSach2.Text))
            {
                query += " AND book_id = @bookId";
                parameters.Add(new SqlParameter("@bookId", txtmaSach2.Text));
            }
            if (!string.IsNullOrEmpty(txtMaNV2.Text))
            {
                query += " AND user_id = @userId";
                parameters.Add(new SqlParameter("@userId", txtMaNV2.Text));
            }
            if (dtimeTaoPhieu2.Value != DateTime.MinValue)
            {
                query += " AND CONVERT(date, thoiGianTaoPhieu) = @thoiGianTaoPhieu";
                parameters.Add(new SqlParameter("@thoiGianTaoPhieu", dtimeTaoPhieu2.Value.Date));
            }
            if (dtimeThucHien2.Value != DateTime.MinValue)
            {
                query += " AND CONVERT(date, thoiGianThucHien) = @thoiGianThucHien";
                parameters.Add(new SqlParameter("@thoiGianThucHien", dtimeThucHien2.Value.Date));
            }
            if (!string.IsNullOrEmpty(txtSoLuong2.Text))
            {
                query += " AND soLuong = @soLuong";
                parameters.Add(new SqlParameter("@soLuong", txtSoLuong2.Text));
            }

            try
            {
                DataTable resultTable = kn.GetDataTable(query, parameters.ToArray());

                // Kiểm tra và gán dữ liệu nếu có kết quả
                if (resultTable.Rows.Count > 0)
                {
                    var nhapKhoRows = resultTable.AsEnumerable().Where(r => r.Field<string>("hinhThuc") == "Nhập kho");
                    dgvNhapKho2.DataSource = nhapKhoRows.Any() ? nhapKhoRows.CopyToDataTable() : null;

                    var xuatKhoRows = resultTable.AsEnumerable().Where(r => r.Field<string>("hinhThuc") == "Xuất kho");
                    dgvXuatKho2.DataSource = xuatKhoRows.Any() ? xuatKhoRows.CopyToDataTable() : null;
                }
                else
                {
                    dgvNhapKho2.DataSource = null;
                    dgvXuatKho2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message);
            }
        }

    }
}
