using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class InvoiceDetailsForm : Form
    {
        private string selectedInvoiceId; // Biến để lưu mã hóa đơn
        private Connect kn = new Connect(); // Đối tượng kết nối cơ sở dữ liệu

        // Constructor nhận vào selectedInvoiceId từ AdminPage
        public InvoiceDetailsForm(string invoiceId)
        {
            InitializeComponent();
            this.selectedInvoiceId = invoiceId; // Lưu mã hóa đơn vào biến
            LoadInvoiceDetails(invoiceId); // Gọi phương thức tải chi tiết hóa đơn
        }

        // Phương thức để tải chi tiết hóa đơn
        private void LoadInvoiceDetails(string invoiceId)
        {
            string query = "SELECT MaChiTiet, MaHoaDon, MaSach, DonGia, SoLuong, TongGia FROM ChiTietHoaDon WHERE MaHoaDon = @invoiceId";

            // Tạo tham số cho câu truy vấn
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@invoiceId", SqlDbType.NVarChar) { Value = invoiceId }
            };

            // Lấy dữ liệu từ cơ sở dữ liệu
            DataTable invoiceDetailsTable = kn.GetDataTable(query, parameters);

            // Gán dữ liệu vào DataGridView
            dgvInvoiceDetails.DataSource = invoiceDetailsTable;
        }

    }
}
