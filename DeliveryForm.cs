using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class DeliveryForm : Form
    {
        private Connect kn = new Connect();
        private string selectedBookId;
        private string selectedUserId;

        public DeliveryForm(string bookId)
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
        private void LoadHinhThucData()
        {
            // Thêm các giá trị vào ComboBox
            cmbHinhThuc.Items.Add("Xuất kho");
            cmbHinhThuc.Items.Add("Nhập kho");

            // Chọn giá trị mặc định nếu có
            cmbHinhThuc.SelectedIndex = 0; // Chọn "Bán" làm mặc định
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
            string hinhThuc = cmbHinhThuc.SelectedItem.ToString();  

            int quantityShelf = 0;
            int quantityStore = 0;

            string queryQuantity = "SELECT quantityShelf, quantityStore FROM books WHERE book_id = @book_id";
            SqlParameter paramBookId = new SqlParameter("@book_id", maSach);
            DataTable resultQuantity = kn.GetDataTable(queryQuantity, new SqlParameter[] { paramBookId });

            if (resultQuantity.Rows.Count > 0)
            {
                quantityShelf = Convert.ToInt32(resultQuantity.Rows[0]["quantityShelf"]);
                quantityStore = Convert.ToInt32(resultQuantity.Rows[0]["quantityStore"]);
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin sách!");
                return;
            }

            if (hinhThuc == "Nhập kho")
            {
                if (soLuong > quantityShelf)
                {
                    MessageBox.Show("Số lượng nhập kho không được lớn hơn số lượng trong kho bày!");
                    return;
                }
            }
            else if (hinhThuc == "Xuất kho")
            {
                if (soLuong > quantityStore)
                {
                    MessageBox.Show("Số lượng xuất kho không được lớn hơn số lượng trong kho!");
                    return;
                }
            }

            object thoiGianThucHien = DBNull.Value;

            string trangThai = "Chưa làm";

            string query = "INSERT INTO PhieuXuatKho (book_id, user_id, thoiGianTaoPhieu, thoiGianThucHien, soLuong, hinhThuc, trangThai) " +
                           "VALUES (@book_id, @user_id, @thoiGianTaoPhieu, @thoiGianThucHien, @soLuong, @hinhThuc, @trangThai)";

            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@book_id", maSach),
                new SqlParameter("@user_id", nhanVienId),
                new SqlParameter("@thoiGianTaoPhieu", DateTime.Now),
                new SqlParameter("@thoiGianThucHien", thoiGianThucHien),
                new SqlParameter("@soLuong", soLuong),
                new SqlParameter("@hinhThuc", hinhThuc),
                new SqlParameter("@trangThai", trangThai)  
            };
            kn.ExecuteQuery(query, parameters.ToArray());
            if(hinhThuc == "Nhập kho")
            {
                MessageBox.Show("Phiếu nhập kho đã được tạo thành công!");

            }
            else if (hinhThuc == "Xuất kho")
            {
                MessageBox.Show("Phiếu xuất kho đã được tạo thành công!");

            }
            this.Close();
        }
        private void PhieuXuatKhoForm_Load(object sender, EventArgs e)
        {
        }
    }
}
