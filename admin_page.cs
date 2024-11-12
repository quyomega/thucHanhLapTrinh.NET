using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Cryptography;
using System.Windows.Forms;
//test2
namespace baitaplon
{
    public partial class admin_page : Form
    {
        private ketnoi kn = new ketnoi();
        private LoginForm loginForm; 
        private string selectedBookId;

        public admin_page(LoginForm form)
        {
            InitializeComponent();
            this.loginForm = form;
            LoadBooks();
            LoadInvoices();
            LoadStaffData();
            SetupEventHandlers();
            LoadDeliveryData();
        }

        private void SetupEventHandlers()
        {
            txtName.TextChanged += txtSearch_TextChanged;
            txtCategory.TextChanged += SearchCriteriaChanged;
            txtPublisher.TextChanged += SearchCriteriaChanged;
            txtPrice.TextChanged += SearchCriteriaChanged;
            cmbLocation.SelectedIndexChanged += SearchCriteriaChanged;
            searchTimer.Tick += searchTimer_Tick;
            dataGridViewBooks.SelectionChanged += dataGridViewBooks_SelectionChanged;

            txtMaHoaDon.TextChanged += InvoiceSearchCriteriaChanged;
            txtTenKH.TextChanged += InvoiceSearchCriteriaChanged;
            txtMaSach.TextChanged += InvoiceSearchCriteriaChanged;
            txtTenNhanVien.TextChanged += InvoiceSearchCriteriaChanged;
            txtTongGia.TextChanged += InvoiceSearchCriteriaChanged;
            dtpTuNgay.ValueChanged += InvoiceSearchCriteriaChanged;
            dtpDenNgay.ValueChanged += InvoiceSearchCriteriaChanged;
            searchTimer2.Tick += searchTimer2_Tick;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }
        private void SearchCriteriaChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }
        private void InvoiceSearchCriteriaChanged(object sender, EventArgs e)
        {
            searchTimer2.Stop();
            searchTimer2.Start();
        }
        private void searchTimer_Tick(object sender, EventArgs e)
        {
            searchTimer.Stop();
            SearchBooks();
        }
       
        private void searchTimer2_Tick(object sender, EventArgs e)
        {
            searchTimer2.Stop();
            SearchInvoices();
        }
        private void LoadBooks()
        {
            string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books";
            DataTable booksTable = kn.GetDataTable(query);

            dataGridViewBooks.DataSource = booksTable;
            SetBookColumnHeaders();
        }

        private void LoadInvoices()
        {
            string query = "SELECT MaHoadon, TenKhachHang, MaSach, TenNhanVien, DonGia, SoLuong, TongGia, ThoiGianBan FROM Hoadon";
            DataTable HoadonTable = kn.GetDataTable(query);

            dataGridViewInvoices.DataSource = HoadonTable;
            SetInvoiceColumnHeaders();
        }

        private void LoadStaffData()
        {
            string query = "SELECT user_id, TenNhanVien, Email, Phone, Address FROM users";
            DataTable staffTable = kn.GetDataTable(query);

            dataGridViewStaff.DataSource = staffTable;
            SetStaffColumnHeaders();
        }
        private void LoadDeliveryData()
        {
            // Truy vấn dữ liệu từ bảng PhieuXuatKho
            string query = "SELECT maPhieu, book_id, user_id, thoiGianTaoPhieu, thoiGianThucHien, soLuong, trangThai FROM PhieuXuatKho";

            // Lấy dữ liệu từ database
            DataTable deliveryTable = kn.GetDataTable(query);

            // Đưa dữ liệu vào DataGridView
            dataGridViewDelivery.DataSource = deliveryTable;

            // Thiết lập tiêu đề cột
            SetDeliveryColumnHeaders();
        }

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
            
