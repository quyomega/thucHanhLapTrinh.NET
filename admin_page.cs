using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class admin_page : Form
    {
        private ketnoi kn = new ketnoi();
        private LoginForm loginForm; // Tham chiếu đến LoginForm

        public admin_page(LoginForm form)
        {
            InitializeComponent();
            this.loginForm = form; // Gán tham chiếu
            LoadBooks();

            // Sự kiện TextChanged cho TextBox tìm kiếm
            txtName.TextChanged += txtSearch_TextChanged;
            txtCategory.TextChanged += SearchCriteriaChanged;
            txtPublisher.TextChanged += SearchCriteriaChanged;
            txtPrice.TextChanged += SearchCriteriaChanged;

            cmbLocation.SelectedIndexChanged += SearchCriteriaChanged;
            // Sự kiện Tick cho Timer tìm kiếm
            searchTimer.Tick += searchTimer_Tick;
        }

        // Khi có thay đổi trong TextBox tìm kiếm
        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // Đặt lại Timer mỗi lần người dùng thay đổi nội dung
            searchTimer.Stop();
            searchTimer.Start();
        }

        // Xử lý tìm kiếm khi Timer hết hạn
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

                dataGridViewBooks.Columns["book_id"].Width = 100;          // Chiều rộng cột "Mã Sách"
                dataGridViewBooks.Columns["book_name"].Width = 140;       // Chiều rộng cột "Tên Sách"
                dataGridViewBooks.Columns["category"].Width = 120;        // Chiều rộng cột "Thể Loại"
                dataGridViewBooks.Columns["publishing"].Width = 150;      // Chiều rộng cột "Nhà Xuất Bản"
                dataGridViewBooks.Columns["price"].Width = 90;           // Chiều rộng cột "Giá"
                dataGridViewBooks.Columns["quantityShelf"].Width = 120;   // Chiều rộng cột "SL Trên Kệ"
                dataGridViewBooks.Columns["quantityStore"].Width = 140;
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

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }

        private void SearchCriteriaChanged(object sender, EventArgs e)
        {
            searchTimer.Stop();
            searchTimer.Start();
        }
    }

}
