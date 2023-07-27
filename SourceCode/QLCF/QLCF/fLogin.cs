using QLCF.DAO;
using QLCF.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCF
{
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }

        private void cbxHienThiMatKhau_CheckedChanged(object sender, EventArgs e)
        {
            if (cbxHienThiMatKhau.Checked)
                txtMatKhau.UseSystemPasswordChar = false;

            if (!cbxHienThiMatKhau.Checked)
                txtMatKhau.UseSystemPasswordChar = true;
        }

        private void bntThoat_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát chương trình ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                this.Close();
            }
        }

        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát chương trình ?", "Thông báo" , MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK){
                e.Cancel = true;
            }
        }

        private void bntDangNhap_Click(object sender, EventArgs e)
        {
            string userName = txtTaiKhoan.Text;
            string password = txtMatKhau.Text;
            if (Login(userName, password))
            {
                Account loginAccount = AccountDAO.Instance.GetAccountByUserName(userName);
                fMain f = new fMain(loginAccount);
                this.Hide();
                f.ShowDialog();
                this.Show();
            }
            else 
            {
                MessageBox.Show("Sai tên tài khoản hoặc mật khẩu!", "Thông báo");
            }
        }
        bool Login(string userName, string passWord)
        {
             
            return AccountDAO.Instance.Login(userName, passWord);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn muốn thoát chương trình ?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != System.Windows.Forms.DialogResult.No)
            {
                this.Close();
            }
        }
    }
}
