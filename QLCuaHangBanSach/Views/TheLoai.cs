using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace QLCuaHangBanSach.Views
{
    public partial class TheLoai : Form
    {
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";
        BindingSource categoryBindingSource = new BindingSource();

        public TheLoai()
        {
            InitializeComponent();
        }

       // Hiển thị dữ liệu thể loại lên dataGridView
        private void TheLoai_Load(object sender, EventArgs e)
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            categoryBindingSource.DataSource = categoryDAO.getAllCategory();

            dataGridView.DataSource = categoryDAO.getAllCategory();
        }


        // Kiểm tra mã thể loại
        private bool KiemTraMaTheLoai(string matheloai)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    // Tạo một lệnh SQL để kiểm tra mã tác giả
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM TheLoai WHERE MaTheLoai = @MaTheLoai", connection);
                    cmd.Parameters.AddWithValue("@MaTheLoai", matheloai);

                    // Thực thi lệnh và kiểm tra kết quả
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        return true; // Mã tác giả đã tồn tại
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi kết nối đến cơ sở dữ liệu: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return false; // Mã tác giả không tồn tại
        }
          
        // Thêm thể loại
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string matheloai = txtMaTheLoai.Text;
            if (KiemTraMaTheLoai(matheloai))
            {
                MessageBox.Show("Thể loại này đã tồn tại");
            }
            else
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "INSERT INTO TheLoai (MaTheLoai, TenTheLoai ) VALUES (@MaTheLoai, @TenTheLoai)";
                    if (string.IsNullOrEmpty(txtMaTheLoai.Text) || string.IsNullOrEmpty(txtTenTheLoai.Text))
                    {
                        MessageBox.Show("Vui lòng nhập đầy đủ thông tin thể loại");
                        return;
                    }
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm các tham số vào câu lệnh SQL để tránh SQL Injection
                        command.Parameters.AddWithValue("@MaTheLoai", matheloai);
                        command.Parameters.AddWithValue("@TenTheLoai", txtTenTheLoai.Text);


                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Kiểm tra kết quả để xác định việc thêm dữ liệu thành công hay không
                        if (result > 0)
                        {
                            CategoryDAO categoryDAO = new CategoryDAO();
                            categoryBindingSource.DataSource = categoryDAO.getAllCategory();

                            dataGridView.DataSource = categoryDAO.getAllCategory();
                            MessageBox.Show("Thêm thể loại thành công!");

                        }
                        else
                        {
                            MessageBox.Show("Thêm thể loại không thành công.");
                        }
                    }
                }
            }
        }


        // Sửa thông tin thể loại
        private void btnInsert_Click(object sender, EventArgs e)
        {
            string matheloai = txtMaTheLoai.Text;
            string tentheloai = txtTenTheLoai.Text;

            string query = "UPDATE TheLoai SET TenTheLoai = @TenTheLoai WHERE MaTheLoai = @MaTheLoai";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaTheLoai", matheloai);
                    command.Parameters.AddWithValue("@TenTheLoai", tentheloai);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Kiểm tra xem có bản ghi nào được cập nhật không
                        if (result > 0)
                        {
                            CategoryDAO categoryDAO = new CategoryDAO();
                            categoryBindingSource.DataSource = categoryDAO.getAllCategory();

                            dataGridView.DataSource = categoryDAO.getAllCategory();
                            MessageBox.Show("Cập nhật thông tin thể loại thành công.");


                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thể loại để cập nhật.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra khi cập nhật thông tin thể loại: " + ex.Message);
                    }
                }
            }
        }

        // xóa thông tin thể loại
        private void btnRemove_Click(object sender, EventArgs e)
        {
            string matheloai = txtMaTheLoai.Text;
            string query = "DELETE FROM TheLoai WHERE MaTheLoai = @MaTheLoai";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MaTheLoai", matheloai);
                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();

                        // Kiểm tra xem có bản ghi nào được xóa không
                        if (result > 0)
                        {
                            CategoryDAO categoryDAO = new CategoryDAO();
                            categoryBindingSource.DataSource = categoryDAO.getAllCategory();

                            dataGridView.DataSource = categoryDAO.getAllCategory();
                            MessageBox.Show("Xóa thể loại thành công.");

                        }
                        else
                        {
                            MessageBox.Show("Không tìm thấy thể loại để xóa.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Có lỗi xảy ra khi xóa thể loại: " + ex.Message);
                    }
                }
            }
        }

         // Hiển thị thông tin thể loại
        private void btnDisplayAuthorData_Click(object sender, EventArgs e)
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            categoryBindingSource.DataSource = categoryDAO.getAllCategory();

            dataGridView.DataSource = categoryDAO.getAllCategory();
        }

        // Hiển thị dữ liệu của hàng thể loại được chọn vào các trường
        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Selected)
                {

                    txtMaTheLoai.Text = dataGridView.Rows[i].Cells[0].Value.ToString();
                    txtTenTheLoai.Text = dataGridView.Rows[i].Cells[1].Value.ToString();
                }
            }
        }

        // Xóa dữ liệu ở các trường
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMaTheLoai.Text = "";
            txtTenTheLoai.Text = "";
        }

        // Tìm kiếm thể loại
        private void btnSearch_Click(object sender, EventArgs e)
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource =
                categoryDAO.getAllCategory().FindAll(elm => elm.TenTheLoai.ToLower().Contains(textSearchName) == true);
        }

        // Tìm kiếm thể loại
        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            if (e.KeyData == Keys.Enter)
                dataGridView.DataSource =
                    categoryDAO.getAllCategory().FindAll(elm => elm.TenTheLoai.ToLower().Contains(textSearchName) == true);
        }

        // Tìm kiếm thể loại
        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            CategoryDAO categoryDAO = new CategoryDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource =
                categoryDAO.getAllCategory().FindAll(elm => elm.TenTheLoai.ToLower().Contains(textSearchName) == true);
        }

        // quay về trang chủ
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Home HomeForm = new Home();
            HomeForm.Show();
        }

        private void TheLoai_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
