using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace QLCuaHangBanSach
{
    public partial class PublisherManagement : Form
    {
        BindingSource publisherBindingSource = new BindingSource();
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public PublisherManagement()
        {
            InitializeComponent();
        }

        // Load dữ liệu nhà xuất bản khi Form được load
        private void PublisherManagement_Load(object sender, EventArgs e)
        {
            PublisherDAO publisherDAO = new PublisherDAO();
            publisherBindingSource.DataSource = publisherDAO.getAllPublisher();

            dataGridView.DataSource = publisherDAO.getAllPublisher();
        }

        // Load dữ liệu nhà xuất bản khi click vào button
        private void btnDisplayAuthorData_Click(object sender, EventArgs e)
        {
            PublisherDAO publisherDAO = new PublisherDAO();
            publisherBindingSource.DataSource = publisherDAO.getAllPublisher();

            dataGridView.DataSource = publisherDAO.getAllPublisher();
        }

        // Hiển thị thông tin nhà xuất bản khi click vào hàng được chọn
        private void dataGridView_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Selected)
                {

                    txtMaNhaXuatBan.Text = dataGridView.Rows[i].Cells[0].Value.ToString();
                    txtTenNhaXuatBan.Text = dataGridView.Rows[i].Cells[1].Value.ToString();
                    txtLienLac.Text = dataGridView.Rows[i].Cells[2].Value.ToString();
                    txtSoDienThoai.Text = dataGridView.Rows[i].Cells[3].Value.ToString();
                }
            }
        }

        // Tìm kiếm nhà xuất bản khi click vào button tìm kiếm
        private void btnSearch_Click(object sender, EventArgs e)
        {
            PublisherDAO publisherDAO = new PublisherDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource =
                publisherDAO.getAllPublisher().FindAll(elm => elm.TenNhaXuatBan.ToLower().Contains(textSearchName) == true);
        }

        // Tìm kiếm nhà xuất bản khi nhấn Enter
        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            PublisherDAO publisherDAO = new PublisherDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            if (e.KeyData == Keys.Enter)
                dataGridView.DataSource =
                publisherDAO.getAllPublisher().FindAll(elm => elm.TenNhaXuatBan.ToLower().Contains(textSearchName) == true);
        }

        // Tìm kiếm nhà xuất bản khi TextBox thay đổi giá trị
        private void txtSearchName_TextChanged(object sender, EventArgs e)
        {
            PublisherDAO publisherDAO = new PublisherDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource =
                publisherDAO.getAllPublisher().FindAll(elm => elm.TenNhaXuatBan.ToLower().Contains(textSearchName) == true);
        }

        // Thêm nhà xuất bản
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string textMaNhaXuatBan = txtMaNhaXuatBan.Text;
            string textTenNhaXuatBan = txtTenNhaXuatBan.Text;
            string textDiaChi = txtLienLac.Text;
            string textSoDienThoai = txtSoDienThoai.Text;

            // Check Form Validate
            if (textMaNhaXuatBan == "" || textTenNhaXuatBan == "" || textDiaChi == "" || textSoDienThoai == "")
            {
                MessageBox.Show("Vui lòng nhập các trường thông tin cần thiết", "Thông báo");
                return;
            }

            // Check Form Validate
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (textMaNhaXuatBan == dataGridView.Rows[i].Cells[0].Value.ToString())
                {
                    MessageBox.Show("Mã nhà xuất bản đã tồn tại. Vui lòng nhập lại", "Thông báo");
                    return;
                }
            }

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlInsert = "INSERT INTO NhaXuatBan(MaNhaXuatBan, TenNhaXuatBan, DiaChi, DienThoai) VALUES('"+ textMaNhaXuatBan +"', N'"+ textTenNhaXuatBan +"', N'"+ textDiaChi +"', '"+ textSoDienThoai +"')";

                SqlCommand sqlCommand = new SqlCommand(sqlInsert, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                PublisherDAO publisherDAO = new PublisherDAO();
                publisherBindingSource.DataSource = publisherDAO.getAllPublisher();
                dataGridView.DataSource = publisherDAO.getAllPublisher();

                MessageBox.Show("Thêm thành công", "Thông báo");

                textMaNhaXuatBan = "";
                textTenNhaXuatBan = "";
                textDiaChi = "";
                textSoDienThoai = "";
            }
            catch (Exception error)
            {
                Console.WriteLine("Insert Fail: " + error);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // Sửa nhà xuất bản
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string textMaNhaXuatBan = txtMaNhaXuatBan.Text;
            string textTenNhaXuatBan = txtTenNhaXuatBan.Text;
            string textDiaChi = txtLienLac.Text;
            string textSoDienThoai = txtSoDienThoai.Text;

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlUpdate = "UPDATE NhaXuatBan SET " +
                    "NhaXuatBan.TenNhaXuatBan = N'"+ textTenNhaXuatBan +"', " +
                    "NhaXuatBan.DiaChi = N'"+ textDiaChi +"', " +
                    "NhaXuatBan.DienThoai = '"+ textSoDienThoai +"' " +
                    "WHERE NhaXuatBan.MaNhaXuatBan = '"+ textMaNhaXuatBan +"'";

                SqlCommand sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                sqlCommand.ExecuteNonQuery(); 

                PublisherDAO publisherDAO = new PublisherDAO();
                publisherBindingSource.DataSource = publisherDAO.getAllPublisher();
                dataGridView.DataSource = publisherDAO.getAllPublisher();

                if (sqlCommand.ExecuteNonQuery() > 0)
                    MessageBox.Show("Sửa thành công", "Thông báo");
                else
                    MessageBox.Show("Sửa thất bại", "Thông báo");
            }
            catch (Exception errors)
            {
                Console.WriteLine("Update Fail: " + errors);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // Xóa nhà xuất bản
        private void btnRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu không", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);

                try
                {
                    sqlConnection.Open();

                    for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                    {
                        string publisherId = dataGridView.SelectedRows[i].Cells[0].Value.ToString();
                        string sqlDelete = "delete from NhaXuatBan where NhaXuatBan.MaNhaXuatBan = '" + publisherId + "'";

                        SqlCommand sqlCommand = new SqlCommand(sqlDelete, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }
                    PublisherDAO publisherDAO = new PublisherDAO();
                    publisherBindingSource.DataSource = publisherDAO.getAllPublisher();
                    dataGridView.DataSource = publisherDAO.getAllPublisher();

                    MessageBox.Show("Xóa thành công", "Thông báo");

                    txtMaNhaXuatBan.Text = "";
                    txtTenNhaXuatBan.Text = "";
                    txtLienLac.Text = "";
                    txtSoDienThoai.Text = "";

                    
                }
                catch (Exception errors)
                {
                    Console.WriteLine("Delete Fail: " + errors);
                }
                finally
                {
                    sqlConnection.Close();
                }
            }
        }

        // Xuất File Text
        private void btnExport_Click(object sender, EventArgs e)
        {
            string filePath = "D://TaiLieuDaiHoc/Project/C-Sharp-Project---QLCuaHangBanSach/QLCuaHangBanSach/FileExport/PublisherList.txt";

            string timeExport = DateTime.Now.ToString();
            File.AppendAllText(filePath, "Thời gian xuất File: " + timeExport + "\n");
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                for (int j = 0; j < dataGridView.Columns.Count; j++)
                {
                    string textSave = dataGridView.Rows[i].Cells[j].Value.ToString() + "\t";
                    File.AppendAllText(filePath, textSave);

                    if (j != dataGridView.Columns.Count)
                    {
                        File.AppendAllText(filePath, "| ");
                    }
                }
                File.AppendAllText(filePath,
                    "\n--------------------------------------------------------------------------------------\n"
                );
            }
            File.AppendAllText(filePath, "\n\n");
            MessageBox.Show("Xuất File thành công", "Thông báo");
        }

        // Xóa dữ liệu trong trường
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMaNhaXuatBan.Text = "";
            txtTenNhaXuatBan.Text = "";
            txtLienLac.Text = "";
            txtSoDienThoai.Text = "";
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Views.Home HomeForm = new Views.Home();
            HomeForm.Show();
        }

        private void PublisherManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
