using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;
using BCrypt.Net;
using TalepTakip1.Models;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TalepTakip1
{
    public partial class Login : Form
    {
        private UserService userService;
        public string uName { get; private set; }

        public Login()
        {
            InitializeComponent();
            var userRepository = new UserRepository();
            this.userService = new UserService(userRepository);

            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(Login_KeyDown);
        }
        private void Login_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter tuşuna basıldığında button1_Click olayını tetikliyoruz
                guna2Button1_Click(sender, e);

                // Enter tuşuna basıldığında bip sesi çıkmasını engelliyoruz
                e.SuppressKeyPress = true;
            }
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = guna2TextBox1.Text;
            string password = guna2TextBox2.Text;

            bool isAuthenticated = userService.Login(username, password);
            if (isAuthenticated)
            {
                // Kullanıcı adını sakla
                uName = username;

                // Ana sayfaya yönlendirme kodu
                Home homeForm = new Home(uName);
                homeForm.Show();
                this.Hide(); // Login formunu gizle
            }
            else
            {
                MessageBox.Show("Geçersiz kullanıcı adı veya şifre.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox2_TextChanged(object sender, EventArgs e)
        {

        }
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel3_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }
    }
}
