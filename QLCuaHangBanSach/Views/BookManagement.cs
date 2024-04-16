using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace QLCuaHangBanSach
{
    public partial class BookManagement : Form
    {
        BindingSource bookBindingSource = new BindingSource();
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public BookManagement()
        {
            InitializeComponent();
        }


        private void BookManagement_Load(object sender, EventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            bookBindingSource.DataSource = bookDAO.getAllBook();

            dataGridView.DataSource = bookDAO.getAllBook();
        }


        // Hiển thị dữ liệu Sách vào dataGridView khi nhấn vào button Load Book Data
        private void displayBookData_Click(object sender, EventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            bookBindingSource.DataSource = bookDAO.getAllBook();

            dataGridView.DataSource = bookDAO.getAllBook();
        }


        // Hiển thị dữ liệu của hàng sách được chọn vào các trường
        private void listViewData_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (dataGridView.Rows[i].Selected)
                {

                    txtMaSach.Text = dataGridView.Rows[i].Cells[0].Value.ToString();
                    txtTenSach.Text = dataGridView.Rows[i].Cells[1].Value.ToString();
                    txtSoLuongTon.Text = dataGridView.Rows[i].Cells[2].Value.ToString();
                    txtMaTheLoai.Text = dataGridView.Rows[i].Cells[3].Value.ToString();
                    txtMaNhaXuatBan.Text = dataGridView.Rows[i].Cells[4].Value.ToString();
                    txtMaTacGia.Text = dataGridView.Rows[i].Cells[5].Value.ToString();
                }
            }
        }

        // Tìm kiếm dữ liệu Sách khi nhấn vào button Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            string textSearchName = txtSearchName.Text;

            dataGridView.DataSource = 
                bookDAO.getAllBook().FindAll(elm => elm.TenSach.Contains(textSearchName) == true);
        }

        // Tìm kiếm dữ liệu Sách khi nhấn Enter
        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            string textSearchName = txtSearchName.Text;
            
            if (e.KeyData == Keys.Enter)
                dataGridView.DataSource = 
                    bookDAO.getAllBook().FindAll(elm => elm.TenSach.Contains(textSearchName) == true);
            
        }


        // Tìm kiếm dữ liệu Sách khi TextBox thay đổi giá trị
        private void txtTextSearch_TextChanged(object sender, EventArgs e)
        {
            BookDAO bookDAO = new BookDAO();
            string textSearchName = txtSearchName.Text.ToLower();

            dataGridView.DataSource = 
                bookDAO.getAllBook().FindAll(elm => elm.TenSach.ToLower().Contains(textSearchName) == true);
        }


        // Thêm dữ liệu Sách
        private void btnAdd_Click(object sender, EventArgs e)
        {
            string textMaSach = txtMaSach.Text;
            string textTenSach = txtTenSach.Text;
            string textSoLuongTon = txtSoLuongTon.Text;
            string textMaTheLoai = txtMaTheLoai.Text;
            string textMaNhaXuatBan = txtMaNhaXuatBan.Text;
            string textMaTacGia = txtMaTacGia.Text;

            // Check Form Validate
            if (textMaSach == "" || textTenSach == "" || textMaTheLoai == "" || textMaNhaXuatBan == "" || textMaTacGia == "" || textSoLuongTon == "")
            {
                MessageBox.Show("Vui lòng nhập các trường thông tin cần thiết", "Thông báo");
                return;
            }

            // Check Form Validate
            if (int.Parse(textSoLuongTon) < 0)
            {
                MessageBox.Show("Yêu cầu số lượng không được bé hơn 0", "Thông báo");
                return;
            }

            // Check Form Validate
            for (int i = 0; i < dataGridView.Rows.Count; i++)
            {
                if (textMaSach == dataGridView.Rows[i].Cells[0].Value.ToString())
                {
                    MessageBox.Show("Mã sách không được trùng nhau. Vui lòng nhập lại", "Thông báo");
                    return;
                }
            }

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlInsert = "insert into [dbo].[Sach]([MaSach],[TenSach],[SoLuongTon],[MaTheLoai],[MaNhaXuatBan],[MaTacGia]) values" +
                    "('" + textMaSach + "',N'" + textTenSach + "'," + textSoLuongTon + ",'" + textMaTheLoai + "','" + textMaNhaXuatBan + "','" + textMaTacGia + "');";
                SqlCommand sqlCommand = new SqlCommand(sqlInsert, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                textMaSach = "";
                textTenSach = "";
                textSoLuongTon = "";
                textMaTheLoai = "";
                textMaNhaXuatBan = "";
                textMaTacGia = "";

                BookDAO bookDAO = new BookDAO();
                bookBindingSource.DataSource = bookDAO.getAllBook();
                dataGridView.DataSource = bookDAO.getAllBook();

                MessageBox.Show("Thêm thành công", "Thông báo");
            } catch (Exception error)
            {
                Console.WriteLine("Insert Fail: " + error);
            } finally
            {
                sqlConnection.Close();
            }
        }


        // Sửa dữ liệu Sách
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            string textMaSach = txtMaSach.Text;
            string textTenSach = txtTenSach.Text;
            string textSoLuongTon = txtSoLuongTon.Text;
            string textMaTheLoai = txtMaTheLoai.Text;
            string textMaNhaXuatBan = txtMaNhaXuatBan.Text;
            string textMaTacGia = txtMaTacGia.Text;

            try
            {
                sqlConnection.Open();

                string sqlUpdate = "update Sach set " + 
                    "Sach.TenSach = N'"+ textTenSach +"', " +
                    "Sach.SoLuongTon = "+ int.Parse(textSoLuongTon) + ", " +
                    "Sach.MaTheLoai = '"+ textMaTheLoai +"', " +
                    "Sach.MaNhaXuatBan = '"+ textMaNhaXuatBan +"', " +
                    "Sach.MaTacGia = '"+ textMaTacGia +"' " +
                    "where Sach.MaSach = '"+ textMaSach +"';";

                SqlCommand sqlCommand = new SqlCommand(sqlUpdate, sqlConnection);
                sqlCommand.ExecuteNonQuery();

                BookDAO bookDAO = new BookDAO();
                bookBindingSource.DataSource = bookDAO.getAllBook();
                dataGridView.DataSource = bookDAO.getAllBook();

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


        // Xóa dữ liệu Sách
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
                        string bookId = dataGridView.SelectedRows[i].Cells[0].Value.ToString();
                        string sqlDelete = "delete from Sach where Sach.MaSach = '" + bookId + "'";

                        SqlCommand sqlCommand = new SqlCommand(sqlDelete, sqlConnection);
                        sqlCommand.ExecuteNonQuery();
                    }

                    BookDAO bookDAO = new BookDAO();
                    bookBindingSource.DataSource = bookDAO.getAllBook();
                    dataGridView.DataSource = bookDAO.getAllBook();

                    txtMaSach.Text = "";
                    txtTenSach.Text = "";
                    txtSoLuongTon.Text = "";
                    txtMaTheLoai.Text = "";
                    txtMaNhaXuatBan.Text = "";
                    txtMaTacGia.Text = "";

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


        // Xuất File Text Sách
        private void btnExport_Click(object sender, EventArgs e)
        {
            string filePath = "D://TaiLieuDaiHoc/Project/C-Sharp-Project---QLCuaHangBanSach/QLCuaHangBanSach/FileExport/BookList.txt";

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


        // Xóa dữ liệu Sách trong các trường
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtMaSach.Text = "";
            txtTenSach.Text = "";
            txtSoLuongTon.Text = "";
            txtMaTheLoai.Text = "";
            txtMaNhaXuatBan.Text = "";
            txtMaTacGia.Text = "";
        }


        // Lọc dữ liệu Sách
        private void lstFilter_Leave(object sender, EventArgs e)
        {
            for (int i = 0; i < lstFilter.Items.Count; i++)
            {
                lstFilter.SetSelected(i, false);
            }
        }
        private void lstFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFilter.SelectedItem == null) return;
            int itemSelect = lstFilter.SelectedIndex + 1;
            BookDAO bookDAO = new BookDAO();

            Console.WriteLine(itemSelect);

            switch(itemSelect)
            {
                case 1:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL1");
                    break;
                case 2:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL2");
                    break;
                case 3:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL3");
                    break;
                case 4:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL4");
                    break;
                case 5:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL5");
                    break;
                case 6:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL6");
                    break;
                case 7:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL7");
                    break;
                case 8:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL8");
                    break;
                case 9:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TL9");
                    break;
                case 10:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TLa1");
                    break;
                case 11:
                    dataGridView.DataSource = bookDAO.getAllBook().FindAll(elm => elm.MaTheLoai == "TLa2");
                    break;
                default:
                    dataGridView.DataSource = bookDAO.getAllBook();
                    break;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            Views.Home HomeForm = new Views.Home();
            HomeForm.Show();
        }

        private void BookManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        
    }
}
