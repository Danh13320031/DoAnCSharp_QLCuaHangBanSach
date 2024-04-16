using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace QLCuaHangBanSach
{
    public partial class AuthorManagement : Form
    {
        BindingSource authorBindingSource = new BindingSource();

        public AuthorManagement()
        {
            InitializeComponent();
        }


        // Hiển thị dữ liệu tác giả khi Form được Load
        private void AuthorManager_Load(object sender, EventArgs e)
        {
            AuthorDAO authorDAO = new AuthorDAO();
            authorBindingSource.DataSource = authorDAO.getAllAuthor();

            dataGridView.DataSource = authorDAO.getAllAuthor();
        }


        // Hiển thị dữ liệu tác gải khi kích vào button
        private void btnDisplayAuthorData_Click(object sender, EventArgs e)
        {
            AuthorDAO authorDAO = new AuthorDAO();
            authorBindingSource.DataSource = authorDAO.getAllAuthor();

            dataGridView.DataSource = authorDAO.getAllAuthor();
        }


        // Hiển thị dữ liệu của hàng tác giả được chọn vào các trường
        private void listViewDataAuthor_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Selected)
                {

                    txtMaTacGia.Text = dataGridView.Rows[i].Cells[0].Value.ToString();
                    txtTenTacGia.Text = dataGridView.Rows[i].Cells[1].Value.ToString();
                    txtLienLac.Text = dataGridView.Rows[i].Cells[2].Value.ToString();
                }
            }
        }

        // Tìm kiếm tác giả
        private void btnSearchAthor_Click(object sender, EventArgs e)
        {
            AuthorDAO authorDAO = new AuthorDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource =
                authorDAO.getAllAuthor().FindAll(elm => elm.TenTacGia.ToLower().Contains(textSearchName) == true);
        }

        // Tìm kiếm tác giả
        private void txtSearchNameAuthor_KeyDown(object sender, KeyEventArgs e)
        {
            AuthorDAO authorDAO = new AuthorDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            if (e.KeyData == Keys.Enter)
                dataGridView.DataSource =
                    authorDAO.getAllAuthor().FindAll(elm => elm.TenTacGia.ToLower().Contains(textSearchName) == true);
        }

        // Tìm kiếm tác giả
        private void txtSearchNameAuthor_TextChanged(object sender, EventArgs e)
        {
            AuthorDAO authorDAO = new AuthorDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource =
                authorDAO.getAllAuthor().FindAll(elm => elm.TenTacGia.ToLower().Contains(textSearchName) == true);
        }

        // Thêm dữ liệu tác giả
        private void btnAddAuthor_Click(object sender, EventArgs e)
        {
            string textMaTacGia = txtMaTacGia.Text;
            string textTenTacGia = txtTenTacGia.Text;
            string textLienLac = txtLienLac.Text;

            // Check Form Validate
            if (textMaTacGia == "" || textTenTacGia == "" || textLienLac == "" ||  textMaTacGia == "")
            {
                MessageBox.Show("Vui lòng nhập các trường thông tin cần thiết", "Thông báo");
                return;
            }

            // Check Form Validate
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (textMaTacGia == dataGridView.Rows[i].Cells[0].Value.ToString())
                {
                    MessageBox.Show("Mã tác giả không được trùng nhau. Vui lòng nhập lại", "Thông báo");
                    return;
                }
            }

            string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlInsert = "insert into [dbo].[TacGia]([MaTacGia],[TenTacGia],[LienLac]) values" +
                    "('" + textMaTacGia + "', N'" + textTenTacGia + "', N'" + textLienLac + "');";
                SqlCommand sqlCommand = new SqlCommand(sqlInsert, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                textMaTacGia = "";
                textTenTacGia = "";
                textLienLac = "";

                AuthorDAO authorDAO = new AuthorDAO();
                authorBindingSource.DataSource = authorDAO.getAllAuthor();
                dataGridView.DataSource = authorDAO.getAllAuthor();

                MessageBox.Show("Thêm thành công", "Thông báo");
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


        // Sửa dữ liệu tác giả
        private void btnInsertAuthor_Click(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string textMaTacGia = txtMaTacGia.Text;
            string textTenTacGia = txtTenTacGia.Text;
            string textLienLac = txtLienLac.Text;

            try
            {
                sqlConnection.Open();

                string sqlUpdate = "update TacGia set " +
                    "TacGia.TenTacGia = N'" + textTenTacGia + "', " +
                    "TacGia.LienLac = N'" + textLienLac + "' " +
                    "where TacGia.MaTacGia = '" + textMaTacGia + "';";

                SqlCommand sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                AuthorDAO authorDAO = new AuthorDAO();
                authorBindingSource.DataSource = authorDAO.getAllAuthor();
                dataGridView.DataSource = authorDAO.getAllAuthor();

                MessageBox.Show("Sửa thành công", "Thông báo");
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


        // Xóa dữ liệu tác giả
        private void btnRemoveAuthor_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa dữ liệu không", "Thông báo", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";
                SqlConnection sqlConnection = new SqlConnection(connectionString);

                try
                {
                    sqlConnection.Open();

                    for (int i = 0; i < dataGridView.SelectedRows.Count; i++)
                    {
                        string authorId = dataGridView.SelectedRows[i].Cells[0].Value.ToString();
                        string sqlDelete = "delete from TacGia where TacGia.MaTacGia = '" + authorId + "'";

                        SqlCommand sqlCommand = new SqlCommand(sqlDelete, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }

                    AuthorDAO authorDAO = new AuthorDAO();
                    authorBindingSource.DataSource = authorDAO.getAllAuthor();
                    dataGridView.DataSource = authorDAO.getAllAuthor();

                    MessageBox.Show("Xóa thành công", "Thông báo");
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
        private void btnExportAuthor_Click(object sender, EventArgs e)
        {
            string filePath = "D://TaiLieuDaiHoc/Project/C-Sharp-Project---QLCuaHangBanSach/QLCuaHangBanSach/FileExport/AuthorList.txt";

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
                    "\n----------------------------------------------------\n"
                );
            }
            File.AppendAllText(filePath, "\n\n");
            MessageBox.Show("Xuất File thành công", "Thông báo");
        }

        private void btnClearAuthor_Click(object sender, EventArgs e)
        {
            txtMaTacGia.Text = "";
            txtTenTacGia.Text = "";
            txtLienLac.Text = "";
        }

        private void lstFilterAuthor_Leave(object sender, EventArgs e)
        {
            for (int i = 0; i < lstFilter.Items.Count; i++)
            {
                lstFilter.SetSelected(i, false);
            }
        }

        private void lstFilterAuthor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFilter.SelectedItem == null) return;
            int itemSelect = lstFilter.SelectedIndex + 1;
            AuthorDAO authorDAO = new AuthorDAO();

            Console.WriteLine(itemSelect);

            switch (itemSelect)
            {
                case 1:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Việt Nam");
                    break;
                case 2:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Đức");
                    break;
                case 3:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Nhật Bản");
                    break;
                case 4:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Cannada");
                    break;
                case 5:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Pháp");
                    break;
                case 6:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Anh");
                    break;
                case 7:
                    dataGridView.DataSource = authorDAO.getAllAuthor().FindAll(elm => elm.LienLac == "Hàn Quốc");
                    break;
                default:
                    dataGridView.DataSource = authorDAO.getAllAuthor();
                    break;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Views.Home HomeForm = new Views.Home();
            HomeForm.Show();
        }

        private void AuthorManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
