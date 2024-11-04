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
        }
        private void LoadBooks()
        {
            string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT book_id, book_name, category, publishing, price, quantityShelf, quantityStore FROM books"; // Bỏ cột img
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

                // Thiết lập AutoSizeMode cho các cột
                foreach (DataGridViewColumn column in dataGridViewBooks.Columns)
                {
                    //column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; 
                    column.MinimumWidth = 150;
                    //column.Width = Math.Min(column.Width, 200);
                }

                // Đảm bảo cột đầu tiên vẫn giữ độ rộng tự động
                dataGridViewBooks.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells; // Cột đầu tiên là Mã Sách
            }
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }
    }
}
