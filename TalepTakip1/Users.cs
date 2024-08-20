using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TalepTakip1.Models;

namespace TalepTakip1
{
    public partial class Users : Form
    {
        private string uName { get; set; }
        private UserService userService;

        public Users(string uName)
        {
            InitializeComponent();
            this.uName = uName;
            userService = new UserService(new UserRepository());
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            KullaniciEkle kayitOlForm = new KullaniciEkle(uName);
            kayitOlForm.Show();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            Home homeForm = new Home(uName);
            homeForm.Show();
            this.Hide();
        }

        private void Users_Load(object sender, EventArgs e)
        {
            guna2HtmlLabel1.Text = $"{uName}";

            List<User> users = userService.GetAllUsers();

            // Kullanıcıları DataGridView'e ekle
            foreach (var user in users)
            {
                int rowIndex = guna2DataGridView1.Rows.Add();
                guna2DataGridView1.Rows[rowIndex].Cells["Column1"].Value = user.name; // Mevcut "İsim" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column2"].Value = user.surname; // Mevcut "Soyisim" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value = user.userName; // Mevcut "Kullanıcı Adı" sütununa ekle
                guna2DataGridView1.Rows[rowIndex].Cells["Column4"].Value = user.userRole; // Mevcut "Kullanıcı Rolü" sütununa ekle
            }

            // DataSource'u ayarladıktan veya verileri yükledikten sonra, isme göre sırala
            guna2DataGridView1.Sort(guna2DataGridView1.Columns["Column1"], System.ComponentModel.ListSortDirection.Ascending);

        }

        private void User_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        // Kullanıcıyı sil
        private void kullanıcıyıSilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Seçili satırı al
            int rowIndex = guna2DataGridView1.SelectedCells[0].RowIndex;
            string userName = guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value.ToString();

            // Kullanıcıyı sil
            bool isDeleted = userService.DeleteUser(userName);
            if (isDeleted)
            {
                MessageBox.Show("Kullanıcı başarıyla silindi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Kullanıcı silinirken bir hata oluştu.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Kullanıcıları yeniden yükle
            this.Hide();
            Users usersForm = new Users(uName);
            usersForm.Show();

        }
        // Kullanıcıyı düzenle
        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Geçerli kullanıcı adını al
            string uName = this.uName;

            // Seçili satırı al
            int rowIndex = guna2DataGridView1.SelectedCells[0].RowIndex;
            string userName = guna2DataGridView1.Rows[rowIndex].Cells["Column3"].Value.ToString();

            // Kullanıcı bilgilerini al
            User user = userService.GetUserByUsername(userName);


            this.Hide();
            Settings settingsForm = new Settings(user, uName);
            settingsForm.Show();
        }

        private void guna2HtmlLabel8_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.Show();
        }
    }
}
