﻿using System.Data.SqlClient;
using System.Data;
using System.Windows.Forms;
using System;

public class Connect
{
    private string connectionString = "Data Source=(Localdb)\\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";

    // Phương thức để thực thi câu lệnh SQL không trả về dữ liệu
    public void ExecuteQuery(string query, SqlParameter[] parameters)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // Thêm các tham số vào câu lệnh SQL nếu có
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();  // Thực thi câu lệnh SQL
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }
        }
    }

    // Phương thức để lấy dữ liệu dưới dạng DataTable
    public DataTable GetDataTable(string query, SqlParameter[] parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }

    // Phương thức để thực thi câu lệnh SQL và trả về một giá trị duy nhất
    public object ExecuteScalar(string query, SqlParameter[] parameters = null)
    {
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                try
                {
                    conn.Open();
                    return cmd.ExecuteScalar();  // Thực thi câu lệnh và trả về giá trị đầu tiên
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                    return null;
                }
            }
        }
    }

    // Phương thức để thực thi câu lệnh SQL và trả về SqlDataReader
    public SqlDataReader ExecuteReader(string query, SqlParameter[] parameters = null)
    {
        SqlConnection conn = new SqlConnection(connectionString);
        SqlCommand cmd = new SqlCommand(query, conn);
        if (parameters != null)
        {
            cmd.Parameters.AddRange(parameters);
        }

        try
        {
            conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);  // Trả về SqlDataReader và đóng kết nối khi đọc xong
        }
        catch (Exception ex)
        {
            MessageBox.Show("Lỗi: " + ex.Message);
            conn.Close();  // Đảm bảo đóng kết nối trong trường hợp lỗi
            return null;
        }
    }
}
