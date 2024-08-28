using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Windows.Forms;
using BCrypt.Net;
using TalepTakip.Models;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TalepTakip
{
    public partial class Login : Form
    {
        private UserService userService;
        private NotificationService notificationService;
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string username = usernameTextBox.Text;
            string password = passwordTextBox.Text;

            bool isAuthenticated = userService.Login(username, password);
            if (isAuthenticated)
            {
                // Kullanıcı adını sakla
                uName = username;
                User user = userService.GetUserByUsername(username);
                if (user.userRole == "Yönetici")
                {
                    // Bildirim servisini başlat
                    notificationService = new NotificationService(userService);
                    notificationService.Start();
                }

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
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }
    }
}
