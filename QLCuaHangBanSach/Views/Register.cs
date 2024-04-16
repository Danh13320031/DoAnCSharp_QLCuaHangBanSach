using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    public partial class Register : Form
    {
        BindingSource signInBindingSource = new BindingSource();
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public Register()
        {
            InitializeComponent();
        }

        // Đăng ký
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string textUserNameRg = txtUserNameRg.Text;
            string textPassRg = txtPassRg.Text;

            // Check Form Validate
            if (textUserNameRg == "" || textPassRg == "")
            {
                MessageBox.Show("Vui lòng điền thông tin vào các trường", "Thông báo");
                return;
            }

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlSignIn = "select top(1) users.username from users where " +
                    "users.username = '" + textUserNameRg + "'";

                SqlCommand sqlCommand = new SqlCommand(sqlSignIn, sqlConnection);

                // Check Form Validate
                if (sqlCommand.ExecuteScalar() == null)
                {
                    string sqlRegister = "INSERT INTO users(username, password) VALUES('" + textUserNameRg + "', '" + textPassRg + "')";
                    SqlCommand sqlCommandChange = new SqlCommand(sqlRegister, sqlConnection);
                    SqlDataReader readerChange = sqlCommandChange.ExecuteReader();

                    MessageBox.Show("Đăng ký thành công", "Thông báo");

                    this.Hide();
                    SignIn SignInForm = new SignIn();
                    SignInForm.Show();
                } else
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    txtUserNameRg.Text = "";
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        // Hiển thị Form Login
        private void btnBackRg_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignIn SignInForm = new SignIn();
            SignInForm.Show();
        }

        // Thoát chương trình
        private void Register_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
