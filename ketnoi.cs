using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace baitaplon
{
    internal class ketnoi
    {
        private string kn = @"Data Source=(Localdb)\mssqlLocaldb;Initial Catalog=baitaplon;Integrated Security=True";
        private SqlConnection sqlConnection = new SqlConnection();
        public ketnoi()
        {
            try
            {
                sqlConnection.ConnectionString = kn;
            }
            catch (SqlException)
            {
                MessageBox.Show("Kết nối thất bại ");
                Application.Exit();
            }
        }
        public int RunExecuteNonQuery(SqlCommand sqlCommand)
        {
            int result = 0;
            try
            {
                sqlCommand.Connection = sqlConnection;
                sqlConnection.Open();
                result = sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close(); 
            }
            return result;
        }
        public DataTable GetDataTable(string query)
        {
            DataTable data = new DataTable();
            try
            {
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); 
                sqlDataAdapter.Fill(data);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return data;
        }
    }
}
