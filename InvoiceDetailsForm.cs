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
        private string selectedInvoiceId; 
        private Connect kn = new Connect(); 
        public InvoiceDetailsForm(string invoiceId)
        {
            InitializeComponent();
            this.selectedInvoiceId = invoiceId; 
            LoadInvoiceDetails(invoiceId);
            SetInvoiceDetails();
        }
        private void SetInvoiceDetails()
        {
            dgvInvoiceDetails.ColumnHeadersDefaultCellStyle.Font = new Font(dgvInvoiceDetails.Font, FontStyle.Bold);
        }

        private void LoadInvoiceDetails(string invoiceId)
        {
            string query = "SELECT MaChiTiet, MaHoaDon, MaSach, DonGia, SoLuong, TongGia FROM ChiTietHoaDon WHERE MaHoaDon = @invoiceId";

            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@invoiceId", SqlDbType.NVarChar) { Value = invoiceId }
            };

            DataTable invoiceDetailsTable = kn.GetDataTable(query, parameters);

            dgvInvoiceDetails.DataSource = invoiceDetailsTable;
        }

    }
}
