using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TalepTakip1.Models;

namespace TalepTakip1
{
    public partial class Settings : Form
    {
        public string uName { get; private set; }
        private UserService userService;
        private User user;
        private string currentUser;

        public Settings(User user, string currentUser)
        {
            InitializeComponent();
            this.user = user;
            userService = new UserService(new UserRepository());
            this.currentUser = currentUser;
            Settings_Load(this, null);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            List<Role> roles = userService.GetAllRoles();

            guna2ComboBox1.DataSource = roles;
            guna2ComboBox1.DisplayMember = "roleName";
            guna2ComboBox1.ValueMember = "roleID";

            if (user != null)
            {
                guna2TextBox1.Text = user.name;
                guna2TextBox7.Text = user.surname;
                guna2TextBox2.Text = user.userName;
                guna2ComboBox1.Text = user.userRole;
                guna2TextBox4.Text = ""; // şifre alanını boşalt
            }
            else
            {
                MessageBox.Show("Kullanıcı bilgileri yüklenemedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }


        private void Setting_Closing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Users usersForm = new Users(currentUser);
            usersForm.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox4.Text != "")
            {
                if (guna2TextBox5.Text == "" || guna2TextBox6.Text == "")
                {
                    MessageBox.Show("Lütfen Yeni Şifre Alanlarını doldurun.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (user != null)
            {
                // Kullanıcı bilgilerini güncelle
                user.name = guna2TextBox1.Text;
                user.surname = guna2TextBox7.Text;
                user.userRole = guna2ComboBox1.Text;

                // Şifre kontrolü
                if (!string.IsNullOrWhiteSpace(guna2TextBox4.Text))
                {
                    // Mevcut şifreyi kontrol et
                    if (BCrypt.Net.BCrypt.Verify(guna2TextBox4.Text, user.userPassword))
                    {
                        // Yeni şifrelerin aynı olup olmadığını kontrol et
                        if (guna2TextBox5.Text == guna2TextBox6.Text)
                        {
                            // Yeni şifreyi hash'le ve kaydet
                            user.userPassword = BCrypt.Net.BCrypt.HashPassword(guna2TextBox6.Text);
                        }
                        else
                        {
                            MessageBox.Show("Yeni şifreler eşleşmiyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mevcut şifre yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                //Değişiklikleri veritabanına kaydet
                bool isUpdated = userService.UpdateUser(user);

                if (isUpdated)
                {
                    MessageBox.Show("Kullanıcı bilgileri başarıyla güncellendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2TextBox4.Text = "";
                    guna2TextBox5.Text = "";
                    guna2TextBox6.Text = "";
                }
                else
                {
                    MessageBox.Show("Kullanıcı bilgileri güncellenemedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

                
            }
        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}
