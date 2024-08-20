using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;
using System.Web.Security;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using TalepTakip1.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace TalepTakip1
{
    public partial class KullaniciEkle : Form
    {
        private UserService userService;
        public string uName { get; private set; }

        public KullaniciEkle(string uName)
        {
            InitializeComponent();
            this.uName = uName;
            userService = new UserService(new UserRepository());

            // Formun KeyPreview özelliğini true yaparak formun KeyDown olayını yakalayabilmesini sağlıyoruz
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(KayitOl_KeyDown);
        }
        private void KayitOl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Enter tuşuna basıldığında button1_Click olayını tetikliyoruz
                guna2Button1_Click_1(sender, e);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void KayitOl_Load(object sender, EventArgs e)
        {
            List<Role> roles = userService.GetAllRoles();
           
            guna2ComboBox1.DataSource = roles;
            guna2ComboBox1.DisplayMember = "roleName";
            guna2ComboBox1.ValueMember = "roleID";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void KayitOl_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click_1(object sender, EventArgs e)
        {
            string isim = guna2TextBox3.Text;
            string soyisim = guna2TextBox4.Text;
            string kullaniciAdi = guna2TextBox1.Text;
            string sifre = guna2TextBox2.Text;

            if (string.IsNullOrWhiteSpace(isim) ||
                string.IsNullOrWhiteSpace(soyisim) ||
                string.IsNullOrWhiteSpace(kullaniciAdi) ||
                string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen tüm alanları doldurun.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            // Kullanıcı adı doğrulama
            if (!System.Text.RegularExpressions.Regex.IsMatch(kullaniciAdi, "^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Kullanıcı adı sadece İngilizce karakterler ve rakamlardan oluşmalıdır.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Şifreyi hashle
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(sifre);

            User yeniKullanici = new User
            {
                name = isim,
                surname = soyisim,
                userName = kullaniciAdi,
                userPassword = hashedPassword
            };

            bool isRegistered = userService.Register(yeniKullanici.userName, yeniKullanici.userPassword, yeniKullanici.name, yeniKullanici.surname);

            if (isRegistered)
            {
                // Seçili rol
                string selectedRole = guna2ComboBox1.Text;

                // Kullanıcı rolünü güncelle
                bool isRoleUpdated = userService.UpdateUserRole(yeniKullanici.userName, selectedRole);

                if (isRoleUpdated)
                {
                    MessageBox.Show("Kullanıcı başarıyla kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    guna2TextBox1.Text = "";
                    guna2TextBox2.Text = "";
                    guna2TextBox3.Text = "";
                    guna2TextBox4.Text = "";
                    guna2ComboBox1.SelectedIndex = -1;
                }
                else
                {
                    MessageBox.Show("Kullanıcı kaydedilemedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            else
            {
                MessageBox.Show("Kullanıcı kaydedilemedi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            // Kulanıcılar sayfasına yönlendirme kodu
            Users userForm = new Users(uName);
            userForm.Show();
            this.Hide();
        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {
            About aboutForm = new About();
            aboutForm.Show();
        }
    }
    
}
