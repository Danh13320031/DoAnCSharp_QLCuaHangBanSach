using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLCuaHangBanSach.Views
{
    public partial class ChiTietHoaDon : Form
    {
        public ChiTietHoaDon()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        // Tìm kiếm dữ liệu chi tiết hóa đơn
        private void button1_Click(object sender, EventArgs e)
        {
            string soHoaDon = textBox1.Text; // Lấy số phiếu nhập từ TextBox

            if (string.IsNullOrWhiteSpace(soHoaDon))
            {
                MessageBox.Show("Vui lòng nhập số hóa đơn.");
                return;
            }

            string query = @"
SELECT Sach.MaSach, Sach.TenSach,HoaDon.NgayBan, ChiTietHoaDon.SoLuongBan, ChiTietHoaDon.GiaBan
FROM ChiTietHoaDon
JOIN Sach ON Sach.MaSach = ChiTietHoaDon.MaSach
JOIN HoaDon ON HoaDon.SoHoaDon = ChiTietHoaDon.SoHoaDon
WHERE ChiTietHoaDon.SoHoaDon = @SoHoaDon";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SoHoaDon", soHoaDon);

                    DataTable table = new DataTable();
                    adapter.Fill(table); // Tải dữ liệu vào DataTable

                    if (table.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = table; // Đặt nguồn dữ liệu cho DataGridView
                    }
                    else
                    {
                        MessageBox.Show("Không tìm thấy dữ liệu cho số hóa đơn này.");
                        dataGridView1.DataSource = null; // Xóa dữ liệu hiện tại trên DataGridView
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi truy vấn cơ sở dữ liệu: " + ex.Message);
                }
            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home HomeForm = new Home();
            HomeForm.Show();
        }

        private void ChiTietHoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
