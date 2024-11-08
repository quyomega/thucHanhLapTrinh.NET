using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class PhieuXuatKhoForm : Form
    {
        private ketnoi kn = new ketnoi();
        private string selectedBookId;
        private string selectedUserId;

        public PhieuXuatKhoForm(string bookId)
        {
            InitializeComponent();
            this.selectedBookId = bookId;
            LoadStaffData();
            GenerateMaPhieu();
            SetBookIdInTextBox();
            SetDateTimePickerReadOnly();
        }

        private void LoadStaffData()
        {
            string query = "SELECT user_id, TenNhanVien FROM users";
            DataTable staffTable = kn.GetDataTable(query);

            cmbNhanVien.DataSource = staffTable;
            cmbNhanVien.DisplayMember = "TenNhanVien";
            cmbNhanVien.ValueMember = "user_id";
        }

        private void SetDateTimePickerReadOnly()
        {
            dateTimePickerNgayXuat.Enabled = false;
        }

        private void SetBookIdInTextBox()
        {
            txtMaSach.Text = selectedBookId;
        }

        private void GenerateMaPhieu()
        {
            string query = "SELECT ISNULL(MAX(maPhieu), 0) + 1 FROM PhieuXuatKho";
            DataTable result = kn.GetDataTable(query);

            if (result.Rows.Count > 0)
            {
                txtMaPhieu.Text = result.Rows[0][0].ToString();
            }
        }

        private void btnLuuPhieu_Click(object sender, EventArgs e)
        {
            string maPhieu = txtMaPhieu.Text;
            string maSach = selectedBookId;
            int soLuong = int.Parse(txtSoLuong.Text);
            DateTime ngayXuat = dateTimePickerNgayXuat.Value;
            string nhanVienId = cmbNhanVien.SelectedValue.ToString();

            // Kiểm tra số lượng có lớn hơn số lượng trong kho không
            string queryQuantity = "SELECT quantityStore FROM books WHERE book_id = @book_id";
            SqlParameter paramBookId = new SqlParameter("@book_id", maSach);
            DataTable resultQuantity = kn.GetDataTable(queryQuantity, new SqlParameter[] { paramBookId });

            if (resultQuantity.Rows.Count > 0)
            {
                int quantityStore = Convert.ToInt32(resultQuantity.Rows[0]["quantityStore"]);

                // Nếu số lượng nhập vào lớn hơn số lượng trong kho
                if (soLuong > quantityStore)
                {
                    MessageBox.Show("Số lượng xuất kho không được lớn hơn số lượng trong kho!");
                    return; // Dừng lại nếu số lượng không hợp lệ
                }
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin sách!");
                return;
            }

            // Đặt thoiGianThucHien là NULL khi chưa thực hiện
            object thoiGianThucHien = DBNull.Value;

            // Giá trị mặc định của trangThai là "Chưa làm"
            string trangThai = "Chưa làm";

            string query = "INSERT INTO PhieuXuatKho (book_id, user_id, thoiGianTaoPhieu, thoiGianThucHien, soLuong, trangThai) " +
                           "VALUES (@book_id, @user_id, @thoiGianTaoPhieu, @thoiGianThucHien, @soLuong, @trangThai)";

            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@book_id", maSach),
        new SqlParameter("@user_id", nhanVienId),
        new SqlParameter("@thoiGianTaoPhieu", DateTime.Now),
        new SqlParameter("@thoiGianThucHien", thoiGianThucHien), // Gửi giá trị NULL
        new SqlParameter("@soLuong", soLuong),
        new SqlParameter("@trangThai", trangThai) // Gửi giá trị trangThai là "Chưa làm"
    };

            kn.ExecuteQuery(query, parameters.ToArray());
            MessageBox.Show("Phiếu xuất kho đã được tạo thành công!");
            this.Close();
        }

        private void PhieuXuatKhoForm_Load(object sender, EventArgs e)
        {
        }
    }
}
