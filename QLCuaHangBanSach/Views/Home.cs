using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCuaHangBanSach.Views
{
    public partial class Home : Form
    {
        public Home()
        {
            InitializeComponent();
        }

        private void btnAuthor_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuthorManagement AuthorManagementForm = new AuthorManagement();
            AuthorManagementForm.Show();
        }

        private void btnBook_Click(object sender, EventArgs e)
        {
            this.Hide();
            BookManagement BookManagementForm = new BookManagement();
            BookManagementForm.Show();
        }

        private void btnPublisher_Click(object sender, EventArgs e)
        {
            this.Hide();
            PublisherManagement PublisherManagementForm = new PublisherManagement();
            PublisherManagementForm.Show();
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            this.Hide();
            SignIn SignInForm = new SignIn();
            SignInForm.Show();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            TheLoai tl = new TheLoai();
            tl.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            PhieuNhapSach pn = new PhieuNhapSach();
            pn.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChiTietPhieuNhap ctpn = new ChiTietPhieuNhap();
            ctpn.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            HoaDon hd = new HoaDon();
            hd.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            ChiTietHoaDon cthd = new ChiTietHoaDon();
            cthd.Show();
        }
    }
}
