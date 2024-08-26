using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TalepTakip
{
    public partial class DatabaseConn : Form
    {
        public DatabaseConn()
        {
            InitializeComponent();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            // Kullanıcıdan alınan bilgiler
            string server = guna2TextBox1.Text;
            string database = guna2TextBox2.Text;

            // Connection string'i dinamik olarak oluşturma
            string connectionString = $"Server={server};Database={database};User Id=root;Password=;Pooling=true;SslMode=none;";

            try
            {
                // MySQL bağlantısını açma
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MessageBox.Show("Bağlantı başarılı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda mesaj gösterme
                MessageBox.Show($"Bağlantı hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Login loginform = new Login();
            loginform.Show();
            this.Hide();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Login loginform = new Login();
            loginform.Show();
            this.Hide();
        }
    }
}
