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

            string checkQuery = "SELECT book_name, category, publishing, price, quantityStore FROM books WHERE book_id = @book_id";
            SqlParameter[] checkParams = { new SqlParameter("@book_id", maSach) };
            var result = kn.ExecuteReader(checkQuery, checkParams);

            if (result.HasRows)
            {
                if (result.Read())
                {
                    txtTenSach.Text = result["book_name"].ToString();
                    txtTheLoai.Text = result["category"].ToString();
                    txtNhaXuatBan.Text = result["publishing"].ToString();
                    txtGia.Text = result["price"].ToString();
                }
                if (string.IsNullOrEmpty(txtSoLuong.Text) || !int.TryParse(txtSoLuong.Text, out soLuong))
                {
                    MessageBox.Show("Vui lòng nhập số lượng sách cần thêm.");
                    return;
                }
                int currentQuantity = 0;
                if (result["quantityStore"] != DBNull.Value)
                {
                    currentQuantity = Convert.ToInt32(result["quantityStore"]);
                }
                int newQuantity = currentQuantity + soLuong;
                string updateQuery = "UPDATE books SET quantityStore = @quantityStore WHERE book_id = @book_id";
                SqlParameter[] updateParams =
                {
                    new SqlParameter("@quantityStore", newQuantity),
                    new SqlParameter("@book_id", maSach)
                };
                kn.ExecuteQuery(updateQuery, updateParams);
                MessageBox.Show("Số lượng sách đã được cập nhật.");

                return; 
            }
            if (string.IsNullOrEmpty(tenSach) || string.IsNullOrEmpty(theLoai) || string.IsNullOrEmpty(nhaXuatBan) ||
                !int.TryParse(txtGia.Text, out gia) || !int.TryParse(txtSoLuong.Text, out soLuong))
            {
                MessageBox.Show("Vui lòng điền đầy đủ và chính xác thông tin.");
                return;
            }
            string query = "INSERT INTO books (book_id, book_name, category, publishing, price,quantityShelf, quantityStore) " +
                           "VALUES (@book_id, @book_name, @category, @publishing, @price,@quantityShelf, @quantityStore)";
            int quantityShelff = 0;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@book_id", maSach),
                new SqlParameter("@book_name", tenSach),
                new SqlParameter("@category", theLoai),
                new SqlParameter("@publishing", nhaXuatBan),
                new SqlParameter("@price", gia),
                new SqlParameter("@quantityShelf", quantityShelff),
                new SqlParameter("@quantityStore", soLuong)
            };

            kn.ExecuteQuery(query, parameters.ToArray());
            MessageBox.Show("Thêm sách thành công!");
            txtMaSach.Clear();
            txtTenSach.Clear();
            txtTheLoai.Clear();
            txtNhaXuatBan.Clear();
            txtGia.Clear();
            txtSoLuong.Clear();
        }

        private void ThemSachForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }
    }
}
