using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    public partial class ForgotPassword : Form
    {
        BindingSource signInBindingSource = new BindingSource();
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void btnBackRg_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignIn SignInForm = new SignIn();
            SignInForm.Show();
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            string textUserName = txtUserName.Text;
            string textOldPass = txtOldPass.Text;
            string textNewPass = txtNewPass.Text;
            string textConfNewPass = txtConfNewPass.Text;

            if (textUserName == "" || textOldPass == "" || textNewPass == "" || textConfNewPass == "")
            {
                MessageBox.Show("Vui lòng điền thông tin vào các trường", "Thông báo");
                return;
            }

            if (textNewPass != textConfNewPass)
            {
                MessageBox.Show("Xác nhận mật khẩu không chính xác", "Thông báo");
                return;
            }

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlSignIn = "select top(1) * from users where " +
                    "users.username = '" + textUserName + "' and " +
                    "users.password = '" + textOldPass + "'";

                SqlCommand sqlCommand = new SqlCommand(sqlSignIn, sqlConnection);

                if (sqlCommand.ExecuteScalar() == null)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                    return;
                }

                string sqlForgotPass = "UPDATE users SET users.password = '"+ textNewPass + "' WHERE users.username = '"+ textUserName + "' and users.password = '"+ textOldPass + "'";
                SqlCommand sqlCommandChange = new SqlCommand(sqlForgotPass, sqlConnection);
                SqlDataReader readerChange = sqlCommandChange.ExecuteReader();

                txtUserName.Text = "";
                txtOldPass.Text = "";
                txtNewPass.Text = "";
                txtConfNewPass.Text = "";

                MessageBox.Show("Đổi mật khẩu thành công", "Thông báo");
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

        private void ForgotPassword_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
