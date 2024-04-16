using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace QLCuaHangBanSach
{
    public partial class SignIn : Form
    {
        BindingSource signInBindingSource = new BindingSource();
        string connectionString = @"Data Source=DESKTOP-UQ06DIF;Initial Catalog=SQLQuery3.sql;Integrated Security=True";

        public SignIn()
        {
            InitializeComponent();
        }

        // Đăng nhập
        private void btnSignIn_Click(object sender, EventArgs e)
        {
            string textUserNameSi = txtUserNameSi.Text;
            string textPassSi = txtPassSi.Text;

            if (textUserNameSi == "" || textPassSi == "")
            {
                MessageBox.Show("Vui lòng điền thông tin tài khoản và mật khẩu", "Thông báo");
                return;
            }

            SqlConnection sqlConnection = new SqlConnection(connectionString);
            try
            {
                sqlConnection.Open();

                string sqlSignIn = "select top(1) * from users where " +
                    "users.username = '"+ textUserNameSi +"' and " +
                    "users.password = '"+ textPassSi +"'";

                SqlCommand sqlCommand = new SqlCommand(sqlSignIn, sqlConnection);
                if (sqlCommand.ExecuteScalar() == null)
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!");
                    return;
                } else
                {
                    MessageBox.Show("Đăng nhập thành công", "Thông báo");
                    this.Hide();
                    Views.Home HomeForm = new Views.Home();
                    HomeForm.Show();
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

        // Hiển thị Form ForgotPassword
        private void linkForgotPass_Click(object sender, EventArgs e)
        {
            this.Hide();
            ForgotPassword ForgotPasswordForm = new ForgotPassword();
            ForgotPasswordForm.Show();
        }

        // Thoát khỏi ứng dụng khi click vào button Close
        private void btnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        // Thoát khỏi ứng dụng
        private void SignIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        // Hiển thị Form Register
        private void txtRegister_Click(object sender, EventArgs e)
        {
            this.Hide();
            Register RegisterForm = new Register();
            RegisterForm.Show();
        }
    }
}