            dataGridViewBooks.Columns["book_id"].Width = 99;
            dataGridViewBooks.Columns["book_name"].Width = 140;
            dataGridViewBooks.Columns["category"].Width = 120;
            dataGridViewBooks.Columns["publishing"].Width = 150;
            dataGridViewBooks.Columns["price"].Width = 90;
            dataGridViewBooks.Columns["quantityShelf"].Width = 120;
            dataGridViewBooks.Columns["quantityStore"].Width = 123;
        }

        private void SetInvoiceColumnHeaders()
        {
            dataGridViewInvoices.Columns["MaHoadon"].HeaderText = "Mã HD";
            dataGridViewInvoices.Columns["TenKhachHang"].HeaderText = "Khách Hàng";
            dataGridViewInvoices.Columns["MaSach"].HeaderText = "Mã Sách";
            dataGridViewInvoices.Columns["TenNhanVien"].HeaderText = "Nhân Viên";
            dataGridViewInvoices.Columns["DonGia"].HeaderText = "Đơn Giá";
            dataGridViewInvoices.Columns["SoLuong"].HeaderText = "Số Lượng";
            dataGridViewInvoices.Columns["TongGia"].HeaderText = "Tổng Giá";
            dataGridViewInvoices.Columns["ThoiGianBan"].HeaderText = "Thời Gian";
            dataGridViewInvoices.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewInvoices.Font, FontStyle.Bold);
            
            dataGridViewInvoices.Columns["MaHoadon"].Width = 70;
            dataGridViewInvoices.Columns["TenKhachHang"].Width = 150;
            dataGridViewInvoices.Columns["MaSach"].Width = 100;
            dataGridViewInvoices.Columns["TenNhanVien"].Width = 130;
            dataGridViewInvoices.Columns["DonGia"].Width = 90;
            dataGridViewInvoices.Columns["SoLuong"].Width = 90;
            dataGridViewInvoices.Columns["TongGia"].Width = 100;
            dataGridViewInvoices.Columns["ThoiGianBan"].Width = 114;
        }

        private void SetStaffColumnHeaders()
        {
            dataGridViewStaff.Columns["user_id"].HeaderText = "Mã Nhân Viên";
            dataGridViewStaff.Columns["TenNhanVien"].HeaderText = "Tên Nhân Viên";
            dataGridViewStaff.Columns["Email"].HeaderText = "Email";
            dataGridViewStaff.Columns["Phone"].HeaderText = "Số Điện Thoại";
            dataGridViewStaff.Columns["Address"].HeaderText = "Địa Chỉ";
            dataGridViewStaff.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewStaff.Font, FontStyle.Bold);

            dataGridViewStaff.Columns["user_id"].Width = 150;
            dataGridViewStaff.Columns["TenNhanVien"].Width = 180;
            dataGridViewStaff.Columns["Email"].Width = 190;
            dataGridViewStaff.Columns["Phone"].Width = 140;
            dataGridViewStaff.Columns["Address"].Width = 188;
        }
        private void SetDeliveryColumnHeaders()
        {
            dataGridViewDelivery.Columns["maPhieu"].HeaderText = "Mã Phiếu";
            dataGridViewDelivery.Columns["book_id"].HeaderText = "Mã Sách";
            dataGridViewDelivery.Columns["user_id"].HeaderText = "Mã Nhân Viên";
            dataGridViewDelivery.Columns["thoiGianTaoPhieu"].HeaderText = "Thời Gian Tạo Phiếu";
            dataGridViewDelivery.Columns["thoiGianThucHien"].HeaderText = "Thời Gian Thực Hiện";
            dataGridViewDelivery.Columns["soLuong"].HeaderText = "Số Lượng";
            dataGridViewDelivery.Columns["trangThai"].HeaderText = "Trạng Thái";

            dataGridViewDelivery.ColumnHeadersDefaultCellStyle.Font = new Font(dataGridViewDelivery.Font, FontStyle.Bold);
            dataGridViewDelivery.Columns["maPhieu"].Width = 80;
            dataGridViewDelivery.Columns["book_id"].Width = 100;
            dataGridViewDelivery.Columns["user_id"].Width = 100;
            dataGridViewDelivery.Columns["thoiGianTaoPhieu"].Width = 170;
            dataGridViewDelivery.Columns["thoiGianThucHien"].Width = 170;
            dataGridViewDelivery.Columns["soLuong"].Width = 120;
            dataGridViewDelivery.Columns["trangThai"].Width = 117;
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
                query += " AND price LIKE @price";
                parameters.Add(new SqlParameter("@price", price));
            }
            string selectedLocation = cmbLocation.SelectedItem?.ToString();
            if (selectedLocation == "Tại Kệ")
                query += " AND quantityShelf > 0";
            else if (selectedLocation == "Tại Kho")
                query += " AND quantityStore > 0";

            DataTable booksTable = kn.GetDataTable(query, parameters.ToArray());
            dataGridViewBooks.DataSource = booksTable;
        }
        private void SearchInvoices()
        {
            string query = "SELECT MaHoadon, TenKhachHang, MaSach, TenNhanVien, DonGia, SoLuong, TongGia, ThoiGianBan FROM HoaDon WHERE 1=1";
            List<SqlParameter> parameters1 = new List<SqlParameter>();

            if (!string.IsNullOrWhiteSpace(txtMaHoaDon.Text))
            {
                query += " AND MaHoadon LIKE @MaHoaDon";
                parameters1.Add(new SqlParameter("@MaHoaDon", "%" + txtMaHoaDon.Text + "%"));
            }
            if (!string.IsNullOrWhiteSpace(txtTenKH.Text))
            {
                query += " AND TenKhachHang LIKE @TenKH";
                parameters1.Add(new SqlParameter("@TenKH", "%" + txtTenKH.Text + "%"));
            }
            if (!string.IsNullOrWhiteSpace(txtMaSach.Text))
            {
                query += " AND MaSach LIKE @MaSach";
                parameters1.Add(new SqlParameter("@MaSach", "%" + txtMaSach.Text + "%"));
            }
            if (!string.IsNullOrWhiteSpace(txtTenNhanVien.Text))
            {
                query += " AND TenNhanVien LIKE @TenNhanVien";
                parameters1.Add(new SqlParameter("@TenNhanVien", "%" + txtTenNhanVien.Text + "%"));
            }
            if (!string.IsNullOrWhiteSpace(txtTongGia.Text))
            {
                if (decimal.TryParse(txtTongGia.Text, out decimal tongGia))
                {
                    query += " AND TongGia = @TongGia";
                    parameters1.Add(new SqlParameter("@TongGia", tongGia));
                }
            }
            if (dtpTuNgay.Value != DateTimePicker.MinimumDateTime)
            {
                query += " AND ThoiGianBan >= @TuNgay";
                parameters1.Add(new SqlParameter("@TuNgay", dtpTuNgay.Value));
            }
            if (dtpDenNgay.Value != DateTimePicker.MinimumDateTime)
            {
                query += " AND ThoiGianBan <= @DenNgay";
                parameters1.Add(new SqlParameter("@DenNgay", dtpDenNgay.Value));
            }

            DataTable HoadonTable = kn.GetDataTable(query, parameters1.ToArray());
            dataGridViewInvoices.DataSource = HoadonTable;
        }
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }
        private void dataGridViewBooks_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewBooks.SelectedRows.Count > 0)
            {
                selectedBookId = dataGridViewBooks.SelectedRows[0].Cells["book_id"].Value.ToString();

                DataGridViewRow selectedRow = dataGridViewBooks.SelectedRows[0];

                txtTenSach.Text = selectedRow.Cells["book_name"].Value.ToString();
                txtTheLoai.Text = selectedRow.Cells["category"].Value.ToString();
                txtNhaXuatBan.Text = selectedRow.Cells["publishing"].Value.ToString();
                txtGia.Text = selectedRow.Cells["price"].Value.ToString();
                txtTaiKe.Text = selectedRow.Cells["quantityShelf"].Value.ToString();
                txtTaiKho.Text = selectedRow.Cells["quantityStore"].Value.ToString();

            }
        }
        private void dataGridViewInvoices_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewInvoices.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridViewInvoices.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridViewInvoices.Rows[selectedRowIndex];

                ttMaHoaDon.Text = selectedRow.Cells["MaHoaDon"].Value.ToString();
                ttTenKH.Text = selectedRow.Cells["TenKhachHang"].Value.ToString();
                ttMaSach.Text = selectedRow.Cells["MaSach"].Value.ToString();
                ttTenNV.Text = selectedRow.Cells["TenNhanVien"].Value.ToString();
                ttDonGia.Text = selectedRow.Cells["DonGia"].Value.ToString();
                ttSoLuong.Text = selectedRow.Cells["SoLuong"].Value.ToString();
                dtpThoiGian.Text = selectedRow.Cells["ThoiGianBan"].Value.ToString();
                ttTongGia.Text = selectedRow.Cells["TongGia"].Value.ToString();
            }
        }
        private void dataGridViewStaff_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewStaff.SelectedCells.Count > 0)
            {
                int selectedRowIndex = dataGridViewStaff.SelectedCells[0].RowIndex;

                DataGridViewRow selectedRow = dataGridViewStaff.Rows[selectedRowIndex];

                txtMaNV.Text = selectedRow.Cells["user_id"].Value.ToString();
                txtTenNV.Text = selectedRow.Cells["TenNhanVien"].Value.ToString();
                txtEmailNV.Text = selectedRow.Cells["Email"].Value.ToString();
                txtSDTNV.Text = selectedRow.Cells["Phone"].Value.ToString();
                txtDiaChiNV.Text = selectedRow.Cells["Address"].Value.ToString();
            }
        }

        private void btnThemNV_Click(object sender, EventArgs e)
        {
                string maNV = txtMaNV.Text;
                string tenNV = txtTenNV.Text;
                string email = txtEmailNV.Text;
                string phone = txtSDTNV.Text;
                string address = txtDiaChiNV.Text;

                if (string.IsNullOrEmpty(maNV) || string.IsNullOrEmpty(tenNV) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                    return;
                }

                string query = "INSERT INTO users (user_id, TenNhanVien, Email, Phone, Address) VALUES (@user_id, @TenNhanVien, @Email, @Phone, @Address)";
                List<SqlParameter> parameters = new List<SqlParameter>
                    {
                        new SqlParameter("@user_id", maNV),
                        new SqlParameter("@TenNhanVien", tenNV),
                        new SqlParameter("@Email", email),
                        new SqlParameter("@Phone", phone),
                        new SqlParameter("@Address", address)
                    };
            kn.ExecuteQuery(query, parameters.ToArray());
            MessageBox.Show("Thêm nhân viên thành công!");
            LoadStaffData();
        }

        private void btnSuaNV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để sửa.");
                return;
            }

            string maNV = txtMaNV.Text;
            string tenNV = txtTenNV.Text;
            string email = txtEmailNV.Text;
            string phone = txtSDTNV.Text;
            string address = txtDiaChiNV.Text;

            string query = "UPDATE users SET TenNhanVien = @TenNhanVien, Email = @Email, Phone = @Phone, Address = @Address WHERE user_id = @user_id";
            List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@user_id", maNV),
                    new SqlParameter("@TenNhanVien", tenNV),
                    new SqlParameter("@Email", email),
                    new SqlParameter("@Phone", phone),
                    new SqlParameter("@Address", address)
                };

            kn.ExecuteQuery(query, parameters.ToArray());
            MessageBox.Show("Cập nhật nhân viên thành công!");
            LoadStaffData();
        }

        private void btnXoaNV_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaNV.Text))
            {
                MessageBox.Show("Vui lòng chọn nhân viên để xóa.");
                return;
            }

            string maNV = txtMaNV.Text;

            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên này?", "Xóa Nhân Viên", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                string query = "DELETE FROM users WHERE user_id = @user_id";
                List<SqlParameter> parameters = new List<SqlParameter>
        {
            new SqlParameter("@user_id", maNV)
        };

                kn.ExecuteQuery(query, parameters.ToArray());
                MessageBox.Show("Xóa nhân viên thành công!");
                LoadStaffData();  // Tải lại dữ liệu sau khi xóa
            }
        }

        private void btnCapNhatThongTinSach_Click(object sender, EventArgs e)
        {
            // Kiểm tra xem đã chọn sách hay chưa
            if (string.IsNullOrEmpty(selectedBookId))
            {
                MessageBox.Show("Vui lòng chọn một sách để cập nhật.");
                return;
            }

            // Lấy dữ liệu từ các TextBox
            string bookName = txtTenSach.Text;
            string category = txtTheLoai.Text;
            string publishing = txtNhaXuatBan.Text;
            string priceText = txtGia.Text;

            // Kiểm tra dữ liệu đầu vào
            if (string.IsNullOrEmpty(bookName) || string.IsNullOrEmpty(category) ||
                string.IsNullOrEmpty(publishing) || string.IsNullOrEmpty(priceText) || !decimal.TryParse(priceText, out decimal price))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin và giá phải là số hợp lệ.");
                return;
            }

            // Câu truy vấn cập nhật
            string query = "UPDATE books SET book_name = @book_name, category = @category, publishing = @publishing, price = @price WHERE book_id = @book_id";
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@book_name", bookName),
                new SqlParameter("@category", category),
                new SqlParameter("@publishing", publishing),
                new SqlParameter("@price", price),
                new SqlParameter("@book_id", selectedBookId)
            };

            // Thực thi truy vấn
            kn.ExecuteQuery(query, parameters.ToArray());
            MessageBox.Show("Cập nhật thông tin sách thành công!");

            // Tải lại dữ liệu sách để cập nhật giao diện
            LoadBooks();
        }
        private void btnThemSach_Click_1(object sender, EventArgs e)
        {
            ThemSachForm themSachForm = new ThemSachForm();
            themSachForm.ShowDialog();
        }
        private void btnPhieuXuatKho_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBookId))
            {
                MessageBox.Show("Vui lòng chọn một sách từ danh sách.");
                return;
            }

            PhieuXuatKhoForm phieuXuatKhoForm = new PhieuXuatKhoForm(selectedBookId);
            phieuXuatKhoForm.ShowDialog();
        }
        private void btnPhieuNhapKho_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(selectedBookId))
            {
                MessageBox.Show("Vui lòng chọn một sách từ danh sách.");
                return;
            }

            PhieuNhapKhoForm phieuNhapKhoForm = new PhieuNhapKhoForm(selectedBookId);
            phieuNhapKhoForm.ShowDialog();
        }
    }
}
