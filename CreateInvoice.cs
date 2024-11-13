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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace baitaplon
{
    public partial class CreateInvoice : Form
    {
        private Connect ketNoi = new Connect();
        private string tenNhanVien;
        private string username;

        public CreateInvoice(string username)
        {
            InitializeComponent();
            this.username = username;
            tenNhanVien = LayTenNhanVien(username); 
            txtTenNhanVien.Text = tenNhanVien;
            txtTenNhanVien.ReadOnly = true;

            LoadBookNames(); 
            cbTenSach.SelectedIndexChanged += cbTenSach_SelectedIndexChanged;
            txtSoLuong.TextChanged += txtSoLuong_TextChanged;
        }

        private void LoadBookNames()
        {
            string query = "SELECT book_name FROM Books";
            DataTable booksTable = ketNoi.GetDataTable(query);

            foreach (DataRow row in booksTable.Rows)
            {
                cbTenSach.Items.Add(row["book_name"].ToString());
            }
        }

        private void txtSoLuong_TextChanged(object sender, EventArgs e)
        {
            CalculateTotalPrice();
        }
        private void CalculateTotalPrice()
        {
            if (int.TryParse(txtSoLuong.Text, out int soLuong) && int.TryParse(txtDonGia.Text, out int donGia))
            {
                int tongGia = soLuong * donGia;
                txtTongGia1.Text = tongGia.ToString();
            }
            else
            {
                txtTongGia1.Text = string.Empty;
            }
        }

        private string LayTenNhanVien(string username)
        {
            string query = "SELECT TenNhanVien FROM users WHERE username = @username";
            SqlParameter[] parameters = { new SqlParameter("@username", username) };

            try
            {
                object result = ketNoi.ExecuteScalar(query, parameters);
                if (result != null && result != DBNull.Value)
                {
                    return result.ToString();
                }
                else
                {
                    MessageBox.Show("Không tìm thấy tên nhân viên cho tài khoản này.");
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
                return string.Empty;
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            string tenSach = cbTenSach.Text;
            if (string.IsNullOrEmpty(tenSach))
            {
                MessageBox.Show("Vui lòng chọn tên sách.");
                return;
            }

            if (!int.TryParse(txtSoLuong.Text, out int soLuong) || !int.TryParse(txtDonGia.Text, out int donGia))
            {
                MessageBox.Show("Vui lòng nhập số lượng và đơn giá hợp lệ.");
                return;
            }

            int maSach = LayMaSachTheoTen(tenSach);

            int quantityShelf = LaySoLuongSachTrongKho(maSach);

            if (soLuong > quantityShelf)
            {
                MessageBox.Show($"Số lượng sách bạn nhập vượt quá số lượng có sẵn trong kho. Số lượng trong kho: {quantityShelf}");
                return;
            }

            int tongGia = soLuong * donGia;

            dgvHoaDon.Rows.Add(tenSach, soLuong, donGia, tongGia);

            txtTongGia1.Text = tongGia.ToString();

            UpdateTotalPrice();

            cbTenSach.SelectedIndex = -1;
            txtSoLuong.Clear();
            txtDonGia.Clear();
        }

        private int LaySoLuongSachTrongKho(int maSach)
        {
            string query = "SELECT quantityShelf FROM Books WHERE book_id = @MaSach";
            SqlParameter[] parameters = { new SqlParameter("@MaSach", maSach) };

            object result = ketNoi.ExecuteScalar(query, parameters);
            if (result != null)
            {
                return Convert.ToInt32(result);
            }
            else
            {
                MessageBox.Show("Không tìm thấy sách trong kho.");
                return 0;
            }
        }


        private void UpdateTotalPrice()
        {
            int totalPrice = 0;
            foreach (DataGridViewRow row in dgvHoaDon.Rows)
            {
                if (row.IsNewRow) continue; 
                totalPrice += Convert.ToInt32(row.Cells["TongGia"].Value);
            }
            txtTongGia.Text = totalPrice.ToString();
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            string tenKhachHang = txtTenKH.Text;
            string tenNhanVien = txtTenNhanVien.Text;
            DateTime thoiGianBan = DateTime.Now;

            if (string.IsNullOrEmpty(tenKhachHang))
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng.");
                return;
            }

            if (!int.TryParse(txtTongGia.Text, out int tongGiaHoaDon))
            {
                MessageBox.Show("Tổng giá không hợp lệ.");
                return;
            }

            try
            {
                ketNoi.ExecuteQuery("BEGIN TRANSACTION", null);

                string queryHoaDon = "INSERT INTO HoaDon (ThoiGianBan, TenNhanVien, TenKhachHang, TongGia) OUTPUT INSERTED.MaHoaDon VALUES (@ThoiGianBan, @TenNhanVien, @TenKhachHang, @TongGia)";
                SqlParameter[] parametersHoaDon = {
                    new SqlParameter("@ThoiGianBan", thoiGianBan),
                    new SqlParameter("@TenNhanVien", tenNhanVien),
                    new SqlParameter("@TenKhachHang", tenKhachHang),
                    new SqlParameter("@TongGia", tongGiaHoaDon)
                };

                object result = ketNoi.ExecuteScalar(queryHoaDon, parametersHoaDon);
                if (result == null)
                {
                    throw new Exception("Không thể lấy mã hóa đơn vừa tạo.");
                }

                int maHoaDon = (int)result;

                foreach (DataGridViewRow row in dgvHoaDon.Rows)
                {
                    if (row.IsNewRow) continue;

                    string tenSach = row.Cells["TenSach"].Value.ToString();
                    int soLuong = Convert.ToInt32(row.Cells["SoLuong"].Value);
                    int donGia = Convert.ToInt32(row.Cells["DonGia"].Value);

                    int maSach = LayMaSachTheoTen(tenSach);

                    string queryChiTiet = "INSERT INTO ChiTietHoaDon (MaHoaDon, MaSach, SoLuong, DonGia) VALUES (@MaHoaDon, @MaSach, @SoLuong, @DonGia)";
                    SqlParameter[] parametersChiTiet = {
                        new SqlParameter("@MaHoaDon", maHoaDon),
                        new SqlParameter("@MaSach", maSach),
                        new SqlParameter("@SoLuong", soLuong),
                        new SqlParameter("@DonGia", donGia)
                   };

                    ketNoi.ExecuteQuery(queryChiTiet, parametersChiTiet);

                    string queryUpdateBook = "UPDATE Books SET quantityShelf = quantityShelf - @SoLuong WHERE book_id = @MaSach";
                    SqlParameter[] parametersUpdateBook = {
                        new SqlParameter("@SoLuong", soLuong),
                        new SqlParameter("@MaSach", maSach)
                    };
                    ketNoi.ExecuteQuery(queryUpdateBook, parametersUpdateBook);
                }

                MessageBox.Show("Hóa đơn đã được xác nhận và lưu thành công!");

                dgvHoaDon.Rows.Clear();
                txtTenKH.Clear();
                txtTongGia.Clear();
            }
            catch (Exception ex)
            {
                ketNoi.ExecuteQuery("ROLLBACK TRANSACTION", null);
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }


        private int LayMaSachTheoTen(string tenSach)
        {
            string query = "SELECT TOP 1 book_id FROM Books WHERE book_name = @TenSach";
            SqlParameter[] parameters = { new SqlParameter("@TenSach", tenSach) };

            object result = ketNoi.ExecuteScalar(query, parameters);
            if (result != null)
            {
                return (int)result;
            }
            else
            {
                throw new Exception("Không tìm thấy mã sách cho tên sách: " + tenSach);
            }
        }

        private void cbTenSach_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbTenSach.SelectedItem == null)
            {
                txtDonGia.Text = string.Empty; 
                return;
            }

            string selectedBookName = cbTenSach.SelectedItem.ToString();
            string query = "SELECT price FROM Books WHERE book_name = @bookName";
            SqlParameter[] parameters = { new SqlParameter("@bookName", selectedBookName) };

            object result = ketNoi.ExecuteScalar(query, parameters);
            if (result != null)
            {
                txtDonGia.Text = result.ToString();
                CalculateTotalPrice();
            }
        }
    }
}
