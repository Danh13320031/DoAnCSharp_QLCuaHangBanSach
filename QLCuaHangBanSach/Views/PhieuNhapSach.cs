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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QLCuaHangBanSach.Views
{
    public partial class PhieuNhapSach : Form
    {
        public PhieuNhapSach()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public void SetupDataGridView()
        {
            // Giả sử dataGridView1 là DataGridView của bạn
            dataGridView.ColumnCount = 8; // Số cột bạn cần, không tính cột "Thành tiền"
            dataGridView.Columns[0].Name = "Mã Sách";
            dataGridView.Columns[1].Name = "Tên Sách";
            dataGridView.Columns[2].Name = "Mã Thể Loại";
            dataGridView.Columns[3].Name = "Mã Tác Giả";
            dataGridView.Columns[4].Name = "Mã NXB";
            dataGridView.Columns[5].Name = "Số Lượng";
            dataGridView.Columns[6].Name = "Giá Nhập";
            dataGridView.Columns[7].Name = "Thành Tiền";

            // Đặt cột "Thành Tiền" không cho nhập
            dataGridView.Columns[7].ReadOnly = true;
        }

        // Thêm phiếu nhập sách
        private void button5_Click(object sender, EventArgs e)
        {
            string maNXB = (string)comboBoxNXB.SelectedValue;
            string maTacGia = (string)comboBoxTG.SelectedValue;
            string maTL = (string)comboBoxTL.SelectedValue;

            string maSach = txtMaSach.Text;
            string tenSach = txtTenSach.Text;
            string tenNXB = comboBoxNXB.Text;
            string tenTacGia = comboBoxTG.Text;
            string tenTheLoai = comboBoxTL.Text;
            int soLuong;
            decimal giaNhap, thanhTien;

            // Validate Form khi không nhập thông tin vào các trường
            if (txtMaSach.Text == "" || txtSoLuong.Text == "" || txtGiaNhap.Text == "" || txtTenSach.Text == "")
            {
                MessageBox.Show("Vui lòng điền thông tin Sách", "Thông báo");
                return;
            }

            // Validate Form khi mã sách đã tồn tại
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string sqlMaSach = "SELECT TOP(1) Sach.MaSach FROM Sach WHERE Sach.MaSach = '" + txtMaSach.Text + "'";

                SqlCommand sqlCommand = new SqlCommand(sqlMaSach, conn);
                Console.WriteLine("SQL: " + sqlCommand.ExecuteScalar());
                if (sqlCommand.ExecuteScalar() != null)
                {
                    MessageBox.Show("Sách đã tồn tại, vui lòng nhập lại");
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

            // Validate Form khi số lượng và giá nhập sai định dạng
            if (!int.TryParse(txtSoLuong.Text, out soLuong) || !decimal.TryParse(txtGiaNhap.Text, out giaNhap))
            {
                MessageBox.Show("Số lượng và giá nhập phải là số.");
                return;
            }

            // Tính thành tiền
            thanhTien = soLuong * giaNhap;

            // Thêm dòng mới vào DataGridView
            dataGridView.Rows.Add(maSach, tenSach, maTL, maTacGia, maNXB, soLuong, giaNhap, thanhTien);
            xoaTextBox();
        }

        private void xoaTextBox()
        {
            txtGiaNhap.Clear();
            txtMaSach.Clear();
            txtSoLuong.Clear();
            txtTenSach.Clear();
        }

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

        private void LoadNhaXuatBanComboBox()
        {
            string query = "SELECT MaNhaXuatBan, TenNhaXuatBan FROM NhaXuatBan";
            DataTable nhaxuatban = GetData(query);
            comboBoxNXB.DataSource = nhaxuatban;
            comboBoxNXB.DisplayMember = "TenNhaXuatBan"; // Cột bạn muốn hiển thị (tên)
            comboBoxNXB.ValueMember = "MaNhaXuatBan"; // Giá trị khi một mục được chọn (mã)
        }

        private void LoadNhaTacGiaComboBox()
        {
            string query = "SELECT MaTacGia, TenTacGia FROM TacGia";
            DataTable tacgia = GetData(query);
            comboBoxTG.DataSource = tacgia;
            comboBoxTG.DisplayMember = "TenTacGia";
            comboBoxTG.ValueMember = "MaTacGia";
        }

        private void LoadTheLoaiComboBox()
        {
            string query = "SELECT MaTheLoai, TenTheLoai FROM TheLoai";
            DataTable theloai = GetData(query);
            comboBoxTL.DataSource = theloai;
            comboBoxTL.DisplayMember = "TenTheLoai";
            comboBoxTL.ValueMember = "MaTheLoai";
        }

        private void PhieuNhapSach_Load(object sender, EventArgs e)
        {
            LoadNhaTacGiaComboBox();
            LoadNhaXuatBanComboBox();
            LoadTheLoaiComboBox();
            SetupDataGridView();
        }

        // Lập phiếu mới
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                // Lặp qua tất cả các dòng đang được chọn (trong trường hợp chọn nhiều dòng)
                foreach (DataGridViewRow item in dataGridView.SelectedRows)
                {
                    dataGridView.Rows.RemoveAt(item.Index); // Xóa dòng tại vị trí được chọn
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn ít nhất một dòng để xóa.");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView.Rows.Clear();
            xoaTextBox();
            txtPhieuNhap.Clear();
        }

        // Tính tổng tiền
        private void button2_Click(object sender, EventArgs e)
        {
            decimal tongTien = 0;

            // Duyệt qua từng hàng trong DataGridView
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                if (row.Cells["Thành Tiền"].Value != null)
                {
                    // Cộng dồn giá trị của cột "Thành Tiền" vào biến tongTien
                    tongTien += Convert.ToDecimal(row.Cells["Thành Tiền"].Value);
                }
            }

            // Hiển thị tổng tiền trên textTongTien
            textBox2.Text = tongTien.ToString("N2"); // "N2" để format số thành dạng có hai chữ số thập phân
        }

        // Tạo phiếu nhập
        private void CreatePhieuNhap()
        {
            string query = "INSERT INTO PhieuNhap(SoPhieuNhap, NgayNhap, MaNhaXuatBan) VALUES(@SoPN, @NgayNhap, @maNXB)";
            DateTime selectedDateTime = dateTimePicker1.Value;
            string maNXB = (string)comboBoxNXB.SelectedValue;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@SoPN", txtPhieuNhap.Text);
                        command.Parameters.AddWithValue("@NgayNhap", selectedDateTime);
                        command.Parameters.AddWithValue("@maNXB", maNXB);

                        int result = (int)command.ExecuteNonQuery();
                        MessageBox.Show(result > 0 ? "Thêm phiếu nhập thành công." : "Không thêm được phiếu nhập.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi thêm phiếu nhập: " + ex.Message);
                    }
                }
            }
        }

        private void CreateChiTietPhieuNhap()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string query = "INSERT INTO ChiTietPhieuNhap (MaSach, SoPhieuNhap, SoLuongNhap, GiaNhap) VALUES(@MaSach, @SoPN, @SLN, @GiaNhap)";
                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaSach", row.Cells[0].Value ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@SoPN", txtPhieuNhap.Text);
                                cmd.Parameters.AddWithValue("@SLN", int.TryParse(row.Cells[5].Value?.ToString(), out int soNhap) ? (object)soNhap : DBNull.Value);
                                cmd.Parameters.AddWithValue("@GiaNhap", decimal.TryParse(row.Cells[6].Value?.ToString(), out decimal giaNhap) ? (object)giaNhap : DBNull.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    MessageBox.Show("Dữ liệu đã được thêm thành công vào Chi Tiết Phiếu Nhập.");
                }
                catch (Exception error)
                {
                    Console.WriteLine(error);
                }
                finally
                {
                    conn.Close();
                }
            }
        }

        private void CreateSach()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    foreach (DataGridViewRow row in dataGridView.Rows)
                    {
                        if (!row.IsNewRow)
                        {
                            string query = "INSERT INTO Sach(MaSach, TenSach, SoLuongTon, MaTheLoai, MaNhaXuatBan, MaTacGia) VALUES(@MaSach, @TenSach, @SoLuongTon, @MaTheLoai, @MaNhaXuatBan, @MaTacGia)";

                            using (SqlCommand cmd = new SqlCommand(query, conn))
                            {
                                cmd.Parameters.AddWithValue("@MaSach", row.Cells[0].Value ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@TenSach", row.Cells[1].Value ?? DBNull.Value);

                                int soLuongTon;
                                if (int.TryParse(row.Cells[5].Value?.ToString(), out soLuongTon))
                                    cmd.Parameters.AddWithValue("@SoLuongTon", soLuongTon);
                                else
                                    cmd.Parameters.AddWithValue("@SoLuongTon", DBNull.Value);

                                cmd.Parameters.AddWithValue("@MaTheLoai", row.Cells[2].Value ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@MaNhaXuatBan", row.Cells[4].Value ?? DBNull.Value);
                                cmd.Parameters.AddWithValue("@MaTacGia", row.Cells[3].Value ?? DBNull.Value);

                                decimal giaBan;
                                if (decimal.TryParse(row.Cells[6].Value?.ToString(), out giaBan))
                                    cmd.Parameters.AddWithValue("@GiaBan", giaBan);
                                else
                                    cmd.Parameters.AddWithValue("@GiaBan", DBNull.Value);

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }

                    MessageBox.Show("Dữ liệu đã được thêm thành công vào bảng Sach.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Validate Form khi không nhập vào số phiếu nhập
            if (txtPhieuNhap.Text == "")
            {
                MessageBox.Show("Vui lòng điền thông Số phiếu nhập", "Thông báo");
                return;
            }

            // Validate Form khi mã phiếu nhập đã tồn tại
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string sqlMaSach = "SELECT TOP(1) PhieuNhap.SoPhieuNhap FROM PhieuNhap WHERE PhieuNhap.SoPhieuNhap = '" + txtPhieuNhap.Text + "'";

                SqlCommand sqlCommand = new SqlCommand(sqlMaSach, conn);
                if (sqlCommand.ExecuteScalar() != null)
                {
                    MessageBox.Show("Phiếu nhập đã tồn tại, vui lòng nhập lại");
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

            // Validate Form khi Chi tiết phiếu nhập trống, chưa có Sách để ghi phiếu
            if (dataGridView.RowCount <= 1)
            {
                MessageBox.Show("Vui lòng thêm dữ liệu Sách vào bảng trước khi thực hiện ghi phiếu", "Thông báo");
                return;
            }

            CreateSach();
            CreatePhieuNhap();
            CreateChiTietPhieuNhap();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home HomeForm = new Home();
            HomeForm.Show();
        }

        private void PhieuNhapSach_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
