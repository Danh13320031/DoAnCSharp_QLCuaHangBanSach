using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLCuaHangBanSach.Views
{
    public partial class ChiTietPhieuNhap : Form
    {
        public ChiTietPhieuNhap()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";
        
        // Tìm kiếm dữ liệu chi tiết phiếu nhập
        private void button1_Click(object sender, EventArgs e)
        {
            string soPhieuNhap = textBox1.Text; // Lấy số phiếu nhập từ TextBox

            if (string.IsNullOrWhiteSpace(soPhieuNhap))
            {
                MessageBox.Show("Vui lòng nhập số phiếu nhập.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT Sach.MaSach, Sach.TenSach, PhieuNhap.NgayNhap, ChiTietPhieuNhap.SoLuongNhap, ChiTietPhieuNhap.GiaNhap " +
                        "FROM ChiTietPhieuNhap " +
                        "JOIN Sach ON Sach.MaSach = ChiTietPhieuNhap.MaSach " +
                        "JOIN PhieuNhap ON PhieuNhap.SoPhieuNhap = ChiTietPhieuNhap.SoPhieuNhap " +
                        "WHERE ChiTietPhieuNhap.SoPhieuNhap = @SoPhieuNhap";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SoPhieuNhap", soPhieuNhap);

                    DataTable table = new DataTable();
                    adapter.Fill(table); // Tải dữ liệu vào DataTable

                    if (table.Rows.Count > 0)
                    {
                        dataGridView.DataSource = table; // Đặt nguồn dữ liệu cho DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu cho số phiếu nhập này.");
                        dataGridView.DataSource = null; // Xóa dữ liệu hiện tại trên DataGridView
                        Console.WriteLine("Không tìm thấy dữ liệu");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy vấn cơ sở dữ liệu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home HomeForm = new Home();
            HomeForm.Show();
        }

        private void ChiTietPhieuNhap_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
