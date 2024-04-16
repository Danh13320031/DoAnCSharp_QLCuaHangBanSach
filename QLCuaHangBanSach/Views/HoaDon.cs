using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLCuaHangBanSach.Views
{
    public partial class HoaDon : Form
    {
        public HoaDon()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public void SetupDataGridView()
        {
            dataGridView1.ColumnCount = 5;
            dataGridView1.Columns[0].Name = "Mã sách";
            dataGridView1.Columns[1].Name = "Tên sách";
            dataGridView1.Columns[2].Name = "Số lượng";
            dataGridView1.Columns[3].Name = "Giá bán";
            dataGridView1.Columns[4].Name = "Thành Tiền";

            // Đặt cột "Thành Tiền" không cho nhập
            dataGridView1.Columns[3].ReadOnly = true;
        }

        // Lấy dữ liệu 
        private DataTable GetData(string query)
        {

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        private void LoadSachComboBox()
        {
            string query = "SELECT MaSach, TenSach FROM Sach";
            DataTable theloai = GetData(query);
            comboBox1.DataSource = theloai;
            comboBox1.DisplayMember = "TenSach";
            comboBox1.ValueMember = "MaSach";
        }

        private void HoaDon_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadSachComboBox();
        }

        // Thêm dữ liệu hóa đơn vào dataGridView
        private void button5_Click(object sender, EventArgs e)
        {
            string tenSach = comboBox1.Text;
            string maSach = (string)comboBox1.SelectedValue;
            int soLuong;
            decimal giaBan, thanhTien;

            if (textBox1.Text == "" || textBox2.Text == "")
            {
                MessageBox.Show("Vui lòng nhập thông tin Sách", "Thông báo");
                return;
            }

            if (!int.TryParse(textBox2.Text, out soLuong) || !decimal.TryParse(textBox1.Text, out giaBan))
            {
                MessageBox.Show("Số lượng và giá nhập phải là số.");
                return;
            }

            // Tính thành tiền
            thanhTien = soLuong * giaBan;

            // Thêm dòng mới vào DataGridView
            dataGridView1.Rows.Add(maSach,tenSach, soLuong,giaBan, thanhTien);
            xoaTextBox();
        }

        // Xóa hóa đơn
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    dataGridView1.Rows.RemoveAt(item.Index); // Xóa dòng tại vị trí được chọn
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dòng để xóa.");
            }
        }

        private void xoaTextBox()
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            xoaTextBox();
            txtSoHoaDon.Clear();
        }

        // Tính tổng tiền
        private void button1_Click(object sender, EventArgs e)
        {
            decimal tongTien = 0;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["Thành Tiền"].Value != null)
                {
                    tongTien += Convert.ToDecimal(row.Cells[4].Value);
                }
            }

            // Hiển thị tổng tiền trên textTongTien
            textBox3.Text = tongTien.ToString("N2"); // "N2" để format số thành dạng có hai chữ số thập phân
        }

        // Tạo hóa đơn
        private void CreateHoaDon()
        {
            string query = "INSERT INTO HoaDon (SoHoaDon, NgayBan) VALUES (@SoHD, @NgayBan)";
            DateTime selectedDateTime = dateTimePicker1.Value;
            string soHD = txtSoHoaDon.Text;
            string tongTien = textBox3.Text;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.Add("@SoHD", SqlDbType.NVarChar).Value = soHD ?? (object)DBNull.Value;
                    command.Parameters.AddWithValue("@NgayBan", selectedDateTime);
                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        MessageBox.Show(result > 0 ? "Thêm hóa đơn thành công." : "Không thêm được hóa đơn.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm hóa đơn: " + ex.Message);
                    }
                }
            }
        }

        // Lấy mã Sách từ tên Sách
        private string GetMaSachFromTenSach(string tenSach, SqlConnection conn)
        {
            string query = "SELECT Sach.MaSach FROM Sach WHERE Sach.TenSach = @TenSach";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@TenSach", tenSach);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return reader["MaSach"].ToString();
                    }
                    else
                    {
                        return null; // Xử lý trường hợp không tìm thấy mã sách
                    }
                }
            }
        }

        // Tạo chi tiết hóa đơn
        private void CreateChiTietHoaDon()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string tenSach = row.Cells[1].Value.ToString();
                        string maSach = GetMaSachFromTenSach(tenSach, conn);
                        string query = "INSERT INTO ChiTietHoaDon(MaSach, SoHoaDon, SoLuongBan, GiaBan) VALUES (@MaSach, @SoHD, @SLB, @GiaBan)";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.Add("@MaSach", SqlDbType.NVarChar).Value = maSach ?? (object)DBNull.Value;
                            cmd.Parameters.Add("@SoHD", SqlDbType.NVarChar).Value = txtSoHoaDon.Text ?? (object)DBNull.Value;
                            cmd.Parameters.AddWithValue("@SLB", int.TryParse(row.Cells[2].Value?.ToString(), out int soLuongBan) ? (object)soLuongBan : DBNull.Value);
                            cmd.Parameters.AddWithValue("@GiaBan", decimal.TryParse(row.Cells[3].Value?.ToString(), out decimal giaBan) ? (object)giaBan : DBNull.Value);

                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Dữ liệu đã được thêm thành công vào Chi Tiết Hóa Đơn.");
            }
        }

        // Ghi hóa đơn
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Form Validate ko cho người dùng ghi hóa đơn khi dữ liệu trong dataGridView trống
            if (dataGridView1.RowCount <= 1)
            {
                MessageBox.Show("Vui lòng thêm dữ liệu Sách vào bảng trước khi thực hiện ghi hóa đơn", "Thông báo");
                return;
            }

            // Form Validate ko cho người dùng ghi hóa đơn khi chưa nhập số hóa đơn
            if (txtSoHoaDon.Text == "")
            {
                MessageBox.Show("Vui lòng nhập số hóa đơn trước khi ghi hóa đơn", "Thông báo");
                return;
            }

            // Validate Form ko cho người dùng ghi hóa đơn khi số hóa đơn đã tồn tại
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string sqlSoHoaDon = "SELECT TOP(1) HoaDon.SoHoaDon FROM HoaDon WHERE HoaDon.SoHoaDon = '" + txtSoHoaDon.Text + "'";

                SqlCommand sqlCommand = new SqlCommand(sqlSoHoaDon, conn);
                if (sqlCommand.ExecuteScalar() != null)
                {
                    MessageBox.Show("Số hóa đơn đã tồn tại, vui lòng nhập lại");
                    return;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            finally
            {
                conn.Close();
            }

            CreateHoaDon();
            CreateChiTietHoaDon();
        }

        // Hiển thị trang chủ
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home HomeForm = new Home();
            HomeForm.Show();
        }

        // Thoát khỏi ứng dụng
        private void HoaDon_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
