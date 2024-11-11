using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class PhieuXuatKhoForm : Form
    {
        private int maPhieu; // Biến để lưu mã phiếu xuất kho
        private string maSach;
        public PhieuXuatKhoForm(string maSach)
        {
            InitializeComponent();
            SetDefaultValues();
            LoadStaff();
            txtMaSach.Text = maSach;// Thiết lập giá trị mặc định cho mã phiếu và ngày xuất kho
        }
        public PhieuXuatKhoForm()
        {
            InitializeComponent();
        }
        private void SetDefaultValues()
        {
            // Tạo mã phiếu xuất kho tự động (có thể sử dụng cách khác để tạo mã duy nhất nếu cần)
            maPhieu = GenerateMaPhieu();
            txtMaPhieu.Text = maPhieu.ToString(); // Giả sử txtMaPhieu là TextBox hiển thị mã phiếu
            dateTimePickerNgayXuat.Value = DateTime.Now; // Đặt ngày xuất kho là ngày hôm nay

            // Vô hiệu hóa TextBox cho mã phiếu và DateTimePicker
            txtMaSach.Enabled = false;
            txtMaPhieu.Enabled = false; // Vô hiệu hóa chỉnh sửa mã phiếu
            dateTimePickerNgayXuat.Enabled = false; // Vô hiệu hóa chỉnh sửa ngày xuất

        }
        private void LoadStaff()
        {
            string connectionString = "Data Source=DESKTOP-V71IBDD;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT username FROM [users] WHERE role = 'staff'";
                SqlCommand cmd = new SqlCommand(query, conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Giả sử bạn lưu username vào ComboBox
                        cmbNhanVien.Items.Add(reader["username"].ToString());
                    }
                }
            }
        }

        private int GenerateMaPhieu()
        {
            int newMaPhieu = 0; // Giá trị mặc định nếu bảng trống

            string connectionString = "Data Source=DESKTOP-V71IBDD;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT MAX(maPhieu) FROM PhieuXuatKho";
                SqlCommand cmd = new SqlCommand(query, conn);

                object result = cmd.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    newMaPhieu = Convert.ToInt32(result) + 1;
                }
            }

            return newMaPhieu;
        }

        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            string maSach = txtMaSach.Text;
            int soLuongXuat;
            DateTime ngayXuat = dateTimePickerNgayXuat.Value;
            string nhanVienThucHien = cmbNhanVien.SelectedItem?.ToString(); // Lấy tên nhân viên từ ComboBox

            // Kiểm tra xem tên nhân viên có được chọn không
            if (string.IsNullOrEmpty(nhanVienThucHien))
            {
                MessageBox.Show("Vui lòng chọn tên nhân viên.");
                return;
            }

            // Kiểm tra xem số lượng xuất có hợp lệ không
            if (!int.TryParse(txtSoLuong.Text, out soLuongXuat) || soLuongXuat <= 0)
            {
                MessageBox.Show("Số lượng phải là một số nguyên dương.");
                return;
            }

            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                SqlTransaction transaction = conn.BeginTransaction();
                try
                {
                    // Kiểm tra tên nhân viên có trong bảng users không
                    string queryCheckStaff = @"
                SELECT COUNT(*) 
                FROM [users] 
                WHERE username = @username AND role = 'staff'";

                    SqlCommand cmdCheckStaff = new SqlCommand(queryCheckStaff, conn, transaction);
                    cmdCheckStaff.Parameters.AddWithValue("@username", nhanVienThucHien);
                    int staffExists = (int)cmdCheckStaff.ExecuteScalar();

                    if (staffExists == 0)
                    {
                        MessageBox.Show("Tên nhân viên không hợp lệ. Vui lòng chọn lại.");
                        return; // Thoát khỏi phương thức nếu nhân viên không hợp lệ
                    }

                    // Kiểm tra số lượng trong kho trước khi tạo phiếu xuất kho
                    string queryCheckQuantity = @"
                SELECT quantityStore 
                FROM books 
                WHERE book_id = @maSach";

                    SqlCommand cmdCheckQuantity = new SqlCommand(queryCheckQuantity, conn, transaction);
                    cmdCheckQuantity.Parameters.AddWithValue("@maSach", maSach);

                    // Lấy giá trị quantityStore từ cơ sở dữ liệu
                    object result = cmdCheckQuantity.ExecuteScalar();
                    if (result == null || result == DBNull.Value)
                    {
                        MessageBox.Show("Sản phẩm không tồn tại.");
                        return; // Thoát nếu sản phẩm không tồn tại
                    }

                    int quantityStore = Convert.ToInt32(result);

                    // Kiểm tra số lượng tồn kho
                    if (quantityStore <= 0)
                    {
                        MessageBox.Show("Không thể tạo phiếu xuất kho vì sản phẩm đã hết hàng.");
                        return; // Thoát khỏi phương thức nếu không đủ hàng
                    }
                    else if (quantityStore < soLuongXuat)
                    {
                        MessageBox.Show("Số lượng xuất kho lớn hơn số lượng hiện có trong kho.");
                        return; // Thoát khỏi phương thức nếu số lượng xuất lớn hơn trong kho
                    }

                    // Ghi lại thông tin phiếu xuất kho vào bảng phiếu xuất
                    string queryLuuPhieu = @"
                INSERT INTO PhieuXuatKho (maPhieu, maSach, soLuongXuat, ngayXuat, nhanVienThucHien)
                VALUES (@maPhieu, @maSach, @soLuongXuat, @ngayXuat, @nhanVienThucHien)"; // Thêm nhanVien vào câu lệnh

                    SqlCommand cmdLuuPhieu = new SqlCommand(queryLuuPhieu, conn, transaction);
                    cmdLuuPhieu.Parameters.AddWithValue("@maPhieu", maPhieu);
                    cmdLuuPhieu.Parameters.AddWithValue("@maSach", maSach);
                    cmdLuuPhieu.Parameters.AddWithValue("@soLuongXuat", soLuongXuat);
                    cmdLuuPhieu.Parameters.AddWithValue("@ngayXuat", ngayXuat);
                    cmdLuuPhieu.Parameters.AddWithValue("@nhanVienThucHien", nhanVienThucHien); // Thêm tên nhân viên vào câu lệnh

                    cmdLuuPhieu.ExecuteNonQuery();

                    MessageBox.Show("Phiếu xuất kho đã được lưu thành công!");
                    transaction.Commit();
                    this.Close();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }


    }
}
