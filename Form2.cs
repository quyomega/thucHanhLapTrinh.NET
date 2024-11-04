using System;
using System.Windows.Forms;

namespace baitaplon
{
    public partial class Form2 : Form
    {
        private LoginForm loginForm; // Tham chiếu đến LoginForm

        public Form2(LoginForm form)
        {
            InitializeComponent();
            this.loginForm = form; // Gán tham chiếu
        }
        private void Form2_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            loginForm.Show();
        }
    }
}
