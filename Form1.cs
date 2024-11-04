using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class Form1 : Form
    {
        private ketnoi kn = new ketnoi();
        private LoginForm loginForm; // Tham chiếu đến LoginForm

        public Form1(LoginForm form)
        {
            InitializeComponent();
            this.loginForm = form; // Gán tham chiếu
            LoadData();
        }

        private void LoadData()
        {
            dgSach.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgSach.DataSource = kn.GetDataTable("SELECT book_id AS [Mã sách], title AS [Tiêu đề], author AS [Tác giả], publisher AS [Nhà xuất bản], price AS [Giá], [describe] AS [Mô tả], quantity AS [Số lượng], category AS [Danh mục] FROM book;");
        }
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }
    }
}
