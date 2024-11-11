using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class ThemSachForm : Form
    {
        private ketnoi kn = new ketnoi();

        public ThemSachForm()
        {
            InitializeComponent();
        }

        private void btnThemSach_Click(object sender, EventArgs e)
        {
            int maSach;
            string tenSach = txtTenSach.Text;
            string theLoai = txtTheLoai.Text;
            string nhaXuatBan = txtNhaXuatBan.Text;
            int gia;
            int soLuong;

            if (!int.TryParse(txtMaSach.Text, out maSach))
            {
                MessageBox.Show("Mã sách phải là số.");
                return;
            }

            // Kiểm tra xem mã sách đã tồn tại chưa
            string checkQuery = "SELECT book_name, category, publishing, price, quantityStore FROM books WHERE book_id = @book_id";
            SqlParameter[] checkParams = { new SqlParameter("@book_id", maSach) };
            var result = kn.ExecuteReader(checkQuery, checkParams);

            if (result.HasRows)
            {
                // Nếu mã sách đã tồn tại, lấy thông tin sách và hiển thị vào các TextBox
                if (result.Read())
                {
                    txtTenSach.Text = result["book_name"].ToString();
                    txtTheLoai.Text = result["category"].ToString();
                    txtNhaXuatBan.Text = result["publishing"].ToString();
                    txtGia.Text = result["price"].ToString();

                    // Không xóa ô số lượng, để người dùng có thể nhập số lượng cần thêm
                }

                // Kiểm tra số lượng nhập vào
                if (string.IsNullOrEmpty(txtSoLuong.Text) || !int.TryParse(txtSoLuong.Text, out soLuong))
                {
                    MessageBox.Show("Vui lòng nhập số lượng sách cần thêm.");
                    return;
                }

                // Kiểm tra số lượng hiện có trong cơ sở dữ liệu
                int currentQuantity = 0;
                if (result["quantityStore"] != DBNull.Value)
                {
                    currentQuantity = Convert.ToInt32(result["quantityStore"]);
                }

                // Cộng số lượng mới vào số lượng hiện tại
                int newQuantity = currentQuantity + soLuong;

                // Cập nhật số lượng mới vào cơ sở dữ liệu
                string updateQuery = "UPDATE books SET quantityStore = @quantityStore WHERE book_id = @book_id";
                SqlParameter[] updateParams =
                {
            new SqlParameter("@quantityStore", newQuantity),
            new SqlParameter("@book_id", maSach)
        };
                kn.ExecuteQuery(updateQuery, updateParams);
                MessageBox.Show("Số lượng sách đã được cập nhật.");

                return; // Nếu sách đã tồn tại và đã cập nhật số lượng, không cần thêm sách mới nữa
            }

            // Kiểm tra các trường khác khi thêm sách mới
            if (string.IsNullOrEmpty(tenSach) || string.IsNullOrEmpty(theLoai) || string.IsNullOrEmpty(nhaXuatBan) ||
                !int.TryParse(txtGia.Text, out gia) || !int.TryParse(txtSoLuong.Text, out soLuong))
            {
                MessageBox.Show("Vui lòng điền đầy đủ và chính xác thông tin.");
                return;
            }

            // Thêm sách mới nếu mã sách không tồn tại
            string query = "INSERT INTO books (book_id, book_name, category, publishing, price, quantityStore) " +
                           "VALUES (@book_id, @book_name, @category, @publishing, @price, @quantityStore)";

            List<SqlParameter> parameters = new List<SqlParameter>
    {
        new SqlParameter("@book_id", maSach),
        new SqlParameter("@book_name", tenSach),
        new SqlParameter("@category", theLoai),
        new SqlParameter("@publishing", nhaXuatBan),
        new SqlParameter("@price", gia),
        new SqlParameter("@quantityStore", soLuong)
    };

            kn.ExecuteQuery(query, parameters.ToArray());
            MessageBox.Show("Thêm sách thành công!");

            // Xóa các trường sau khi thêm thành công
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTheLoai.Clear();
            txtNhaXuatBan.Clear();
            txtGia.Clear();
            txtSoLuong.Clear();
        }



    }
}
